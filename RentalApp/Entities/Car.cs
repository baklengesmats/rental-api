namespace RentalApp.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string RegistrationId { get; set; }
        public bool IsAvailable { get; set; }
        public CarType Type { get; set; }
        public int Km {  get; set; }
        public double BaseDayPrice { get; set; }
        public double? BaseKmPrice { get; set; }
    }

    public enum CarType
    {
        SmallCar,
        CombiCar,
        Truck
    }
}
