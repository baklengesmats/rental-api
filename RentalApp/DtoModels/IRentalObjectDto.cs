namespace RentalApp.DtoModels
{
    public interface IRentalObjectDto
    {
        public string RegistrationId { get; set; }
        public bool IsAvailable { get; set; }
        public string Type { get; set; }
    }
}
