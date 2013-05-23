using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Security.Crypto
{
    /// <summary>
    /// http://stackoverflow.com/questions/165808/simple-2-way-encryption-for-c/5518092#5518092
    /// </summary>
    public class EncryptionUtils
    {
        private static readonly byte[] Key = { 123, 217, 19, 11, 24, 37, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };

        private static readonly byte[] Vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 };
        private static readonly byte[] Entropy = { 120, 54, 65, 235, 11, 3, 31, 251 };
        private static readonly RijndaelManaged _encryptionProvider = new RijndaelManaged();
        private static readonly UTF8Encoding Encoder = new UTF8Encoding();

        /// <summary>
        /// Encrypts the specified unencrypted string by symmetric algorithm.
        /// </summary>
        /// <param name="unencrypted">The unencrypted string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Unencrypted string is null or empty</exception>
        public static string Encrypt(string unencrypted)
        {
            if (unencrypted == null)
                throw new ArgumentNullException("unencrypted");
            return Convert.ToBase64String(InternalEncrypt(unencrypted));
        }

        /// <summary>
        /// Decrypts the specified encrypted string by symmetric algorithm
        /// </summary>
        /// <param name="encrypted">The encrypted string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Encrypted string is null or empty</exception>
        public static string Decrypt(string encrypted)
        {
            if (encrypted == null)
                throw new ArgumentNullException("encrypted");

            return InternalDecrypt(Convert.FromBase64String(encrypted), Key);
        }

        /// <summary>
        /// Encrypt string by using DPAPI. 
        /// This encryption should be used only in machine scope. Encryption and Decryption must be on the same machine.
        /// </summary>
        /// <param name="unencrypted">The unencrypted string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Unencrypted string is null or empty</exception>
        public static string ProtectDataInMachine(string unencrypted)
        {
            if (unencrypted == null)
                throw new ArgumentNullException("unencrypted");

            return Convert.ToBase64String(ProtectedData.Protect(Encoder.GetBytes(unencrypted), Entropy, DataProtectionScope.LocalMachine));
        }

        /// <summary>
        /// Decrypt string by using DPAPI. 
        /// This encryption should be used only in machine scope. Encryption and Decryption must be on the same machine.
        /// </summary>
        /// <param name="encrypted">The encrypted string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Encrypted string is null or empty</exception>
        public static string UnprotectDataInMachine(string encrypted)
        {
            if (encrypted == null)
                throw new ArgumentNullException("encrypted");
            return Encoder.GetString(ProtectedData.Unprotect(Convert.FromBase64String(encrypted), Entropy, DataProtectionScope.LocalMachine));
        }

        private static byte[] InternalEncrypt(string buffer)
        {
            byte[] encrypted;

            using (var encryptStream = new MemoryStream())
            {
                using (var cs = new CryptoStream(encryptStream, _encryptionProvider.CreateEncryptor(Key, Vector), CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(cs))
                    {
                        writer.Write(buffer);
                    }
                }
                encrypted = encryptStream.ToArray();
            }

            return encrypted;
        }

        private static string InternalDecrypt(byte[] buffer, byte[] secretKey)
        {
            string plainData;

            using (var decryptStream = new MemoryStream(buffer))
            {
                using (var cs = new CryptoStream(decryptStream, _encryptionProvider.CreateDecryptor(secretKey, Vector), CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        plainData = reader.ReadToEnd();
                    }
                }
            }

            return plainData;
        }
    }
}
