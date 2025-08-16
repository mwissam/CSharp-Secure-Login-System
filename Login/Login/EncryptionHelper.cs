using System;
using System.Security.Cryptography;
using System.Text;

namespace Login
{
    internal class EncryptionHelper
    {
        public static class PasswordHelper
        {
            // إنشاء Hash و Salt
            public static void CreatePasswordHash(string password, out string hash, out string salt)
            {
                using (var hmac = new HMACSHA256())
                {
                    salt = GenerateSalt();
                    hmac.Key = Encoding.UTF8.GetBytes(salt);
                    hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
                }
            }

            // التحقق من كلمة المرور
            public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
            {
                using (var hmac = new HMACSHA256())
                {
                    hmac.Key = Encoding.UTF8.GetBytes(storedSalt);
                    var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword)));
                    return computedHash == storedHash;
                }
            }

            // توليد Salt عشوائي
            private static string GenerateSalt()
            {
                byte[] saltBytes = new byte[16];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(saltBytes);
                }
                return Convert.ToBase64String(saltBytes);
            }
        }
    }
}
