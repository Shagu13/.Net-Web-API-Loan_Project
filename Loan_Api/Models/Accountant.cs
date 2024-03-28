using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loan_Api.Models
{
    public class Accountant 
    {
        public int Id { get; set; }

        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; } = Roles.Accountant;
        public ICollection<User> Users { get; set; }

    }


}
