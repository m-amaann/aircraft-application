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
        public int UserId { get; set; }

        public required string Name { get; set; }

        public required string email { get; set; }

        public required string mobilenumber { get; set; }

        [DataType(DataType.Password)]
        public required string password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
