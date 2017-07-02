using System.ComponentModel.DataAnnotations;

namespace TreasureGuide.Web.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        [StringLength(256, MinimumLength = 8)]
        [RegularExpression(@"^[A-Za-z0-9-\._@\+ ]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }
    }
}
