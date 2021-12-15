using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;


namespace Login_Reg.Models
{
    public class LoginUser
    {
        [Display(Name = "Email: ")]
        [Required (ErrorMessage = "Please enter a valid email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        // [DataType(DataType.EmailAddress)]
        public string LogEmail { get; set; }

        [Display(Name = "Password: ")]
        [Required]
        [MinLength(8, ErrorMessage = "Please enter a password of at least 8 characters")]
        [DataType(DataType.Password)]
        public string LogPassword { get; set; }

        [NotMapped]
        public int LogUserId { get; set; }
    }
}