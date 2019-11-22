using System;
using System.Security.Cryptography;
using System.Text;

namespace ADTool.Encryption
{
    //https://github.com/Pakhee/Cross-platform-AES-encryption

    /*****************************************************************
		 * CrossPlatform CryptLib
		 *
		 * <p>
		 * This cross platform CryptLib uses AES 256 for encryption. This library can
		 * be used for encryption and decryption of string on iOS, Android and Windows
		 * platform.<br/>
		 * Features: <br/>
		 * 1. 256 bit AES encryption
		 * 2. Random IV generation.
		 * 3. Provision for SHA256 hashing of key.
		 * </p>
		 *
		 * @since 1.0
		 * @author navneet
		 *****************************************************************/

    public class CryptLib
    {
        private UTF8Encoding enc;
        private RijndaelManaged rcipher;
        private byte[] key, pwd, ivBytes, iv;
        private readonly string defaultKey;
        private readonly string defaultInitVector;
        /***
		 * Encryption mode enumeration
		 */

        private enum EncryptMode { ENCRYPT, DECRYPT };

        private readonly char[] CharacterMatrixForRandomIVStringGeneration = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
        };

        public CryptLib()
        {
            enc = new UTF8Encoding();
            rcipher = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 256,
                BlockSize = 128
            };
            key = new byte[32];
            iv = new byte[rcipher.BlockSize / 8]; //128 bit / 8 = 16 bytes
            ivBytes = new byte[16];
            defaultKey = "f0685bf1d4c3f4891cb285834029425";
            defaultInitVector = "4PaDRKUPxjXeCkS2";
        }

        /**
		 * This function encrypts the plain text to cipher text using the key
		 * provided. You'll have to use the same key for decryption
		 *
		 * @param plainText
		 *            Plain text to be encrypted
		 * @return returns encrypted (cipher) text
		 */

        public string Encrypt(string plainText)
        {
            return EncryptDecrypt(plainText, defaultKey, EncryptMode.ENCRYPT, defaultInitVector);
        }

        /**
		 * This function encrypts the plain text to cipher text using the key
		 * provided. You'll have to use the same key for decryption
		 *
		 * @param plainText
		 *            Plain text to be encrypted
		 * @param key
		 *            Encryption Key. You'll have to use the same key for decryption
		 * @return returns encrypted (cipher) text
		 */

        public string Encrypt(string plainText, string key, string initVector)
        {
            return EncryptDecrypt(plainText, key, EncryptMode.ENCRYPT, initVector);
        }

        /***
		 * This funtion decrypts the encrypted text to plain text using the key
		 * provided. You'll have to use the same key which you used during
		 * encryprtion
		 *
		 * @param encryptedText
		 *            Encrypted/Cipher text to be decrypted

		 * @return encrypted value
		 */

        public string Decrypt(string encryptedText)
        {
            return EncryptDecrypt(encryptedText, defaultKey, EncryptMode.DECRYPT, defaultInitVector);
        }

        /***
		 * This funtion decrypts the encrypted text to plain text using the key
		 * provided. You'll have to use the same key which you used during
		 * encryprtion
		 *
		 * @param encryptedText
		 *            Encrypted/Cipher text to be decrypted
		 * @param key
		 *            Encryption key which you used during encryption
		 * @return encrypted value
		 */

        public string Decrypt(string encryptedText, string key, string initVector)
        {
            return EncryptDecrypt(encryptedText, key, EncryptMode.DECRYPT, initVector);
        }

        /**
		 *
		 * @param inputText
		 *            Text to be encrypted or decrypted
		 * @param encryptionKey
		 *            Encryption key to used for encryption / decryption
		 * @param mode
		 *            specify the mode encryption / decryption
		 * @param initVector
		 * 			  initialization vector
		 * @return encrypted or decrypted string based on the mode
		*/

        private string EncryptDecrypt(string inputText, string encryptionKey, EncryptMode mode, string initVector)
        {
            string outputString = "";   // output string

            pwd = Encoding.UTF8.GetBytes(encryptionKey);
            ivBytes = Encoding.UTF8.GetBytes(initVector);

            int len = pwd.Length;
            if (len > key.Length)
            {
                len = key.Length;
            }
            int ivLenth = ivBytes.Length;
            if (ivLenth > iv.Length)
            {
                ivLenth = iv.Length;
            }

            Array.Copy(pwd, key, len);
            Array.Copy(ivBytes, iv, ivLenth);
            rcipher.Key = key;
            rcipher.IV = iv;

            if (mode.Equals(EncryptMode.ENCRYPT))
            {
                //encrypt
                byte[] plainText = rcipher.CreateEncryptor().TransformFinalBlock(enc.GetBytes(inputText), 0, inputText.Length);
                outputString = Convert.ToBase64String(plainText);
            }
            if (mode.Equals(EncryptMode.DECRYPT))
            {
                //decrypt
                byte[] plainText = rcipher.CreateDecryptor().TransformFinalBlock(Convert.FromBase64String(inputText), 0, Convert.FromBase64String(inputText).Length);
                outputString = enc.GetString(plainText);
            }
            rcipher.Dispose();
            return outputString;// return encrypted/decrypted string
        }

        /***
		 * This function decrypts the encrypted text to plain text using the key
		 * provided. You'll have to use the same key which you used during
		 * encryption
		 *
		 * @param encryptedText
		 *            Encrypted/Cipher text to be decrypted
		 * @param key
		 *            Encryption key which you used during encryption
		 */

        public string GetHashSha256(string text, int length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x); //covert to hex string
            }
            if (length > hashString.Length)
                return hashString;
            else
                return hashString.Substring(0, length);
        }

        /**
		 * This function generates random string of the given input length.
		 *
		 * @param plainText
		 *            Plain text to be encrypted
		 * @param key
		 *            Encryption Key. You'll have to use the same key for decryption
		 * @return returns encrypted (cipher) text
		 */

        public string GenerateRandomIV(int length)
        {
            char[] _iv = new char[length];
            byte[] randomBytes = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes); //Fills an array of bytes with a cryptographically strong sequence of random values.
            }

            for (int i = 0; i < _iv.Length; i++)
            {
                int ptr = randomBytes[i] % CharacterMatrixForRandomIVStringGeneration.Length;
                _iv[i] = CharacterMatrixForRandomIVStringGeneration[ptr];
            }
            return new string(_iv);
        }
    }
}
