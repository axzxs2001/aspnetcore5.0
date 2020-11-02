using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPIDemo.Model
{
    /// <summary>
    /// web api验证是加在Modle上的
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [MinLength(6, ErrorMessage = "长度不能小于6")]
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        [UserNameEqualPassword("用户名和密码长度相同了")]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
  
        public string Name { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
    }
}
