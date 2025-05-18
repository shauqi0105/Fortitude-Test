using FortitudeTest.Models;
using FortitudeTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using log4net;

namespace FortitudeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmitTrxMessageController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SubmitTrxMessageController));

        [HttpPost]
        public IActionResult Post([FromBody] PartnerRequest request)
        {
            try
            {
                log.Info($"[REQUEST] PartnerRefNo: {request?.PartnerRefNo}, PartnerKey: {request?.PartnerKey}, Password: {MaskPassword(request?.PartnerPassword)}, TotalAmount: {request?.TotalAmount}");

                if (request == null)
                    return LogAndReturn(400, "Invalid Request");

                if (string.IsNullOrWhiteSpace(request.PartnerRefNo))
                    return LogAndReturn(400, "partnerrefno is required.");

                if (string.IsNullOrWhiteSpace(request.PartnerKey))
                    return LogAndReturn(400, "partnerkey is required.");

                if (string.IsNullOrWhiteSpace(request.PartnerPassword))
                    return LogAndReturn(400, "partnerpassword is required.");

                if (string.IsNullOrWhiteSpace(request.Timestamp))
                    return LogAndReturn(400, "timestamp is required.");

                if (string.IsNullOrWhiteSpace(request.Sig))
                    return LogAndReturn(400, "sig is required.");

                if (request.TotalAmount <= 0)
                    return LogAndReturn(400, "totalamount must be a positive value.");

                if (!DateTime.TryParse(request.Timestamp, null, DateTimeStyles.AdjustToUniversal, out DateTime parsedTimestamp))
                    return LogAndReturn(400, "Invalid timestamp format.");

                var serverTime = DateTime.UtcNow;
                if (parsedTimestamp < serverTime.AddMinutes(-5) || parsedTimestamp > serverTime.AddMinutes(5))
                    return LogAndReturn(400, "Expired.");

                if (!PartnerService.IsAuthorized(request.PartnerRefNo, request.PartnerKey, request.PartnerPassword))
                    return LogAndReturn(401, "Access Denied!");

                string timestampSig = parsedTimestamp.ToString("yyyyMMddHHmmss");
                string generatedSig = PartnerService.GenerateSignature(
                    timestampSig,
                    request.PartnerKey,
                    request.PartnerRefNo,
                    request.TotalAmount,
                    request.PartnerPassword
                );

                if (generatedSig != request.Sig)
                    return LogAndReturn(401, "Access Denied!");

                if (request.Items != null && request.Items.Any())
                {
                    long calculatedTotal = 0;

                    foreach (var item in request.Items)
                    {
                        if (string.IsNullOrWhiteSpace(item.PartnerItemRef))
                            return LogAndReturn(400, "partneritemref is required.");

                        if (string.IsNullOrWhiteSpace(item.Name))
                            return LogAndReturn(400, "name is required.");

                        if (item.Qty <= 0 || item.Qty > 5)
                            return LogAndReturn(400, "qty must be between 1 and 5.");

                        if (item.UnitPrice <= 0)
                            return LogAndReturn(400, "unitprice must be a positive value.");

                        calculatedTotal += item.Qty * item.UnitPrice;
                    }

                    if (calculatedTotal != request.TotalAmount)
                        return LogAndReturn(400, "Invalid Total Amount.");
                }

                var (discount, final) = DiscountService.CalculateDiscount(request.TotalAmount);

                var response = new PartnerResponse
                {
                    Result = 1,
                    TotalAmount = request.TotalAmount,
                    TotalDiscount = discount,
                    FinalAmount = final
                };

                log.Info($"[RESPONSE] Success - TotalAmount: {response.TotalAmount}, Discount: {response.TotalDiscount}, Final: {response.FinalAmount}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                log.Error("Unhandled exception occurred", ex);
                return StatusCode(500, new PartnerResponse { Result = 0, ResultMessage = "Internal Server Error" });
            }
        }

        private IActionResult LogAndReturn(int statusCode, string message)
        {
            log.Warn($"[ERROR] {message}");
            return StatusCode(statusCode, new PartnerResponse { Result = 0, ResultMessage = message });
        }

        private string MaskPassword(string? base64Password)
        {
            return string.IsNullOrEmpty(base64Password) ? "" : "***";
        }
    }
}
