using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exxplora_API.Models
{
    public class Domain
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}