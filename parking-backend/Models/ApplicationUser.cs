using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace parking_backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(150)")]
        [Required]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        [Required]
        public string LastName { get; set; }
    }
}
