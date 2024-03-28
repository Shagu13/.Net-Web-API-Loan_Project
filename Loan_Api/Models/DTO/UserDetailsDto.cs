using System;
using System.Collections.Generic;

namespace Loan_Api.Models.DTO
{
    public class UserDetailsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<LoanForUserDto> Loans { get; set; }

    }
    public class LoanForUserDto
    {
        public int LoanId { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public int LoanPeriodInYears { get; set; }


    }
}
