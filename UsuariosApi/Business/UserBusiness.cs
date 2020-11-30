using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UsuariosApi.Business.Interfaces;
using UsuariosApi.Data.Interfaces;
using UsuariosApi.Models;

namespace UsuariosApi.Business
{
    public class UserBusiness: IUserBusiness
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public UserBusiness(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public async Task<string> AddUser(User user)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            user.Salt = Convert.ToBase64String(salt);
            user.Password = user.GetPasswordHash();
            bool saved = await userRepository.Add(user);
            if (saved)
                return GenerateJSONWebToken(user);

            return "Erro: Falha ao cadastrar.";

        }

        public async Task<string> AuthUser(string username, string password)
        {
            var user = await userRepository.GetUserByUsername(username);

            if (user == null)
                return "Erro: Usuario ou senha não encontrado.";

            if (user.TestPassword(password))
            {
                return GenerateJSONWebToken(user);
            }
            
            return "Erro: Usuario ou senha não encontrado.";
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await userRepository.GetUserByUsername(username);
        }
        public async Task<User> GetUserByGuid(Guid guid)
        {
            return await userRepository.GetUserByGuid(guid);
        }

        public async Task<List<User>> GetUsers(int pageSize, int page)
        {
            return await userRepository.GetUsers(pageSize, page);
        }

        public async Task<bool> DeleteUser(Guid guid)
        {
            return await userRepository.Delete(guid);
        }


        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
              configuration["Jwt:Issuer"],
              claims: new List<Claim> { 
                  new Claim("UserName", userInfo.Username), 
                  new Claim("Email", userInfo.Email),
                  new Claim("AccessLevel", userInfo.AccessLevel.ToString())
              },
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> UpdateUser(User user)
        {
            return await userRepository.Update(user);
        }

        public async Task<bool> UpdatePassword(User user)
        {
            return await userRepository.Update(user);
        }

        public async Task<bool> ResetPasswordToken(Guid guid)
        {
            string token = RandomString(5);

            return await userRepository.AddUserPasswordToken(guid, token);
        }

        public async Task<bool> ChangePassword(ChangePassword model)
        {
            var user = await userRepository.GetUserByGuid(model.UserId);

            if( model.Token != user.PasswordChangeToken)
            {
                return false;
            }

            user.Password = model.NewPassword;

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            user.Salt = Convert.ToBase64String(salt);
            user.Password = user.GetPasswordHash();

            return await userRepository.UpdatePassword(user);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
