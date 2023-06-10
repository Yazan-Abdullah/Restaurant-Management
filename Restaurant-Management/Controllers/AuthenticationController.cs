using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management.Core.DTO;

using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;
using Serilog;
using System.Security.Cryptography;

namespace Restaurant_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly RestaurantContext _DbContext;
        private readonly Helper _helper;
        public AuthenticationController(RestaurantContext DbContext, IConfiguration configuration)
        {
            _DbContext = DbContext;
            _configuration = configuration;
            _helper = new Helper(_DbContext, _configuration);
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult CreateAccount([FromBody] RegesterDTO newUser)
        {
            Customer user = new Customer();
            using (Aes aes = Aes.Create())
            {
                user.Name = _helper.EncryptString(newUser.Name, aes.Key, aes.IV); ;
                user.Email = _helper.EncryptString(newUser.Email, aes.Key, aes.IV); ;
                user.Phone = _helper.EncryptString(newUser.Phone, aes.Key, aes.IV); ;
                user.Key = Convert.ToBase64String(aes.Key);
                user.Iv = Convert.ToBase64String(aes.IV);
                _DbContext.Add(user);
                _DbContext.SaveChanges();
            }
            Login login = new Login();
            login.Email = _helper.GenerateSHA384String(newUser.Email);
            login.Password = _helper.GenerateSHA384String(newUser.Password);
            login.CustomerId = _DbContext.Customers.Where(x => x.Email == user.Email).OrderByDescending
                (x => x.CustomerId).First().CustomerId;
            _DbContext.Add(login);
            _DbContext.SaveChanges();
            //_helper.SendOTPCode(newUser.Email, (int)login.CustomerId);
            return Ok();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                login.Email = _helper.GenerateSHA384String(login.Email);
                login.Password = _helper.GenerateSHA384String(login.Password);
                var CustomerLoginInfo = await _DbContext.Logins.Where(x => x.Email == login.Email && x.Password == login.Password).SingleOrDefaultAsync();
                if (CustomerLoginInfo == null)
                {
                    // unauth
                    return Unauthorized("Either Username or Password is incorrect ");
                }
                else
                {
                    //Update Login 
                    CustomerLoginInfo.IsActive = true;
                    _DbContext.Update(CustomerLoginInfo);
                    await _DbContext.SaveChangesAsync();
                    var customer = _DbContext.Customers.Where(x => x.CustomerId == CustomerLoginInfo.CustomerId).FirstOrDefault();
                    loginResponsDTO respon = new loginResponsDTO();
                    respon.CustomerId = customer.CustomerId;
                    respon.Email = customer.Email;
                    respon.loginId = _DbContext.Logins.Where(x => x.CustomerId == customer.CustomerId).First().LoginId;
                    
                    return Ok(_helper.GenerateJwtToken(respon));
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed Login from {0}", login.Email);
                return Unauthorized("Invalid Operation");
            }
           
        }
    }
}