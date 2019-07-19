using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPIDemo.Model
{
    /// <summary>
    /// 
    /// </summary>
    public enum PaginationResourceUriType
    {
        /// <summary>
        /// 上一页
        /// </summary>
        PreviousPage,
        /// <summary>
        ///下一页
        /// </summary>
        NextPage,
        /// <summary>
        /// 当前页
        /// </summary>
        CurrentPage
    }
}
