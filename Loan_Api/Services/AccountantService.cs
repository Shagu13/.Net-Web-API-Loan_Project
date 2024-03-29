using Loan_Api.Models.DTO;
using System.Threading.Tasks;
using System;
using Loan_Api.Data;
using Loan_Api.Models;
using Microsoft.EntityFrameworkCore;
using Loan_Api.Services.IServices;
using Newtonsoft.Json;
using System.Linq;
using Loan_Api.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Loan_Api.Services
{
    public interface IAccountService
    {
        Task<IRequestErrorMsg> SaveAccountant();
        Task<IRequestErrorMsg> Login(LoginDto loginDto);
        Task<IRequestErrorMsg> GetUserDetails(Guid userId);
        Task<IRequestErrorMsg> ChangeLoanStatus(int loanId, Guid userId, AccountantLoanStatusDto userDto);
        Task<IRequestErrorMsg> DeleteUserLoan(int loanId, Guid userId);
        Task<IRequestErrorMsg> ChangeUserStatus(Guid userId, AccountantChangeUserStatusDto userDto);

    }
    public class AccountService : IAccountService
    {
        private readonly UserContext _dbContext;
        private readonly IJwtHelper _jwtHelper;

        public AccountService(UserContext dbContext, IJwtHelper jwtHelper)
        {
            _dbContext = dbContext;
            _jwtHelper = jwtHelper;
        }


        public async Task<IRequestErrorMsg> SaveAccountant()
        {
            try
            {
                var accountant = new Accountant
                {
                    FirstName = "admin",
                    LastName = "admin",
                    UserName = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                    Role = Roles.Accountant,
                };

                await _dbContext.AddAsync(accountant);
                await _dbContext.SaveChangesAsync();

                return RequestErrorMsg.Success("Accountant saved successfully.");
            }
            catch (Exception ex)
            {
                return RequestErrorMsg.Failure($"An error occurred while saving accountant: {ex.InnerException}");
            }
        }

        public async Task<IRequestErrorMsg> Login(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return RequestErrorMsg.Failure("Invalid login data.");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);
            var accountant = await _dbContext.Accountants.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null && accountant == null)
            {
                return RequestErrorMsg.Failure("Invalid username or password");
            }

            if (user != null && !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return RequestErrorMsg.Failure("Invalid username or password");
            }
            else if (accountant != null && !BCrypt.Net.BCrypt.Verify(loginDto.Password, accountant.Password))
            {
                return RequestErrorMsg.Failure("Invalid username or password");
            }

            object userOrAccountant = user ?? (object)accountant;
            var token = _jwtHelper.GenerateAccountantToken(userOrAccountant);

            return RequestErrorMsg.Success(token);
        }
        

        public async Task<IRequestErrorMsg> GetUserDetails(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return RequestErrorMsg.Failure("Invalid user ID.");
                }

                var user = await _dbContext.Users
                    .Include(u => u.Loans)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return RequestErrorMsg.Failure("User not found.");
                }

                var userDetails = new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Age = user.Age,
                    Email = user.Email,
                    Password = user.Password,
                    PassForMe = user.PassForMe,
                    Salary = user.Salary,
                    IsBlocked = user.IsBlocked,
                    Role = user.Role,
                    Loans = user.Loans?.Select(loan => new Loan
                    {
                        Id = loan.Id,
                        UserId = user.Id,
                        Client = loan.Client,
                        Type = (LoanType)loan.Type,
                        Amount = loan.Amount,
                        Status = (LoanStatus)loan.Status,
                        RequestDate = loan.RequestDate,
                        LoanPeriodInYears = loan.LoanPeriodInYears,
                    }).ToList()
                };

                var jsonSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                };

                var jsonResponse = JsonConvert.SerializeObject(userDetails, jsonSettings);

                return RequestErrorMsg.Success(jsonResponse);
            }
            catch (Exception ex)
            {
                return RequestErrorMsg.Failure($"An error occurred while retrieving user details: {ex.InnerException.Message}");
            }
        }

        public async Task<IRequestErrorMsg> ChangeLoanStatus(int loanId, Guid userId, AccountantLoanStatusDto userDto)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return RequestErrorMsg.Failure("Invalid user ID.");
                }

                var user = await _dbContext.Users.FindAsync(userId);

                if (user == null)
                {
                    return RequestErrorMsg.Failure("User not found.");
                }

                var existingLoan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == loanId);

                if (existingLoan == null)
                {
                    return RequestErrorMsg.Failure("Loan not found.");
                }

                if (existingLoan.UserId != userId)
                {
                    return RequestErrorMsg.Failure("Wrong User ID or Loan ID.");
                }

                existingLoan.Status = (LoanStatus)userDto.Status;

                await _dbContext.SaveChangesAsync();

                return RequestErrorMsg.Failure("User details and loan updated successfully.");
            }
            catch (Exception ex)
            {
                return RequestErrorMsg.Failure($"An error occurred while updating user details: {ex.Message}");
            }
        }

        public async Task<IRequestErrorMsg> DeleteUserLoan(int loanId, Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return RequestErrorMsg.Failure("Invalid user ID.");
                }

                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    return RequestErrorMsg.Failure("User not found.");
                }

                var existingLoan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == loanId && l.UserId == userId);
                if (existingLoan == null)
                {
                    return RequestErrorMsg.Failure("Loan not found or does not belong to the specified user.");
                }

                _dbContext.Loans.Remove(existingLoan);
                await _dbContext.SaveChangesAsync();

                return RequestErrorMsg.Success("User loan deleted successfully.");
            }
            catch (Exception ex)
            {
                return RequestErrorMsg.Failure($"An error occurred while deleting user loan: {ex.Message}");
            }
        }
        public async Task<IRequestErrorMsg> ChangeUserStatus(Guid userId, AccountantChangeUserStatusDto userDto)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return RequestErrorMsg.Failure("Invalid user ID.");
                }

                var user = await _dbContext.Users
                    .Include(u => u.Loans)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return RequestErrorMsg.Failure("User not found.");
                }
                user.IsBlocked = userDto.IsBlocked;

                await _dbContext.SaveChangesAsync();

                return RequestErrorMsg.Success("User status changed successfully.");
            }
            catch (Exception ex)
            {

                return RequestErrorMsg.Failure("An error occurred while changing user status.");
            }
        }

    }
}
