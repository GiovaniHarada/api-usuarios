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
        Task<bool> DeleteUser(Guid guid);
        Task<User> GetUserByGuid(Guid guid);
        Task<bool> UpdateUser(User user);
        Task<bool> ResetPasswordToken(Guid guid);
        Task<bool> ChangePassword(ChangePassword model);
    }
}
