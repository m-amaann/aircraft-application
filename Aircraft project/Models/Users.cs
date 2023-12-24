﻿using System;
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
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public required string email { get; set; }

        [Required]
        [Phone]
        public required string mobilenumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string password { get; set; }
    }
}
