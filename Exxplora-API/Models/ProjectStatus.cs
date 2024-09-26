using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exxplora_API.Models
{
    public class ProjectStatus
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [MinLength(3, ErrorMessage = "Status must be at least 3 characters long.")]
        public string Status { get; set; }
    }
}