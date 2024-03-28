namespace Loan_Api.Services.IServices
{
    public interface ILoanErrorMsg
    {
        bool IsSuccess { get; }
        string ErrorMessage { get; }
    }
    public class LoanErrorMsg : ILoanErrorMsg
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        public LoanErrorMsg(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
