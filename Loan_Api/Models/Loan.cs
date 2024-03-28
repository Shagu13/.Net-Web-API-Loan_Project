using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loan_Api.Models
{
    public class Loan
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }  
        public string Client { get; set; }
        public User User { get; set; }  

        public LoanType Type { get; set; }

        public double Amount { get; set; }

        [Required]
        public LoanStatus Status { get; set; }  

        public DateTime RequestDate { get; set; } = DateTime.Now;
        public int LoanPeriodInYears { get; set; }
    }

    public enum LoanType
    {
        QuickLoan = 1,
        AutoLoan,
        Installment
    }

    public enum LoanStatus
    {
        Processing = 1,
        Approved,
        Rejected
    }
}

