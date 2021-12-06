using System.Security.Cryptography;
using System.Text;

namespace MovieScanToJson
{
    public static class CryptoFileHelper
    {
        private const string Key = "WatchingMoviesIsAwesome!1234s";

        private const string FilePath = "movies.json.encrypted";

        //TODO IMPLEMENT JSON EN-/DECRYPTION
    }

    class CryptoHelper
    {
        //Super secret 32 Byte / 256 bit Key
        private static readonly string KeyString = "RossmannRSR!1234";
        private static byte[] Key => Encoding.Unicode.GetBytes(KeyString);

        //aes settings
        private static readonly PaddingMode Pad = PaddingMode.PKCS7;
        private static readonly CipherMode Mode = CipherMode.CBC;


        /// <summary>
        /// Creates Aes with global settings. IV is not managed here.
        /// </summary>
        /// <returns>Aes with applied key, padding and mode</returns>
        private static Aes GetAes()
        {
            var aes = Aes.Create();
            aes.Key = Key;
            aes.Padding = Pad;
            aes.Mode = Mode;
            return aes;
        }

        /// <summary>
        /// Decrypts a stream to memory
        /// </summary>
        /// <param name="fs">The stream to be decrypted</param>
        /// <returns>MemoryStream of decrypted file. REMEMBER TO DISPOSE THIS STREAM!</returns>
        public static MemoryStream DecryptFile(Stream fs)
        {
            using (var aes = GetAes())
            {
                var iv = new byte[aes.IV.Length];

                //Reads IV value from beginning of the file.
                fs.Read(iv, 0, iv.Length);
                //apply iv to aes object
                aes.IV = iv;

                //Create a CryptoStream, pass it the file stream, and decrypt it with the Aes class and its key and IV.
                using (var cs = new CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    var ms = new MemoryStream();
                    cs.CopyTo(ms);
                    return ms;
                }

            }

        }

    }
}
