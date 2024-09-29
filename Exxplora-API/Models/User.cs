using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exxplora_API.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

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

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string Phone { get; set; }

        //[Required(ErrorMessage = "IsStudent field is required.")]
        public bool IsStudent { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public string Location { get; set; }

        //[Required(ErrorMessage = "Institute is required.")]
        [MaxLength(100, ErrorMessage = "Institute name cannot exceed 100 characters.")]
        public string Institute { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Role id must be grater than 0")]
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public ICollection<Project> Projects { get; set; }

        public ICollection<Project> AuthoredProjects { get; set; }

        public ICollection<Domain> Domains { get; set; }

        [MinLength(3, ErrorMessage = "Profile Picture Path must be at least 3 characters long.")]
        public string ProfilePicturePath { get; set; }
        
        [MinLength(3, ErrorMessage = "Cover Photo Path Picture Path must be at least 3 characters long.")]
        public string CoverPhotoPath { get; set; }

    }
}
