using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsuariosApi.Business;
using UsuariosApi.Data.Interfaces;
using UsuariosApi.Models;
using Xunit;

namespace UsuariosApi.Test
{
    public class UserBusinessTest
    {
        [Fact]
        public void AddUserError()
        {
            var config = TestHelper.InitConfiguration();
            var userRepository = new Mock<IUserRepository>();
            var userBusiness = new UserBusiness(userRepository.Object, config);
            var testUser = new User
            {
                Username = "testUser",
                Password = "testePassword",
                Email = "teste@bol.com",
                AccessLevel = 10,
                Name = "emailTeste"
            };
            var userCreated = userBusiness.AddUser(testUser).Result;
            Assert.Equal("Erro: Falha ao cadastrar.", userCreated);
        }

        [Fact]
        public void AddUserOk()
        {
            var testUser = new User
            {
                Username = "testUser",
                Password = "testePassword",
                Email = "teste@bol.com",
                AccessLevel = 10,
                Name = "emailTeste"
            };

            var config = TestHelper.InitConfiguration();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(p => p.Add(testUser)).Returns( Task.FromResult(true) );
            var userBusiness = new UserBusiness(userRepository.Object, config);

            var userCreated = userBusiness.AddUser(testUser).Result;
            Assert.NotEqual("Erro: Falha ao cadastrar.", userCreated);
        }

        [Fact]
        public void AuthUserWrongPassword()
        {
            var testAttempt = new UserAuth
            {
                Username = "testUser",
                Password = "testePassword"
            };

            var returnUser = new User
            {
                Username = "testUser",
                Email = "gico223",
                AccessLevel = 10,
                Name = "emailTeste",
                Password = "Z3hhAJeUNinPzTAJSRby/5TKC/7/TQWzUu00PqUblw8=",
                Salt = "RUb4A4w5VyR1GuIkttKmyA=="
            };

            var config = TestHelper.InitConfiguration();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(p => p.GetUserByUsername(testAttempt.Username)).Returns(Task.FromResult(returnUser));

            var userBusiness = new UserBusiness(userRepository.Object, config);

            var token = userBusiness.AuthUser(testAttempt.Username, testAttempt.Password).Result;

            Assert.Equal("Erro: Usuario ou senha não encontrado.", token);
        }

        [Fact]
        public void AuthUserNoUsername()
        {
            var testAttempt = new UserAuth
            {
                Username = "testUser",
                Password = "testePassword"
            };

            User returnUser = null;

            var config = TestHelper.InitConfiguration();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(p => p.GetUserByUsername(testAttempt.Username)).Returns(Task.FromResult(returnUser));

            var userBusiness = new UserBusiness(userRepository.Object, config);

            var token = userBusiness.AuthUser(testAttempt.Username, testAttempt.Password).Result;

            Assert.Equal("Erro: Usuario ou senha não encontrado.", token);
        }

        [Fact]
        public void AuthUserOk()
        {
            var testAttempt = new UserAuth
            {
                Username = "testUser",
                Password = "gico223"
            };

            var returnUser = new User
            {
                Username = "testUser",
                Email = "gico223",
                AccessLevel = 10,
                Name = "emailTeste",
                Password = "Z3hhAJeUNinPzTAJSRby/5TKC/7/TQWzUu00PqUblw8=",
                Salt = "RUb4A4w5VyR1GuIkttKmyA=="
            };

            var config = TestHelper.InitConfiguration();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(p => p.GetUserByUsername(testAttempt.Username)).Returns(Task.FromResult(returnUser));

            var userBusiness = new UserBusiness(userRepository.Object, config);

            var token = userBusiness.AuthUser(testAttempt.Username, testAttempt.Password).Result;

            Assert.NotEqual("Erro: Usuario ou senha não encontrado.", token);
        }

        [Fact]
        public void GetUserByUsername()
        {
            string username = "testUser";

            var returnUser = new User
            {
                Username = "testUser",
                Email = "gico223",
                AccessLevel = 10,
                Name = "emailTeste",
                Password = "Z3hhAJeUNinPzTAJSRby/5TKC/7/TQWzUu00PqUblw8=",
                Salt = "RUb4A4w5VyR1GuIkttKmyA=="
            };

            var config = TestHelper.InitConfiguration();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(p => p.GetUserByUsername(username)).Returns(Task.FromResult(returnUser));

            var userBusiness = new UserBusiness(userRepository.Object, config);

            var user = userBusiness.GetUserByUsername(username).Result;

            Assert.NotNull(user);
        }

        [Fact]
        public void GetUserByUsernameNotFound()
        {
            string username = "testUsser";

            User returnUser = null;
            var config = TestHelper.InitConfiguration();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(p => p.GetUserByUsername(username)).Returns(Task.FromResult(returnUser));

            var userBusiness = new UserBusiness(userRepository.Object, config);

            var user = userBusiness.GetUserByUsername(username).Result;

            Assert.Null(user);
        }

        [Fact]
        public void GetUsers()
        {
            string username = "testUser";

            var returnUsers = new List<User>
            {
                new User
                {
                    Username = "testUser2",
                    Email = "gico223",
                    AccessLevel = 10,
                    Name = "emailTeste22",
                    Password = "Z3hhAJeUNinPzTAJSRby/5TKC/7/TQWzUu00PqUblw8=",
                    Salt = "RUb4A4w5VyR1GuIkttKmyA=="
                },
                new User
                {
                    Username = "testUser",
                    Email = "gico223",
                    AccessLevel = 10,
                    Name = "emailTeste",
                    Password = "Z3hhAJeUNinPzTAJSRby/5TKC/7/TQWzUu00PqUblw8=",
                    Salt = "RUb4A4w5VyR1GuIkttKmyA=="
                }
            };

            var config = TestHelper.InitConfiguration();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(p => p.GetUsers(10, 0)).Returns(Task.FromResult(returnUsers));

            var userBusiness = new UserBusiness(userRepository.Object, config);

            var user = userBusiness.GetUsers(10,0).Result;

            Assert.True(user.Count > 0);
        }

        [Fact]
        public void GetUsersNone()
        {
            string username = "testUser";

            var returnUsers = new List<User>
            {

            };

            var config = TestHelper.InitConfiguration();
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(p => p.GetUsers(10, 0)).Returns(Task.FromResult(returnUsers));

            var userBusiness = new UserBusiness(userRepository.Object, config);

            var user = userBusiness.GetUsers(10, 0).Result;

            Assert.True(user.Count == 0);
        }

    }
}
