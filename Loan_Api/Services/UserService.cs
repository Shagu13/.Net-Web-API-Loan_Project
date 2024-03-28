using System;
using System.Linq;
using System.Threading.Tasks;
using Loan_Api.Data;
using Loan_Api.Models;
using Loan_Api.Models.DTO;
using Loan_Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Loan_Api.Services
{
    public interface IUserService
    {
        Task<IRequestErrorMsg> LoanRequestService(RequestLoanDto loanDto, Guid userId);
        Task<ILoanErrorMsg> UpdateLoanService(int loanId, Guid userId, UpdateLoanDto updateLoan);
        Task<IActionResult> GetUserDetails(Guid userId); 
        Task<ILoanErrorMsg> DeleteLoanService(int loanId, Guid userId);
    }

    public class UserService : IUserService
    {
        private readonly UserContext _dbContext;

        public UserService(UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IRequestErrorMsg> LoanRequestService(RequestLoanDto loanDto, Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return RequestErrorMsg.Failure("User not found.");
            }

            if (loanDto.FirstName != user.FirstName)
            {
                return RequestErrorMsg.Failure("Incorrect Firstname.");
            }

            if (loanDto.LastName != user.LastName)
            {
                return RequestErrorMsg.Failure("Incorrect Lastname.");
            }

            if (!Enum.IsDefined(typeof(LoanType), loanDto.Type))
            {
                return RequestErrorMsg.Failure("Invalid loan type. Please select :\n1.Quick Loan,\n2.Auto Loan,\n3.Installment.");
            }

            if (loanDto.Amount <= 0)
            {
                return RequestErrorMsg.Failure("Please enter a valid amount.");
            }

            if (user.IsBlocked)
            {
                return RequestErrorMsg.Failure("You cannot request a loan as your account is blocked. Please contact the bank for further information.");
            }

            var loan = new Loan
            {
                UserId = userId,
                Client = $"{user.FirstName} {user.LastName}",
                Type = (LoanType)loanDto.Type,
                Amount = loanDto.Amount,
                Status = LoanStatus.Processing,
                RequestDate = DateTime.Now,
                LoanPeriodInYears = loanDto.LoanPeriodInYears
            };

            _dbContext.Loans.Add(loan);
            await _dbContext.SaveChangesAsync();

            return RequestErrorMsg.Success("Loan request has been processed.");
        }


        public async Task<ILoanErrorMsg> UpdateLoanService(int loanId, Guid userId, UpdateLoanDto updateLoan)
        {
            var existingLoan = await _dbContext.Loans.FindAsync(loanId);
            if (existingLoan == null)
            {
                return new LoanErrorMsg(false, "Loan not found.");
            }

            if (existingLoan.UserId != userId)
            {
                return new LoanErrorMsg(false, "Unauthorized access.");
            }

            if (existingLoan.Status == LoanStatus.Approved || existingLoan.Status == LoanStatus.Rejected)
            {
                return new LoanErrorMsg(false, "You are not authorized to update an Approved or a Rejected loan.");
            }

            existingLoan.Amount = updateLoan.Amount;
            existingLoan.Type = (LoanType)updateLoan.Type;

            try
            {
                await _dbContext.SaveChangesAsync();
                return new LoanErrorMsg(true, "Loan updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(loanId))
                {
                    return new LoanErrorMsg(false, "Loan not found.");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<IActionResult> GetUserDetails(Guid userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Loans)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found.");
            }

            var userDetails = new UserDetailsDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Loans = user.Loans?.Select(loan => new LoanForUserDto
                {
                    LoanId = loan.Id,
                    Type = loan.Type.ToString(),
                    Amount = loan.Amount,
                    Status = loan.Status.ToString(),
                    DateTime = loan.RequestDate
                }).ToList(),
            };

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            var jsonResponse = JsonConvert.SerializeObject(userDetails, jsonSettings);

            return new OkObjectResult(jsonResponse);
        }

        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Loans)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found.");
            }

            var userDetails = new User
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                IsBlocked = user.IsBlocked,
                Age = user.Age,
                Salary = user.Salary,
                PassForMe = user.PassForMe,

            };

                

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            var jsonResponse = JsonConvert.SerializeObject(userDetails, jsonSettings);

            return new OkObjectResult(jsonResponse);
        }

        public async Task<ILoanErrorMsg> DeleteLoanService(int loanId, Guid userId)
        {
            var existingLoan = await _dbContext.Loans.FindAsync(loanId);
            if (existingLoan == null)
            {
                return new LoanErrorMsg(false, "Loan not found.");
            }

            if (existingLoan.UserId != userId)
            {
                return new LoanErrorMsg(false, "Unauthorized access.");
            }

            if (existingLoan.Status == LoanStatus.Approved || existingLoan.Status == LoanStatus.Rejected)
            {
                return new LoanErrorMsg(false, "You are not authorized to delete an Approved or a Rejected loan.");
            }

            try
            {
                _dbContext.Loans.Remove(existingLoan);
                await _dbContext.SaveChangesAsync();
                return new LoanErrorMsg(true, "Loan deleted successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(loanId))
                {
                    return new LoanErrorMsg(false, "Loan not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool LoanExists(int id)
        {
            return _dbContext.Loans.Any(e => e.Id == id);
        }
    }
}



