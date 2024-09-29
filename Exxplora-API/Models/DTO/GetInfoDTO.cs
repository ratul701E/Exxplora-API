using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exxplora_API.Models.DTO
{
    public class GetInfoDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OrganizationName { get; set; }

        public string Location { get; set; }

        public string ProfilePicUrl { get; set; }

        public string CoverPicUrl { get; set; }
    }
}