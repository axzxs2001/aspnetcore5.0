using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDemo01.Models
{
    public class UserViewModel
    {
        [Key]
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(8, ErrorMessage = "密码长度在6-8位长度", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "确认密码不能为空")]
        [StringLength(8, ErrorMessage = "密码长度在6-8位长度", MinimumLength = 6)]
        [DataType(DataType.Password)]
        
        [Compare("Password",ErrorMessage = "确认密码与密码不一致")]
        [Display(Name = "确认密码")]
        public string ConfirmPassword { get; set; }

    }
}
