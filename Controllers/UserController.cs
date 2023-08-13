using backend.Context;
using backend.Models;
using backend.Service;
using backend.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private readonly PasswordService _passwordService;

        public UserController(AppDBContext context, PasswordService passwordService)
        {
            _dbContext = context;
            _passwordService = passwordService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<User>>Signup([FromBody] User user) 
        {
            if (await _dbContext.users.AnyAsync(a => a.Email == user.Email || a.Name == user.Name))
            {
                return BadRequest("user already exists");
            }

            user.Password = _passwordService.HashPassword(user.Password);

            _dbContext.users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Registration successfully");
        }


        [HttpPost("login")]
        public async Task<ActionResult<User>>Login([FromBody] LoginDTO loginDTO){
            var user = await _dbContext.users.FirstOrDefaultAsync(a => a.Email == loginDTO.EmailOrName || a.Name == loginDTO.EmailOrName);
            
            if (user == null || !_passwordService.VerifyPassword(user.Password, loginDTO.Password))
            {
                return BadRequest("Invalid name or password");
            }
            return Ok("Login successful");
        }
    }
}
