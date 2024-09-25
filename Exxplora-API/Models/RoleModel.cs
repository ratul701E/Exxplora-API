using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exxplora_API.Models
{
    public enum Role
    {
        ADMIN = 1,
        MODERATOR,
        USER,
    }

    public class RoleModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Role Name is required.")]
        [MinLength(3, ErrorMessage = "Role name must be at least 3 characters long.")]
        public string Name { get; set; }
    }
}