using System.ComponentModel.DataAnnotations;

namespace Employees.Models.UserVM
{
    public class UserCreateVM
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(50, ErrorMessage = "Максимум 50 символов")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Можно вводить только латинские буквы, цифры и знак подчеркивания")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [MaxLength(150, ErrorMessage = "Максимум 150 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public int Role { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public int Employee { get; set; }
    }
}
