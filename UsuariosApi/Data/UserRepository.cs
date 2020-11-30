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

        public async Task<User> GetUserByGuid(Guid guid)
        {
            return await appDbContext.Users.Where(u => u.UserId == guid).FirstOrDefaultAsync();
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

        public async Task<bool> Delete(Guid guid)
        {
            var user = await appDbContext.Users.Where(u => u.UserId == guid).FirstOrDefaultAsync();
            appDbContext.Users.Remove(user);
            return await appDbContext.SaveChangesAsync() > 0;

        }

        public async Task<bool> Update(User user)
        {
            var userDb = await appDbContext.Users.Where(u => u.UserId == user.UserId).FirstOrDefaultAsync();

            userDb.Name = user.Name;
            userDb.Username = user.Username;
            userDb.AccessLevel = user.AccessLevel;
            userDb.Email = user.Email;


            appDbContext.Users.Update(userDb);
            return await appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePassword(User user)
        {
            var userDb = await appDbContext.Users.Where(u => u.UserId == user.UserId).FirstOrDefaultAsync();

            userDb.Password = user.Password;
            userDb.Salt = user.Salt;


            appDbContext.Users.Update(userDb);
            return await appDbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddUserPasswordToken(Guid guid, string token)
        {
            var userDb = await appDbContext.Users.Where(u => u.UserId == guid).FirstOrDefaultAsync();

            userDb.PasswordChangeToken = token;


            appDbContext.Users.Update(userDb);
            return await appDbContext.SaveChangesAsync() > 0;
        }

    }
}
