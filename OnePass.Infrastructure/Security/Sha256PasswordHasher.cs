using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePass.Domain;
using System.Security.Cryptography;

namespace OnePass.Infrastructure
{
    public class Sha256PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();

            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            string computedHash = HashPassword(password);
            return string.Equals(computedHash, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
