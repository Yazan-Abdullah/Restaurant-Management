using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management.Core.DTO;
using Restaurant_Management.Core.Models;
using Restaurant_Management.Core.Repository;
using Restaurant_Management.Infra.Repository;

namespace Restaurant_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepo _MenuRepo;
        private readonly IConfiguration _configuration;
        public MenuController(IMenuRepo menuRepo, IConfiguration configuration)
        {
            _MenuRepo = menuRepo;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("AllMenu")]
        public IActionResult GetMenus(int pageSize,int pageNumber)
        {
            List<Menu> menus = _MenuRepo.GetMenus();
            int skipAmount = pageSize * pageNumber - (pageSize);
            return Ok(menus.Skip(skipAmount).Take(pageSize));
            
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
        public async Task<IActionResult> CreateMenu(AddMenuDTO menuDto)
        {
            try
            {
                var createdMenu = await _MenuRepo.CreateMenuAsync(menuDto);
                return Ok("Menu Created Sucessfule");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateMenus/{id}")]
        public async Task<IActionResult> UpdateMenu(int id, UpdateMenuDTO updateMenuDto)
        {
            try
            {
                await _MenuRepo.UpdateMenuAsync(id, updateMenuDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the menu.");
            }
        }
    }
}
