using System.ComponentModel.DataAnnotations;

namespace TreasureGuide.Web.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        [StringLength(48, MinimumLength = 5)]
        [RegularExpression(@"^[A-Za-z0-9-\._@\+ ]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Terms of Service.")]
        public bool ToS { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Privacy Policy.")]
        public bool Privacy { get; set; }
    }
}
