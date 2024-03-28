using Loan_Api.Data;
using Loan_Api.Helper;
using Loan_Api.Models;
using Loan_Api.Models.DTO;
using Loan_Api.Services;
using Loan_Api_Project.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Loan_Api.Controllers
{
    [Authorize(Roles = Roles.Accountant)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantController : ControllerBase
    {
        private readonly UserContext _dbContext;
        private readonly IAccountService _accountService;
        private readonly AppSettings _appSettings;
        private readonly IJwtHelper _jwtHelper;
        public AccountantController(UserContext dbContext, IOptions<AppSettings> appSettings, IJwtHelper jwtHelper, IAccountService accountService)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
            _jwtHelper = jwtHelper;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> SaveAccountant()
        {
            var result = await _accountService.SaveAccountant();

            if (result.IsSuccess)
            {
                return Ok(result.ErrorMessage);
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _accountService.Login(loginDto);

            if (result.IsSuccess)
            {
                return Ok(new { Token = result.ErrorMessage });
            }
            else
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
        }



        [Authorize(Roles = Roles.Accountant)]
        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetails(Guid userId)
        {
            var result = await _accountService.GetUserDetails(userId);

            if (result.IsSuccess)
            {
                return Ok(result.ErrorMessage);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [Authorize(Roles = Roles.Accountant)]
        [HttpPut("ChangeLoanStatus/{loanId}")]
        public async Task<IActionResult> ChangeLoanStatus(int loanId, Guid userId, AccountantLoanStatusDto userDto)
        {
            var result = await _accountService.ChangeLoanStatus(loanId, userId, userDto);

            if (result.IsSuccess)
            {
                return Ok(result.ErrorMessage);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [Authorize(Roles = Roles.Accountant)]
        [HttpDelete("DeleteUserLoan/{loanId}")]
        public async Task<IActionResult> DeleteUserLoan(int loanId, Guid userId)
        {
            var result = await _accountService.DeleteUserLoan(loanId, userId);

            if (result.IsSuccess)
            {
                return Ok(result.ErrorMessage);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}
       




