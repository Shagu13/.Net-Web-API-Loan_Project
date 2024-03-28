using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loan_Api.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PassForMe { get; set; }

        public double Salary { get; set; }

        public bool IsBlocked { get; set; } = false;

        public string Role { get; set; } = Roles.User;
        public ICollection<Loan> Loans { get; set; }
        

    }

    public static class Roles
    {
        public const string Accountant = "accountant";
        public const string User = "user";
    }
}
