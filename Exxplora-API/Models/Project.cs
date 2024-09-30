using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exxplora_API.Models
{

    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public ICollection<User> Contributors { get; set; }

        public int ProjectStatusId { get; set; }

        public ProjectStatus ProjectStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ICollection<Domain> Domains { get; set; }

        public bool IsArchived { get; set; }

        public ICollection<Section> Sections { get; set; }

    }

}