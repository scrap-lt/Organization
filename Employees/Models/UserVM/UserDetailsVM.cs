using System.ComponentModel.DataAnnotations;

namespace Employees.Models.UserVM
{
    public class UserDetailsVM
    {
        public int Id { get; set; }

        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Display(Name = "Роль")]
        public string Role { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [Display(Name = "Должность")]
        public string Position { get; set; }

        [Display(Name = "Дата рождения")]
        public string Birthdate { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }
    }
}
