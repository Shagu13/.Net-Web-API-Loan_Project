using System;

namespace Loan_Api.Models.DTO
{
    public class RequestLoanDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Type { get; set; }
        public double Amount { get; set; }
        public int LoanPeriodInYears { get; set; }

    }
}
