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

        [Required(ErrorMessage = "Author ID is required.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Project status is required.")]
        public int ProjectStatusId { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid start date format.")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Invalid end date format.")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Budget is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive number.")]
        public decimal Budget { get; set; }

        [Required(ErrorMessage = "At least one domain is required.")]
        public ICollection<int> Domains { get; set; }
    }
}
