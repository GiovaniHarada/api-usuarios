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

        public async Task<List<User>> GetUsers(int pageSize, int page)
        {
            return await userRepository.GetUsers(pageSize, page);
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
    }
}
