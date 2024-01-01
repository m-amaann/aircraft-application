using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace Aircraft_project.Models
{
    public class Aircraft
    {
        [Key]
        [Required]
        public int AircraftId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required] 
        public string Image { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public int PassengerCapacity { get; set; }

        [Required]
        public int MaxSpeed { get; set; }

        [Required]
        public double FuelCapacity { get; set; }

        [Required]
        public string EngineType { get; set; }

        [Required]
        public double Range { get; set; }

        public DateTime ManufacturingDate { get; set; }

        [Required]
        public string AdditionalInformation { get; set; }
    }
}
