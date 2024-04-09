using Org.BouncyCastle.Utilities.Encoders;
using System.Text;
using ZhonTai.Common.Helpers.SMEncrypt;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// SM4加密类
/// </summary>
public class SM4Encrypt
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="secretKey"></param>
    /// <param name="hexstring"></param>
    /// <returns></returns>
    public static string Encrypt_ECB(string plainText, string secretKey, bool hexstring = false)
    {
        SM4_Context ctx = new SM4_Context();
        ctx.isPadding = true;
        ctx.mode = SM4.SM4_ENCRYPT;

        byte[] keyBytes;
        if (hexstring)
        {
            keyBytes = Hex.Decode(secretKey);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
        }

        SM4 sm4 = new SM4();
        sm4.sm4_setkey_enc(ctx, keyBytes);
        byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Encoding.ASCII.GetBytes(plainText));

        string cipherText = Encoding.ASCII.GetString(Hex.Encode(encrypted));
        return cipherText;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="secretKey"></param>
    /// <param name="hexstring"></param>
    /// <returns></returns>
    public static string Decrypt_ECB(string cipherText, string secretKey, bool hexstring = false)
    {
        SM4_Context ctx = new SM4_Context();
        ctx.isPadding = true;
        ctx.mode = SM4.SM4_DECRYPT;

        byte[] keyBytes;
        if (hexstring)
        {
            keyBytes = Hex.Decode(secretKey);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
        }

        SM4 sm4 = new SM4();
        sm4.sm4_setkey_dec(ctx, keyBytes);
        byte[] decrypted = sm4.sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
        return Encoding.ASCII.GetString(decrypted);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="secretKey"></param>
    /// <param name="iv"></param>
    /// <param name="hexstring"></param>
    /// <returns></returns>
    public static string Encrypt_CBC(string plainText, string secretKey, string iv, bool hexstring = false)
    {
        SM4_Context ctx = new SM4_Context();
        ctx.isPadding = true;
        ctx.mode = SM4.SM4_ENCRYPT;

        byte[] keyBytes;
        byte[] ivBytes;
        if (hexstring)
        {
            keyBytes = Hex.Decode(secretKey);
            ivBytes = Hex.Decode(iv);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
            ivBytes = Encoding.ASCII.GetBytes(iv);
        }

        SM4 sm4 = new SM4();
        sm4.sm4_setkey_enc(ctx, keyBytes);
        byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Encoding.ASCII.GetBytes(plainText));

        string cipherText = Encoding.ASCII.GetString(Hex.Encode(encrypted));
        return cipherText;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="secretKey"></param>
    /// <param name="iv"></param>
    /// <param name="hexstring"></param>
    /// <returns></returns>
    public static string Decrypt_CBC(string cipherText, string secretKey, string iv, bool hexstring = false)
    {
        SM4_Context ctx = new SM4_Context();
        ctx.isPadding = true;
        ctx.mode = SM4.SM4_DECRYPT;

        byte[] keyBytes;
        byte[] ivBytes;
        if (hexstring)
        {
            keyBytes = Hex.Decode(secretKey);
            ivBytes = Hex.Decode(iv);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
            ivBytes = Encoding.ASCII.GetBytes(iv);
        }

        SM4 sm4 = new SM4();
        sm4.sm4_setkey_dec(ctx, keyBytes);
        byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
        return Encoding.ASCII.GetString(decrypted);
    }
}

