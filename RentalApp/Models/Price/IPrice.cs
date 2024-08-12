namespace RentalApp.Models.Price
{
    public interface IPrice
    {
        public double CalculatePrice(int days, double baseDayPrice);
    }
}
