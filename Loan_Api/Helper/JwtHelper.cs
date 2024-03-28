using Loan_Api.Models;
using Loan_Api_Project.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.Extensions.Options;

namespace Loan_Api.Helper
{
    public interface IJwtHelper
    {
        string GenerateUserToken(object userOrAccountant);
        string GenerateAccountantToken(object userOrAccountant);

    }
    public class JwtHelper : IJwtHelper
    {
        private readonly AppSettings _appSettings;
        public JwtHelper (IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        public string GenerateUserToken(object userOrAccountant)
        {
            if (userOrAccountant is User user)
            {
                return GenerateUserToken(user);
            }

            throw new ArgumentException("Invalid object type");
        }

        private string GenerateUserToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        

        public string GenerateAccountantToken(object userOrAccountant)
        {
            if (userOrAccountant == null)
            {
                throw new ArgumentNullException(nameof(userOrAccountant), "The userOrAccountant object is null.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var accountId = string.Empty;
            var firstName = string.Empty;
            var role = string.Empty;

            if (userOrAccountant is User user)
            {
                accountId = user.Id.ToString();
                firstName = user.FirstName;
                role = user.Role;
            }
            else if (userOrAccountant is Accountant accountant)
            {
                if (accountant == null)
                {
                    throw new ArgumentNullException(nameof(accountant), "The accountant object is null.");
                }

                accountId = accountant.Id.ToString();
                firstName = accountant.FirstName;
                role = accountant.Role;
            }
            else
            {
                throw new ArgumentException("Invalid object type", nameof(userOrAccountant));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, accountId),
            new Claim(ClaimTypes.Name, firstName),
            new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }

}
