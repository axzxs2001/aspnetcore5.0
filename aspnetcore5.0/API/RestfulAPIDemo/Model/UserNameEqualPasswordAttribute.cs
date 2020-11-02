using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPIDemo.Model
{
    /// <summary>
    /// 用户名和密码相等特性  验证特性
    /// </summary>
    public class UserNameEqualPasswordAttribute : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="errorMessage"></param>
        public UserNameEqualPasswordAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        /// <summary>
        /// 添加验证
        /// </summary>
        /// <param name="context"></param>
        public void AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }
     
        /// <summary>
        /// 验证结果
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="validationContext">验证上下文</param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (User)validationContext.ObjectInstance;

            if (user.UserName.Length == user.Password.Length)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
