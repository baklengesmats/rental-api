namespace RentalApp.Models.Price
{
    public class CombiCarPrice : IPrice
    {
        public int Km { get; set; }
        public double BaseKmPrice { get; set; }

        public CombiCarPrice(int km, double baseKmPrice)
        { 
            Km = km;
            BaseKmPrice = baseKmPrice;
        }

        public double CalculatePrice(int days, double baseDayPrice)
        {
            return days * baseDayPrice * 1.3 + Km * BaseKmPrice;
        }
    }
}
