using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;
using ZhonTai.Common.Helpers.SMEncrypt;


namespace ZhonTai.Common.Helpers;

/// <summary>
/// SM2加密类
/// </summary>
public class SM2Encrypt
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pubk"></param>
    /// <param name="prik"></param>
    public static void GenerateKeyPair(out ECPoint pubk, out BigInteger prik)
    {
        SM2 sm2 = SM2.Instance;
        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
        ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
        ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
        BigInteger privateKey = ecpriv.D;
        ECPoint publicKey = ecpub.Q;

        //System.Console.Out.WriteLine("公钥: " + Encoding.ASCII.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper());
        //System.Console.Out.WriteLine("私钥: " + Encoding.ASCII.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper());
        pubk = publicKey;
        prik = privateKey;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pubk"></param>
    /// <param name="prik"></param>
    public static void GenerateKeyPair(out string pubk, out string prik)
    {
        SM2 sm2 = SM2.Instance;
        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
        ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
        ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
        BigInteger privateKey = ecpriv.D;
        ECPoint publicKey = ecpub.Q;

        //System.Console.Out.WriteLine("公钥: " + Encoding.ASCII.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper());
        //System.Console.Out.WriteLine("私钥: " + Encoding.ASCII.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper());
        pubk = Encoding.ASCII.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper();
        prik = Encoding.ASCII.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="publicKey"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string Encrypt(byte[] publicKey, byte[] data)
    {
        if (null == publicKey || publicKey.Length == 0)
        {
            return null;
        }
        if (data == null || data.Length == 0)
        {
            return null;
        }

        byte[] source = new byte[data.Length];
        Array.Copy(data, 0, source, 0, data.Length);

        Cipher cipher = new Cipher();
        SM2 sm2 = SM2.Instance;

        ECPoint userKey = sm2.ecc_curve.DecodePoint(publicKey);

        ECPoint c1 = cipher.Init_enc(sm2, userKey);
        cipher.Encrypt(source);

        byte[] c3 = new byte[32];
        cipher.Dofinal(c3);

        string sc1 = Encoding.ASCII.GetString(Hex.Encode(c1.GetEncoded()));
        string sc2 = Encoding.ASCII.GetString(Hex.Encode(source));
        string sc3 = Encoding.ASCII.GetString(Hex.Encode(c3));

        return (sc1 + sc2 + sc3).ToUpper();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="privateKey"></param>
    /// <param name="encryptedData"></param>
    /// <returns></returns>
    public static byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
    {
        if (null == privateKey || privateKey.Length == 0)
        {
            return null;
        }
        if (encryptedData == null || encryptedData.Length == 0)
        {
            return null;
        }

        string data = Encoding.ASCII.GetString(Hex.Encode(encryptedData));

        byte[] c1Bytes = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(0, 130)));
        int c2Len = encryptedData.Length - 97;
        byte[] c2 = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(130, 2 * c2Len)));
        byte[] c3 = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(130 + 2 * c2Len, 64)));

        SM2 sm2 = SM2.Instance;
        BigInteger userD = new BigInteger(1, privateKey);

        //ECPoint c1 = sm2.ecc_curve.DecodePoint(c1Bytes);

        ECPoint c1 = sm2.ecc_curve.DecodePoint(c1Bytes);
        Cipher cipher = new Cipher();
        cipher.Init_dec(userD, c1);
        cipher.Decrypt(c2);
        cipher.Dofinal(c3);

        return c2;
    }

    //[STAThread]
    //public static void Main()
    //{
    //    GenerateKeyPair();

    //    String plainText = "ererfeiisgod";
    //    byte[] sourceData = Encoding.Default.GetBytes(plainText);

    //    //下面的秘钥可以使用generateKeyPair()生成的秘钥内容  
    //    // 国密规范正式私钥  
    //    String prik = "3690655E33D5EA3D9A4AE1A1ADD766FDEA045CDEAA43A9206FB8C430CEFE0D94";
    //    // 国密规范正式公钥  
    //    String pubk = "04F6E0C3345AE42B51E06BF50B98834988D54EBC7460FE135A48171BC0629EAE205EEDE253A530608178A98F1E19BB737302813BA39ED3FA3C51639D7A20C7391A";

    //    System.Console.Out.WriteLine("加密: ");
    //    String cipherText = SM2Utils.Encrypt(Hex.Decode(pubk), sourceData);
    //    System.Console.Out.WriteLine(cipherText);
    //    System.Console.Out.WriteLine("解密: ");
    //    plainText = Encoding.Default.GetString(SM2Utils.Decrypt(Hex.Decode(prik), Hex.Decode(cipherText)));
    //    System.Console.Out.WriteLine(plainText);

    //    Console.ReadLine();
    //}
}

