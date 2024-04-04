using System;

namespace EmployeeSys.Models
{
    public class RegisterModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public int Employeenumber { get; set; }
        public string Manager { get; set; }
        public string Role { get; set; }
        public decimal Salary { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
