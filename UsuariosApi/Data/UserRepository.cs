using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Data.Interfaces;
using UsuariosApi.Models;

namespace UsuariosApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await appDbContext.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
        }


        public async Task<List<User>> GetUsers(int pagesize, int page)
        {
            int skip = 0;
            if (page > 0)
                 skip = (page - 1) * pagesize;

            return await appDbContext.Users.Skip(skip).Take(pagesize).ToListAsync();
        }

        public async Task<bool> Add(User user)
        {
            appDbContext.Add(user);
            return await appDbContext.SaveChangesAsync() > 0;
        }

    }
}
