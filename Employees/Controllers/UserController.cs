using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Employees.Helpers;
using Employees.Models;
using Employees.Models.UserVM;

namespace Employees.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration config;
        private readonly OrganizationContext context;

        public UserController(IConfiguration config, OrganizationContext context)
        {
            this.config = config;
            this.context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await context.Users.AsNoTracking().ToListAsync();

            IEnumerable<UserPreviewVM> userPreviewVM = users.Select(u => new UserPreviewVM
            {
                Id = u.Id,
                Login = u.Login
            });

            return View(userPreviewVM);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await context.Users
                .Include(u => u.Employee)
                .ThenInclude(u => u.Position)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            string dateFormat = config.GetValue<string>("Birthdate:Format");
            UserDetailsVM userDetails = new UserDetailsVM
            {
                Id = user.Id,
                Login = user.Login,
                Role = user.Role.Name,
                Name = user.Employee.Name,
                Patronymic = user.Employee.Patronymic,
                Surname = user.Employee.Surname,
                Gender = GenderHelper.GetValue(user.Employee.Gender),
                Position = user.Employee.Position.Name,
                Birthdate = user.Employee.Birthdate.ToString(dateFormat),
                Phone = user.Employee.Phone
            };

            return View(userDetails);
        }

        /// <summary>
        /// Create dropdown list for Roles
        /// </summary>
        private SelectList CreateRolesDropDownList() => new SelectList(context.Roles.ToList(), "Id", "Name");

        /// <summary>
        /// Create dropdown list for Employees without account
        /// </summary>
        private SelectList CreateEmployeesWithoutAccountDropDownList()
        {
            var employeesWithoutAccount = from e in context.Employees
                                          where !context.Users.Any(u => u.EmployeeId == e.Id)
                                          select new
                                          {
                                              e.Id,
                                              Details = $"{e.Name} {e.Patronymic} {e.Surname} (возраст: {AgeHelper.GetAge(e.Birthdate)}) - {e.Position.Name}"
                                          };

            return new SelectList(employeesWithoutAccount.ToList(), "Id", "Details");
        }

        // GET: User/Create
        public IActionResult Create()
        {
            ViewData["Roles"] = CreateRolesDropDownList();
            ViewData["Employees"] = CreateEmployeesWithoutAccountDropDownList();

            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = CreateRolesDropDownList();
                ViewData["Employees"] = CreateEmployeesWithoutAccountDropDownList();

                return View(model);
            }

            User user = new User
            {
                Login = model.Login,
                Password = model.Password,
                RoleId = model.Role,
                EmployeeId = model.Employee
            };

            context.Add(user);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            UserEditVM model = new UserEditVM
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Role = user.RoleId,
            };

            ViewData["Roles"] = CreateRolesDropDownList();

            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = CreateRolesDropDownList();

                return View(model);
            }

            User userToEdit = await context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (userToEdit == null)
            {
                return NotFound();
            }

            userToEdit.Login = model.Login;
            
            if (!string.IsNullOrEmpty(model.Password))
            {
                userToEdit.Password = model.Password;
            }

            userToEdit.RoleId = model.Role;

            await context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        // GET: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = new User { Id = id.Value };
            context.Entry(user).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
