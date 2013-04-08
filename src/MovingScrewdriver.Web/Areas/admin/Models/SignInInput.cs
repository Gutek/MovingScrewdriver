using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace MovingScrewdriver.Web.Areas.admin.Models
{
    public class SignInInput
    {
        [Required]
        [Email]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}