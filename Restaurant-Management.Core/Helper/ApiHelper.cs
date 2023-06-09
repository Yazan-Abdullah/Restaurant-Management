using Aspose.Email.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Restaurant_Management.Core.Helper
{
    public class ApiHelper
    {
        private readonly IConfiguration _configuration;
        private readonly RestaurantContext _DbContext;
        private string constraints, outlookemail, outlookpassword;
        public ApiHelper(RestaurantContext DbContext)
        {
            this._DbContext = DbContext;
        }
        public ApiHelper(RestaurantContext DbContext, IConfiguration configuration)
        {
            this._DbContext = DbContext;
            _configuration = configuration;
            constraints = configuration.GetValue<String>("TokenSecrect");
            outlookemail = configuration.GetValue<String>("OutLookEmail");
            outlookpassword = configuration.GetValue<String>("OutLookPassword");
        }  


        public string GenerateJwtToken(loginResponsDTO loginCredinital, string constraints)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string jwtToken = constraints;
            var tokenKey = Encoding.UTF8.GetBytes(constraints);
            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Email,loginCredinital.Email),
                        new Claim("CustomerId",loginCredinital.CustomerId.ToString()),                      
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey)
                , SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptior);
            return tokenHandler.WriteToken(token);
        }
        public bool ValidateJWTtoken(string tokenString, out loginResponsDTO respon)
        {
            var toke = "Bearer " + tokenString;
            var jwtEncodedString = toke.Substring(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(constraints)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
                var principal = tokenHandler.ValidateToken(jwtEncodedString, tokenValidationParameters, out _);

                var customerIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "CustomerId");
                var EmailClaim = principal.Claims.FirstOrDefault(c => c.Type == "Email");

                if (customerIdClaim != null && EmailClaim != null)
                {
                var tempResponse = new loginResponsDTO
                {
                    CustomerId = int.Parse(customerIdClaim.Value),
                    Email = EmailClaim.Value
                    };
                    respon = tempResponse;
                    return true;
                }
            respon = null;
            return false;
        }
        public string GenerateSHA384String(string inputString)
        {
            SHA384 sha512 = SHA384Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        #region Enctyption
        public string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            byte[] encrypted;

            // Create an Aes object with the specified key and IV.
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Create a new MemoryStream object to contain the encrypted bytes.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Create a CryptoStream object to perform the encryption.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Encrypt the plaintext.
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        encrypted = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            string decrypted;

            // Create an Aes object with the specified key and IV.
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Create a new MemoryStream object to contain the decrypted bytes.
                using (MemoryStream memoryStream = new MemoryStream(cipherText))
                {
                    // Create a CryptoStream object to perform the decryption.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        // Decrypt the ciphertext.
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            decrypted = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }
        #endregion
    }
}
