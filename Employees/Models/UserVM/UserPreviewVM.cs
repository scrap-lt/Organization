using System.ComponentModel.DataAnnotations;

namespace Employees.Models.UserVM
{
    public class UserPreviewVM
    {
        public int Id { get; set; }

        [Display(Name = "Логин")]
        public string Login { get; set; }
    }
}
