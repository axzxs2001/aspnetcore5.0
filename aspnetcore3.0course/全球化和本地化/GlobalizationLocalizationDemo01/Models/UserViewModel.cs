using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalizationLocalizationDemo01.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "UserNameRequired")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(8, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
