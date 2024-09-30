using System;
using System.ComponentModel.DataAnnotations;

namespace Exxplora_API.Models
{
    public class Version
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "SectionId is required.")]
        public int SectionId { get; set; }

        public Exxplora_API.Models.Section Section { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(5000, ErrorMessage = "Content cannot be longer than 5000 characters.")]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public User User { get; set; }
    }
}
