using Org.BouncyCastle.Utilities.Encoders;
using System.Text;
using ZhonTai.Common.Helpers.SMEncrypt;

namespace ZhonTai.Common.Helpers;

/// <summary>
/// SM3加密类
/// </summary>
public class SM3Encrypt
{
    /// <summary>
    /// SM3加密类
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="Case">false小写，true大写</param>
    /// <returns></returns>
    public static string Encrypt(string plainText, bool Case = false)
    {
        byte[] md = new byte[32];
        byte[] msg1 = Encoding.Default.GetBytes(plainText);
        SM3Digest sm3 = new SM3Digest();
        sm3.BlockUpdate(msg1, 0, msg1.Length);
        sm3.DoFinal(md, 0);
        string s = new UTF8Encoding().GetString(Hex.Encode(md));
        if (Case == true)
        {
            s = s.ToUpper();
        }
        return s;
    }

}

