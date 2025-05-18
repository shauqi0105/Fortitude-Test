namespace FortitudeTest.Services
{
    public class DiscountService
    {
        public static (long totalDiscount, long finalAmount) CalculateDiscount(long totalAmount)
        {
            double discountPercentage = 0;


            if (totalAmount >= 20000 && totalAmount <= 50000)
                discountPercentage += 0.05;
            else if (totalAmount >= 50100 && totalAmount <= 80000) 
                discountPercentage += 0.07;
            else if (totalAmount >= 80100 && totalAmount <= 120000)
                discountPercentage += 0.10;
            else if (totalAmount > 120000)
                discountPercentage += 0.15;

            if (totalAmount > 50000 && IsPrime(totalAmount))
                discountPercentage += 0.08;

            if (totalAmount > 90000 && totalAmount % 10 == 5)
                discountPercentage += 0.10;

            if (discountPercentage > 0.20)
                discountPercentage = 0.20;

            long totalDiscount = (long)(totalAmount * discountPercentage);
            long finalAmount = totalAmount - totalDiscount;

            return (totalDiscount, finalAmount);
        }

        private static bool IsPrime(long number)
        {
            if (number <= 1) 
                return false;
            if (number == 2 || number == 3)
                return true;
            if (number % 2 == 0)
                return false;
            for (long i = 3; i * i <= number; i += 2)
                if (number % i == 0)
                    return false;
            return true;
        }
    }
}
