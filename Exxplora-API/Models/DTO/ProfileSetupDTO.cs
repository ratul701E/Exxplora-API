using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exxplora_API.Models.DTO
{
    public class ProfileSetupDTO
    {
        public string Location { get; set; }

        public bool IsStudent { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        //[Required(ErrorMessage = "Institute is required.")]
        [MaxLength(100, ErrorMessage = "Institute name cannot exceed 100 characters.")]
        public string Institution { get; set; }

        public ICollection<int> Domains { get; set; }

    }
}