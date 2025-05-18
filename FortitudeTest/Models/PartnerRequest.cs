﻿namespace FortitudeTest.Models
{
    public class PartnerRequest
    {
        public string PartnerKey { get; set; }
        public string PartnerRefNo { get; set; }
        public string PartnerPassword { get; set; }
        public long TotalAmount { get; set; }
        public List<ItemDetail>? Items { get; set; }
        public string Timestamp { get; set; }
        public string Sig { get; set; }
    }
}
