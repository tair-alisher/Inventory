using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models.Account
{
    public class ChangeEmailModel
    {
        [Required]
        [Display(Name = "Почта")]
        public string Email { get; set; }
    }
}