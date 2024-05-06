using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>  
/// 加密工具类，提供基于SM4算法的加密和解密功能。  
/// </summary>  
public static class SM4Encryption
{

    /// <summary>  
    /// 使用SM4算法对指定明文进行加密。  
    /// </summary>  
    /// <param name="msg">待加密的明文。</param>  
    /// <param name="key">用于加密的密钥。</param>  
    /// <param name="iv">初始化向量，对于ECB模式可以传递null。</param>  
    /// <param name="mode">加密模式，默认为"ECB"。</param>  
    /// <param name="isHex">是否返回十六进制格式的密文，默认为false（返回Base64格式）。</param>  
    /// <returns>加密后的密文，格式为十六进制或Base64。</returns>  
    /// <exception cref="ArgumentNullException">当msg或key为空时抛出。</exception>  
    /// <exception cref="ArgumentException">当mode不被支持时抛出。</exception>  
    /// <exception cref="CryptographicException">当加密过程中发生错误时抛出。</exception>  
    public static string Encrypt(string msg, string key, string iv, string mode = "ECB", bool isHex = false)
    {
        // 加密操作  
        // 验证输入参数  
        if (string.IsNullOrEmpty(msg))
            throw new ArgumentNullException(nameof(msg), "Message cannot be null or empty.");
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        if (iv == null && mode != "ECB")
            throw new ArgumentNullException(nameof(iv), "IV is required for modes other than ECB.");
        byte[] plainTextData = Encoding.UTF8.GetBytes(msg);
        var cipher = new SM4Engine();
        byte[] nonce = new byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(iv), 0, nonce, 0, Math.Min(iv.Length, nonce.Length));
        PaddedBufferedBlockCipher cipherMode;
        switch (mode)
        {
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
                break;
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
                break;
            // ... [其他模式的情况] ...  
            default:
                throw new ArgumentException("Unsupported mode: " + mode);
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(key); // 假设的密钥，实际中应该使用安全的方式生成和存储  
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
        ICipherParameters keyParamIV = new ParametersWithIV(keyParam, nonce);

        cipherMode.Init(true, mode == "ECB" ? keyParam : keyParamIV);

        byte[] cipherTextData = new byte[cipherMode.GetOutputSize(plainTextData.Length)];
        int length1 = cipherMode.ProcessBytes(plainTextData, 0, plainTextData.Length, cipherTextData, 0);
        cipherMode.DoFinal(cipherTextData, length1);
        return isHex == true ? Hex.ToHexString(cipherTextData) : Convert.ToBase64String(cipherTextData);
    }
    /// <summary>  
    /// 使用SM4算法对指定密文进行解密。  
    /// </summary>  
    /// <param name="encryptMsg">待解密的密文，可以是十六进制或Base64格式。</param>  
    /// <param name="key">用于解密的密钥。</param>  
    /// <param name="iv">初始化向量，对于ECB模式可以传递null。</param>  
    /// <param name="mode">加密模式，用于确定解密时使用的模式，默认为"ECB"。</param>  
    /// <param name="isHex">是否输入密文是十六进制格式，默认为false（表示Base64格式）。</param>  
    /// <returns>解密后的明文。</returns>  
    /// <exception cref="ArgumentNullException">当key或encryptMsg为空时抛出。</exception>  
    /// <exception cref="ArgumentException">当mode不被支持或iv对于非ECB模式为空时抛出。</exception>  
    /// <exception cref="CryptographicException">当解密过程中发生错误时抛出。</exception>  
    public static string Decrypt(string encryptMsg, string key, string iv, string mode = "ECB", bool isHex = false)
    {
        // 加密操作  
        // 验证输入参数  
        if (string.IsNullOrEmpty(encryptMsg))
            throw new ArgumentNullException(nameof(encryptMsg), "Message cannot be null or empty.");
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        if (iv == null && mode != "ECB")
            throw new ArgumentNullException(nameof(iv), "IV is required for modes other than ECB.");
        byte[] cipherTextData;
        if (isHex)
        {
            cipherTextData = Hex.Decode(encryptMsg);
        }
        else
        {
            cipherTextData = Convert.FromBase64String(encryptMsg);
        }
        var cipher = new SM4Engine();
        byte[] nonce = new byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(iv), 0, nonce, 0, Math.Min(iv.Length, nonce.Length));
        PaddedBufferedBlockCipher cipherMode;
        switch (mode)
        {
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
                break;
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
                break;
            // ... [其他模式的情况] ...  
            default:
                throw new ArgumentException("Unsupported mode: " + mode);
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(key); // 假设的密钥，实际中应该使用安全的方式生成和存储  
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("SM4", keyBytes);
        ICipherParameters keyParamIV = new ParametersWithIV(keyParam, nonce);
        // 解密操作  

        cipherMode.Init(false, mode == "ECB" ? keyParam : keyParamIV);

        byte[] decryptedData = new byte[cipherMode.GetOutputSize(cipherTextData.Length)];
        int length2 = cipherMode.ProcessBytes(cipherTextData, 0, cipherTextData.Length, decryptedData, 0);
        cipherMode.DoFinal(decryptedData, length2);

        // 打印解密后的明文  
        string decryptedMsg = Encoding.UTF8.GetString(decryptedData);
        return decryptedMsg;
    }

    /// <summary>  
    /// 使用SM4算法对指定明文进行加密。  
    /// </summary>  
    /// <param name="msg">待加密的明文。</param>  
    /// <param name="key">用于加密的密钥（字节数组形式）。</param>  
    /// <param name="iv">初始化向量（字节数组形式），对于ECB模式可以传递null。</param>  
    /// <param name="mode">加密模式，默认为"ECB"。</param>  
    /// <param name="isHex">是否返回十六进制格式的密文，默认为false（返回Base64格式）。</param>  
    /// <returns>加密后的密文，格式为十六进制或Base64。</returns>  
    /// <exception cref="ArgumentNullException">当msg或key为空时抛出。</exception>  
    /// <exception cref="ArgumentException">当mode不被支持时抛出。</exception>  
    /// <exception cref="CryptographicException">当加密过程中发生错误时抛出。</exception>  
    public static string Encrypt(string msg, byte[] key, byte[] iv, string mode = "ECB", bool isHex = false)
    {
        // 验证输入参数  
        if (string.IsNullOrEmpty(msg))
            throw new ArgumentNullException(nameof(msg), "Message cannot be null or empty.");
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        if (iv == null && mode != "ECB")
            throw new ArgumentNullException(nameof(iv), "IV is required for modes other than ECB.");

        // 将明文转换为字节数组  
        byte[] plainTextData = Encoding.UTF8.GetBytes(msg);

        var cipher = new SM4Engine();
        PaddedBufferedBlockCipher cipherMode;

        switch (mode)
        {
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
                break;
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
                break;
            // ... [其他模式的情况] ...  
            default:
                throw new ArgumentException("Unsupported mode: " + mode);
        }

        // 创建密钥参数  
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("SM4", key);
        ICipherParameters parameters = mode == "ECB" ? keyParam : new ParametersWithIV(keyParam, iv);

        // 初始化加密器  
        cipherMode.Init(true, parameters);

        // 准备输出缓冲区  
        byte[] cipherTextData = new byte[cipherMode.GetOutputSize(plainTextData.Length)];

        try
        {
            // 执行加密操作  
            int length1 = cipherMode.ProcessBytes(plainTextData, 0, plainTextData.Length, cipherTextData, 0);
            cipherMode.DoFinal(cipherTextData, length1);

            // 根据需求返回加密后的数据格式（十六进制或Base64）  
            return isHex ? Hex.ToHexString(cipherTextData) : Convert.ToBase64String(cipherTextData);
        }
        catch (Exception ex)
        {
            // 处理加密过程中发生的异常  
            throw new CryptographicException("Encryption failed.", ex);
        }
    }
    /// <summary>  
    /// 使用SM4算法对指定密文进行解密。  
    /// </summary>  
    /// <param name="encryptMsg">待解密的密文，可以是十六进制或Base64格式。</param>  
    /// <param name="key">用于解密的密钥（字节数组形式）。</param>  
    /// <param name="iv">初始化向量（字节数组形式），对于ECB模式可以传递null。</param>  
    /// <param name="mode">加密模式，用于确定解密时使用的模式，默认为"ECB"。</param>  
    /// <param name="isHex">是否输入密文是十六进制格式，默认为false（表示Base64格式）。</param>  
    /// <returns>解密后的明文。</returns>  
    /// <exception cref="ArgumentNullException">当key或encryptMsg为空时抛出。</exception>  
    /// <exception cref="ArgumentException">当mode不被支持或iv对于非ECB模式为空时抛出。</exception>  
    /// <exception cref="CryptographicException">当解密过程中发生错误时抛出。</exception>  
    public static string Decrypt(string encryptMsg, byte[] key, byte[] iv, string mode = "ECB", bool isHex = false)
    {
        // 验证输入参数  
        if (string.IsNullOrEmpty(encryptMsg))
            throw new ArgumentNullException(nameof(encryptMsg), "Message cannot be null or empty.");
        if (key == null || key.Length == 0)
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        if (iv == null && mode != "ECB")
            throw new ArgumentNullException(nameof(iv), "IV is required for modes other than ECB.");

        // 转换密文为字节数组  
        byte[] cipherTextData;
        if (isHex)
        {
            cipherTextData = Hex.Decode(encryptMsg);
        }
        else
        {
            cipherTextData = Convert.FromBase64String(encryptMsg);
        }

        var cipher = new SM4Engine();
        PaddedBufferedBlockCipher cipherMode;

        switch (mode)
        {
            case "ECB":
                cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
                break;
            case "CBC":
                cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
                break;
            // ... [其他模式的情况] ...  
            default:
                throw new ArgumentException("Unsupported mode: " + mode);
        }

        // 创建密钥参数  
        KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("SM4", key);
        ICipherParameters parameters = mode == "ECB" ? keyParam : new ParametersWithIV(keyParam, iv);

        // 初始化解密器  
        cipherMode.Init(false, parameters);

        // 准备输出缓冲区  
        byte[] decryptedData = new byte[cipherMode.GetOutputSize(cipherTextData.Length)];

        try
        {
            // 执行解密操作  
            int length1 = cipherMode.ProcessBytes(cipherTextData, 0, cipherTextData.Length, decryptedData, 0);
            cipherMode.DoFinal(decryptedData, length1);

            // 转换解密后的字节数组为字符串  
            string decryptedMsg = Encoding.UTF8.GetString(decryptedData);
            return decryptedMsg;
        }
        catch (Exception ex)
        {
            // 处理解密过程中发生的异常  
            throw new CryptographicException("Decryption failed.", ex);
        }
    }
}
