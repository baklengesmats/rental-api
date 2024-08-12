namespace RentalApp.Models.Price
{
    public class SmallCarPrice : IPrice
    {
        public double CalculatePrice(int days, double baseDayPrice)
        {
            return days * baseDayPrice;
        }
    }
}
