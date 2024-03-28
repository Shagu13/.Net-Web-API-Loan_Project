using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Loan_Api.Models.DTO;
using Loan_Api.Services;

namespace Loan_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _loanRequestService;
        private readonly IUserService _updateLoanService;
        private readonly IUserService _getUserDetails;
        private readonly IUserService _deleteLoanService;
        public UserController(IUserService loanRequestService, IUserService updateLoanService, IUserService getUserDetails, IUserService deleteLoanService)
        {
            _loanRequestService = loanRequestService;
            _updateLoanService = updateLoanService;
            _getUserDetails = getUserDetails;
            _deleteLoanService = deleteLoanService;
        }

        [HttpGet("Details")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return BadRequest("User ID not found in token.");
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var result = await _getUserDetails.GetUserDetails(userId);
            return result;
        }


        [HttpPut("UpdateLoan/{loanId}")]
        public async Task<IActionResult> UpdateLoan(int loanId, [FromQuery] Guid userId, [FromBody] UpdateLoanDto updateLoan)
        {
            var result = await _updateLoanService.UpdateLoanService(loanId, userId, updateLoan);

            if (result.IsSuccess)
            {
                return Ok(result.ErrorMessage);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }


        [Authorize]
        [HttpPost("RequestLoan")]
        public async Task<IActionResult> RequestLoan([FromBody] RequestLoanDto loanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var result = await _loanRequestService.LoanRequestService(loanDto, userId);

            if (result.IsSuccess)
            {
                return Ok(result.ErrorMessage);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }           
        }
        [Authorize]
        [HttpDelete("DeleteLoan/{loanId}")]
        public async Task<IActionResult> DeleteLoan(int loanId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var result = await _deleteLoanService.DeleteLoanService(loanId, userId);
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
