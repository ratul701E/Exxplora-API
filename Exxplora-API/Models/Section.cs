using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exxplora_API.Models
{
    public class Section
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ProjectId is required.")]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public User User { get; set; }

        public ICollection<Exxplora_API.Models.Version> versions { get; set; }
    }
}
