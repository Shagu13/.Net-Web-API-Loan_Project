using System.Reflection.Metadata.Ecma335;

namespace Loan_Api_Project.Models.DTO
{
    public class RegisterDto
    {
        private string _firstName;
        private string _lastName;
        private string _email;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value?.ToLower(); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value?.ToLower(); }
        }
        public string UserName { get;set; }
        
        public int Age { get; set; }
        public double Salary { get; set; }
        public string Email
        {
            get { return _email; }
            set { _email = value?.ToLower(); }
        }

        public string Password { get; set; }
    }
}
