using System.Security.Cryptography;
using System.Text;

namespace QuizPracticeApi.Helpers {
    public class EncryptionHelper {
        /// <summary>
        /// Encodes a string to Base64 format.
        /// </summary>
        /// <param name="plainText">The plain text to encode.</param>
        /// <returns>Base64 encoded string.</returns>
        public static string Base64Encode(string plainText) {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Decodes a Base64 string to its original format.
        /// </summary>
        /// <param name="base64Text">The Base64 encoded string.</param>
        /// <returns>Decoded plain text.</returns>
        public static string Base64Decode(string base64Text) {
            byte[] base64Bytes = Convert.FromBase64String(base64Text);
            return Encoding.UTF8.GetString(base64Bytes);
        }

        /// <summary>
        /// Validates an activation token.
        /// </summary>
        /// <param name="token">The activation token.</param>
        /// <param name="userId">The user ID to validate against.</param>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        public static bool ValidateActivateToken(string token, string username, IConfiguration _configuration) {
            bool isValid = true;
            try {
                string data = Base64Decode(token);
                var datas = data.Split("::");
                var key = _configuration.GetSection("Jwt")["Key"];
                if (datas[0] != username) {
                    isValid = false;
                }
                if (DateTime.TryParse(datas[1], out DateTime dataDate)) {
                    if (dataDate < DateTime.UtcNow) {
                        isValid = false;
                    }
                }
                if (key != datas[2]) {
                    isValid = false;
                }

            } catch {
                isValid = false;
            }
            
            return isValid;
        }

        /// <summary>
        /// Generates a secure token based on the user ID and a timestamp.
        /// </summary>
        public static string GenerateActivateToken(string username, IConfiguration _configuration) {
            string secretKey = _configuration.GetSection("Jwt")["Key"];
            string input = $"{username}::{DateTime.UtcNow.AddMinutes(5):yyyy-MM-dd HH:mm:ss}::{secretKey}";
            return Base64Encode(input);
        }
    }
}
