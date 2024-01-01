namespace Aircraft_project.Models
{
    public class Aircraft
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Color { get; set; }
        public int PassengerCapacity { get; set; }
        public int MaxSpeed { get; set; }
        public int FuelCapacity { get; set; }
        public string EngineType { get; set; }
        public int Range { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public string AdditionalInformation { get; set; }

        public string ImagePath { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }

    }
}
