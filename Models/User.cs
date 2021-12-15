using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Login_Reg
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "First Name: ")]
        [Required]
        [MinLength(2, ErrorMessage = "Please enter a name of at least 2 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name: ")]
        [Required]
        [MinLength(2, ErrorMessage = "Please enter a name of at least 2 characters")]
        public string LastName { get; set; }

        [Display(Name = "Email: ")]
        [Required (ErrorMessage = "Please enter a valid email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        // [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password: ")]
        [Required]
        [MinLength(8, ErrorMessage = "Please enter a password of at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [Display(Name = "Re-enter Password: ")]
        [Required]
        [Compare("Password", ErrorMessage = "Your passwords must match")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
    }
}