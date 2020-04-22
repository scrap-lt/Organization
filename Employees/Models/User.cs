﻿namespace Employees.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
