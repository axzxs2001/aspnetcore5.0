using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPIDemo.Model
{
    /// <summary>
    /// 用户分页实体
    /// </summary>
    public class UserPagination : PaginationBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType
        { get; set; }
  
    }


}
