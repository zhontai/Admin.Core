using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// AES加密解密帮助类
/// 更多参考 https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes
/// </summary>
public class AESEncrypt
{
    /// <summary>
    /// 加密ECB
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <param name="Key">密匙</param>
    /// <returns>密文</returns>
    public static string EncryptStringToString(string plainText, string Key)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(Key);
            aesAlg.Mode = CipherMode.ECB;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        // Return the encrypted bytes from the memory stream.
        return Convert.ToBase64String(encrypted);
    }
    /// <summary>
    /// 解密ECB
    /// </summary>
    /// <param name="cipherText">密文</param>
    /// <param name="Key">密匙</param>
    /// <returns>明文</returns>
    public static string DecryptStringFromString(string cipherText, string Key)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(Key);
            aesAlg.Mode = CipherMode.ECB;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }

}

