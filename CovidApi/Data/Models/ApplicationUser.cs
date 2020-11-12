using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CovidApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public string JobDescription { get; set; }

        [PersonalData]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [PersonalData]
        public string Github { get; set; }

        [PersonalData]
        public string LinkedIn { get; set; }
    }
}
