namespace RentalApp.Models.Price
{
    public class TruckPrice : IPrice
    {
        public int Km { get; set; }
        public double BaseKmPrice { get; set; }
        public TruckPrice(int km, double baseKmPrice ) {
            Km = km;
            BaseKmPrice = baseKmPrice;
        }

        public double CalculatePrice(int days, double baseDayPrice)
        {
            return days * baseDayPrice * 1.5 + Km * BaseKmPrice* 1.5;
        }
    }
}
