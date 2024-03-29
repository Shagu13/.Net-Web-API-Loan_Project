Backend:
 - C#
 - ASP.NET Core
 - Entity Framework Core
 - JWT Authentication
 - BCrypt Password Hashing
 
 Database:
 - Microsoft SQL Server

Folder Structure:

Controllers:
 - Contains controllers for handling HTTP requests and implementing application logic.
Data:
 - Contains the database context and migration files.
Helper:
 - Contains helper classes for JWT generation and other utility functions.
Models:
 - Contains DTOs (Data Transfer Objects) and entity models used throughout the application.
Services:
 - Contains service classes implementing business logic for various functionalities.
AppSettings.json:
 - Configuration file for storing application settings.

Methods 
- SaveAccountant() - saves/creates new Accountant user.
- Login() - used to Login as an Accountant.
- GetUserDetails() - gets user details and loans if there are any by using User Guid ID.
- ChangeLoanStatus() - changes users loan status(Enum Based data - Processing,Approved,Rejected).
- DeleteUserLoan() - Deletes certain loan by user Guid ID and loan ID.
- Register() - register new user.
- Login() - to log in and generate JWT(jason web token).
- RequestLoan() - post end point, user requests a loan,by default loan is being processed unless accountant chnanges its status.
- UpdateLoan() - user can change/update loan Type(Quick Loan,Auto Loan,Installment) if it is still processing, otherwise he/she will not have any rights.
- DeleteLoan() - user can delete his loan, its similar to UpdateLoan as delete is possible only if loan is being processed.
- RegisterValidator is used to validate swagger inputs.

Setup Instructions:
 - Clone the repository to your local machine.
 - Open the solution file in Visual Studio.
 - Restore NuGet packages and build the solution.
 - Update the connection string in appsettings.json to point to your SQL Server instance.
 - Run the Entity Framework Core migrations to create the database schema.
 - Start the application.
 
NuGet Packeges :
 - Newtonsoft.Json - Version=13.0.3
 - BCrypt.Net-Next  - Version=4.0.3
 - FluentValidation - Version=10.3.6
 - FluentValidation.AspNetCore - Version=10.3.6
 - FluentValidation.DependencyInjectionExtensions - Version=10.3.6
 - Microsoft.AspNetCore.Authentication.JwtBearer - Version=5.0.0
 - Microsoft.EntityFrameworkCore.SqlServer - Version=5.0.0
 - Microsoft.EntityFrameworkCore.Tools - Version=5.0.0

