using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsuariosApi.Models
{
    public class User
    {
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; }
        [JsonIgnore]
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int AccessLevel { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }


    }

    public static class UserExtras
    {
        public static string GetPasswordHash(this User user)
        {
            var salt = Convert.FromBase64String(user.Salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: user.Password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));


            return hashed;
        }

        public static bool TestPassword(this User user, string enteredPassword)
        {
            var salt = Convert.FromBase64String(user.Salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: user.Password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

            string enteredHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: user.Password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));


            return hashed == enteredHashed;
        }
    }


}
