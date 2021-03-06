﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Anthill.API.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "the field is too long")]
        public string Password { get; set; }
    }
}
