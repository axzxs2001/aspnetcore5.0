using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginProject.Models
{
    public class UserModel
    {
        public string UserName { get; set; }

        public string RoleName { get; set; }

        public override string ToString()
        {
            return $"UserName={UserName},RoleName={RoleName}";
        }
    }
}
