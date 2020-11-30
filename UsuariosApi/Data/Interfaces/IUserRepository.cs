using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Models;

namespace UsuariosApi.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByGuid(Guid guid);
        Task<List<User>> GetUsers(int pagesize, int page);
        Task<bool> Add(User user);
        Task<bool> Delete(Guid guid);
        Task<bool> Update(User user);
        Task<bool> AddUserPasswordToken(Guid guid, string token);
        Task<bool> UpdatePassword(User user);
    }
}
