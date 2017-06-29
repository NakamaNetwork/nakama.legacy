using System.ComponentModel.DataAnnotations;

namespace TreasureGuide.Web.Models.AccountModels
{
    public class RegisterModel
    {
        [StringLength(256)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [StringLength(256)]
        public string UserName { get; set; }
    }
}
