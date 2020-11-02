using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RestfulAPIDemo.Model
{
    /// <summary>
    /// 帐户
    /// </summary>
    public class Account
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 帐户类型
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// 帐户号
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
    }
}
