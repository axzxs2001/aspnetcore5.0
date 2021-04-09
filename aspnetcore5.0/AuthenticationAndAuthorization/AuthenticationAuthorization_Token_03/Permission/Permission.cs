using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationAuthorization_Token_03
{
    /// <summary>
    /// 用户或角色或其他凭据实体
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Credentials
        { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url
        { get; set; }
        /// <summary>
        /// 请求谓词
        /// </summary>
        public virtual string Method
        { get; set; }
    }
}
