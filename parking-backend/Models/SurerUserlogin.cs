using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace parking_backend.Models
{
    public partial class SurerUserlogin
    {
        
        [Required(ErrorMessage = "EmailRequiredError")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PasswordRequiredError")]
        public string Password { get; set; }
        [Required(ErrorMessage = "FirstNameRequiredError")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastNameRequiredError")]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
