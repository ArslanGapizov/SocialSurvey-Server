using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace SocialSurvey.Server.Auth
{
    public class HashConverter
    {
        private const string SALT = "mysaltsupersave";
        public static string GetHash(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(SALT);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                   password: password,
                                                   salt: salt,
                                                   prf: KeyDerivationPrf.HMACSHA1,
                                                   iterationCount: 10000,
                                                   numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
