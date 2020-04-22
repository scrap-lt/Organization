using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Employees.Helpers;
using Employees.Models;
using Employees.Models.EmployeeVM;

namespace Employees.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration config;
        private readonly OrganizationContext context;

        public EmployeeController(IConfiguration config, OrganizationContext context)
        {
            this.config = config;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await context.Employees
                .Include(p => p.Position)
                .AsNoTracking()
                .ToListAsync();

            string dateFormat = config.GetValue<string>("Birthdate:Format");
            IEnumerable<EmployeePreviewVM> employeesPreview = employees.Select(e => new EmployeePreviewVM
            {
                Id = e.Id,
                Name = e.Name,
                Patronymic = e.Patronymic,
                Surname = e.Surname,
                Gender = GenderHelper.GetValue(e.Gender),
                Position = e.Position.Name,
                Birthdate = e.Birthdate.ToString(dateFormat),
                Phone = e.Phone
            });

            return View(employeesPreview);
        }

        /// <summary>
        /// Create dropdown list for Genders
        /// </summary>
        private SelectList CreateGendersDropDownList() => new SelectList(GenderHelper.CreateList(), "Value", "Text");

        /// <summary>
        /// Create dropdown list for Positions
        /// </summary>
        private SelectList CreatePositionsDropDownList() => new SelectList(context.Positions.ToList(), "Id", "Name");

        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Genders"] = CreateGendersDropDownList();
            ViewData["Positions"] = CreatePositionsDropDownList();

            return View();
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EmployeeAddVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Genders"] = CreateGendersDropDownList();
                ViewData["Positions"] = CreatePositionsDropDownList();

                return View(model);
            }

            Employee employee = new Employee
            {
                Name = model.Name,
                Patronymic = model.Patronymic,
                Surname = model.Surname,
                Gender = model.Gender,
                PositionId = model.Position,
                Birthdate = model.Birthdate,
                Phone = model.Phone
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = await context.Employees
                .Include(p => p.Position)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            EmployeeEditVM model = new EmployeeEditVM
            {
                Id = employee.Id,
                Name = employee.Name,
                Patronymic = employee.Patronymic,
                Surname = employee.Surname,
                Gender = employee.Gender,
                Position = employee.Position.Id,
                Birthdate = employee.Birthdate,
                Phone = employee.Phone
            };

            ViewData["Genders"] = CreateGendersDropDownList();
            ViewData["Positions"] = CreatePositionsDropDownList();

            return View(model);
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Genders"] = CreateGendersDropDownList();
                ViewData["Positions"] = CreatePositionsDropDownList();

                return View(model);
            }

            Employee employeeToEdit = await context.Employees.FirstOrDefaultAsync(e => e.Id == model.Id);
            if (employeeToEdit == null)
            {
                return NotFound();
            }

            employeeToEdit.Name = model.Name;
            employeeToEdit.Patronymic = model.Patronymic;
            employeeToEdit.Surname = model.Surname;
            employeeToEdit.Gender = model.Gender;
            employeeToEdit.PositionId = model.Position;
            employeeToEdit.Birthdate = model.Birthdate;
            employeeToEdit.Phone = model.Phone;

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = new Employee { Id = id.Value };
            context.Entry(employee).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        [ActionName("Genders")]
        public async Task<IActionResult> GetGenderRates()
        {
            int totalEmployees = await context.Employees.CountAsync();
            int males = await context.Employees.CountAsync(e => e.Gender == 1);
            double malePercent = (double)males / totalEmployees * 100;
            int females = await context.Employees.CountAsync(e => e.Gender == 2);
            double femalePercent = (double)females / totalEmployees * 100;

            var gendersData = new[]
            {
                new
                {
                    gender = "male",
                    rate = malePercent
                },
                new
                {
                    gender = "female",
                    rate = femalePercent
                }
            };

            ViewData["GenderData"] = JsonSerializer.Serialize(gendersData);

            return View();
        }

        [Authorize]
        [ActionName("BirthDecade")]
        public async Task<IActionResult> GetBirthDecadeRates()
        {
            int totalEmployees = await context.Employees.CountAsync();
            List<int> birthdates = await context.Employees.Select(e => AgeHelper.GetDecade(e.Birthdate))
                .ToListAsync();

            var decadesData = birthdates.GroupBy(e => e)
                .Select(g => new
                 {
                     decade = g.Key.ToString(),
                     rate = (double)g.Count() / totalEmployees * 100
                 });

            ViewData["DecadeData"] = JsonSerializer.Serialize(decadesData);

            return View();
        }
    }
}
