using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exxplora_API.Models.DTO
{
    public class UpdateProfileDTO
    {
        [Required(ErrorMessage = "FirstName is required.")]
        [MaxLength(32, ErrorMessage = "FirstName cannot exceed 32 characters.")]
        [MinLength(3, ErrorMessage = "FirstName must be at least 3 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        [MaxLength(32, ErrorMessage = "LastName cannot exceed 32 characters.")]
        [MinLength(3, ErrorMessage = "LastName must be at least 3 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public bool IsStudent { get; set; }

        public int StartYear { get; set; }

        //[Required(ErrorMessage = "Institute is required.")]
        //[MaxLength(100, ErrorMessage = "Institute name cannot exceed 100 characters.")]
        public string Institute { get; set; }
    }
}