namespace Loan_Api.Services.IServices
{
    public interface IRequestErrorMsg
    {
        bool IsSuccess { get; }
        string ErrorMessage { get; }
    }

    public class RequestErrorMsg : IRequestErrorMsg
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        private RequestErrorMsg(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static RequestErrorMsg Success(string message = null)
        {
            return new RequestErrorMsg(true, message);
        }

        public static RequestErrorMsg Failure(string errorMessage)
        {
            return new RequestErrorMsg(false, errorMessage);
        }
    }
}
