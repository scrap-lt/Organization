using System;

namespace Employees.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }
        public byte Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phone { get; set; }

        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
