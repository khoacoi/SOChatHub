using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Security.Crypto
{
    /// <summary>
    /// An utility for computing hash string of a raw string.
    /// </summary>
    public static class HashUtils
    {
        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="salt">Recommended the salt size is 128 byte length</param>
        /// <returns></returns>
        public static string ComputeHash(string value, byte[] salt)
        {
            var data = System.Text.Encoding.UTF8.GetBytes(value);
            var hmacSHA512 = new HMACSHA512(salt);
            return Convert.ToBase64String(hmacSHA512.ComputeHash(data));
        }
    }
}
