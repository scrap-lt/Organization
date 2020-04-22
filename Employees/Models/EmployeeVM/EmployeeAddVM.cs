using System;
using System.ComponentModel.DataAnnotations;

using Employees.Filters;

namespace Employees.Models.EmployeeVM
{
    public class EmployeeAddVM
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(150, ErrorMessage = "Максимум 150 символов")]
        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Можно вводить только русские буквы")]
        public string Name { get; set; }

        [MaxLength(150, ErrorMessage = "Максимум 150 символов")]
        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Можно вводить только русские буквы")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(150, ErrorMessage = "Максимум 150 символов")]
        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Можно вводить только русские буквы")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public byte Gender { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public int Position { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Date)]
        [Birthdate]
        public DateTime Birthdate { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+7\s\(\d{3}\)\s\d{3}-\d{2}-\d{2}", ErrorMessage = "Формат телефона: +7 (xxx) xxx-xx-xx")]
        public string Phone { get; set; }
    }
}
