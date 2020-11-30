using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Models;

namespace UsuariosApi.Business.Interfaces
{
    public interface IUserBusiness
    {
        Task<string> AddUser(User user);
        Task<string> AuthUser(string username, string password);
        Task<User> GetUserByUsername(string username);
        Task<List<User>> GetUsers(int pageSize, int page);
    }
}
