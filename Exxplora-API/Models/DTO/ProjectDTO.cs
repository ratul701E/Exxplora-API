using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exxplora_API.Models.DTO
{
    public class ProjectDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public bool IsArchived { get; set; }

        public ICollection<int> Domains { get; set; }
    }
}
