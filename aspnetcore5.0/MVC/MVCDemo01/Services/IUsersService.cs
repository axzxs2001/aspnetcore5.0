using MVCDemo01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDemo01.Services
{
    public interface IUsersService
    {
        bool CreateUser(UserViewModel user);
        bool EditUser(UserViewModel user);
        bool DeleteUser(UserViewModel user);
        List<UserViewModel> GetUsers();
        UserViewModel GetUser(string userName);
    }
}
