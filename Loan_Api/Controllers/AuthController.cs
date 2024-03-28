using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using Loan_Api_Project.Helper;
using Loan_Api_Project.Models.DTO;
using Loan_Api.Models;
using Microsoft.EntityFrameworkCore;
using Loan_Api.Models.DTO;
using Loan_Api.Data;
using Loan_Api.Helper;

namespace Loan_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserContext _dbContext;
        private readonly IJwtHelper _jwtHelper;
        public AuthController(UserContext dbContext, IOptions<AppSettings> appSettings, IJwtHelper jwtHelper)
        {
            _dbContext = dbContext;
            _jwtHelper = jwtHelper;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto userDto)
        {
            try
            {
                if (await _dbContext.Users.AnyAsync(u => u.UserName == userDto.UserName || u.Email == userDto.Email))
                    return Conflict($"Either {userDto.UserName} or {userDto.Email} is already in use.");

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    Age = userDto.Age,
                    Salary = userDto.Salary,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    PassForMe = userDto.Password
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while creating the user. {ex.Message}{ex.InnerException}");
            }
        }


        [AllowAnonymous]
        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.UserName) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Invalid login data.");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return BadRequest("Invalid username or password");
            }

            var token = _jwtHelper.GenerateUserToken(user);

            return Ok(new { Token = token });
        }

    }
}
