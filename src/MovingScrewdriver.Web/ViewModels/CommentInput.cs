using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace MovingScrewdriver.Web.ViewModels
{
    public class CommentInput
    {
        [Required(ErrorMessage = "Imię lub nick jest wymagane.")]
        public string CommenterName { get; set; }

        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [Email(ErrorMessage = "Nie poprawny adres e-mail.")]
        public string CommenterEmail { get; set; }

        [DataAnnotationsExtensions.Url(UrlOptions.OptionalProtocol, ErrorMessage = "Nie poprawny URL.")]
        public string CommenterWebsite { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Treść komentarza jest wymagana.")]
        [DataType(DataType.MultilineText)]
        public string CommenterComment { get; set; }

        public bool Subscribe { get; set; }

    }
}