using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Aircraft_project.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        [Required]
        public string Address { get; set; }


        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
