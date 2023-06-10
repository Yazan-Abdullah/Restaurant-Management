using Restaurant_Management.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_Management.Core.Repository
{
    public interface IHelper
    {
        public string GenerateJwtToken(loginResponsDTO loginCredinital);
        public bool ValidateJWTtoken(string tokenString);
        public bool ValidateJWTtoken(string tokenString, out loginResponsDTO respon);
        public string GenerateSHA384String(string inputString);
        public string GetStringFromHash(byte[] hash);
        public void SendOTPCode(string Email, int CustomerId);
        public string EncryptString(string plainText, byte[] key, byte[] iv);
        public string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv);
    }
}
