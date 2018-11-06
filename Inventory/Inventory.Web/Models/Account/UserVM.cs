using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models.Account
{
    public class UserVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}