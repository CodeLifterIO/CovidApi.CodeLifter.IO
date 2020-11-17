using System.ComponentModel.DataAnnotations;

namespace CovidApi.ViewModels
{
    public class ProfileViewModel
    {
        [StringLength(30, ErrorMessage = "The {0} must be at max {1} characters long.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(30, ErrorMessage = "The {0} must be at max {1} characters long.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [StringLength(30, ErrorMessage = "The {0} must be at max {1} characters long.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        public string Username { get; set; }
    }
}
