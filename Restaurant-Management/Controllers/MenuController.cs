using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Restaurant_Management.Core.DTO;

using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;
using Serilog;

namespace Restaurant_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepo _MenuRepo;
        private readonly IConfiguration _configuration;
        private readonly Helper _helper;
        private readonly RestaurantContext _DbContext;
        public MenuController(IMenuRepo menuRepo, IConfiguration configuration, RestaurantContext DbContext)
        {
            _MenuRepo = menuRepo;
            _configuration = configuration;
            _helper = new Helper(_DbContext,_configuration);
            _DbContext = DbContext;
        }
        [HttpGet]
        [Route("AllMenu")]
        public IActionResult GetMenus( string token ,int pageSize,int pageNumber)
        {
            if (_helper.ValidateJWTtoken(token))
            {
                List<Menu> menus = _MenuRepo.GetMenus();
                int skipAmount = pageSize * pageNumber - (pageSize);
                Log.Information("Successfully Get All Menus ");
                return Ok(menus.Skip(skipAmount).Take(pageSize));
            }
            Log.Error("Failed Get All Menus : Invalid Token");
            return Unauthorized("Invalid Token");
        }
        [HttpGet]
        [Route("GetMenuById/{id}")]
        public IActionResult GetMenuById(int id)
        {
            try
            {
                var menu = _MenuRepo.GetMenuById(id);

                if (menu != null)
                {
                    return Ok(menu);

                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }
        [HttpPost]
        [Route("AddMenu")]
        public async Task<IActionResult> CreateMenu(string token, AddMenuDTO menuDto)
        {
            try
            {
                if (_helper.ValidateJWTtoken(token))
                {
                    var createdMenu = await _MenuRepo.CreateMenuAsync(menuDto);
                    Log.Information("Successfully Create Menu ");
                    return Ok("Menu Created Sucessfule");
                }
                Log.Error("Failed Create Menu : Invalid Token");
                return Unauthorized("Invalid Token");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("UpdateMenus/{id}")]
        public async Task<IActionResult> UpdateMenu( string token,int id, UpdateMenuDTO updateMenuDto)
        {
            try
            {
                if (_helper.ValidateJWTtoken(token))
                {
                    await _MenuRepo.UpdateMenuAsync(id, updateMenuDto);
                    Log.Information("Successfully Updated Menu");
                    return NoContent();
                }
                Log.Error("Failed Update Menu : Invalid Token");
                return Unauthorized("Invalid Token");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the menu.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu( string token, int id)
        {
            try
            {
                if (_helper.ValidateJWTtoken(token))
                {
                    await _MenuRepo.DeleteMenuAsync(id);
                    Log.Information("Successfully Delete Menu");
                    return Ok("Menu deleted successfully.");
                }
                Log.Error("Failed Delete Menu : Invalid Token");
                return Unauthorized("Invalid Token");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the menu: {ex.Message}");
            }
        }
    }
}
