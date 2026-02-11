using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace SupplyChain.Utils
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] salt = GenerateSalt();
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] combined = Combined(passwordBytes, salt);
                byte[] hash = sha.ComputeHash(combined);

                return Convert.ToBase64String(hash) + ":" + Convert.ToBase64String(salt);
            }
        }
        public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;
            byte[] hash = Convert.FromBase64String(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);

            using (var sha = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] combined = Combined(passwordBytes, salt);
                byte[] computedHash = sha.ComputeHash(combined);

                return SlowEquals(hash, computedHash);
            }
        }
        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        private static byte[] Combined(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}