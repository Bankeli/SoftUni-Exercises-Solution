using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var dbContext = new SoftUniContext();

            string result = GetEmployeesInPeriod(dbContext);
                Console.WriteLine(result);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                 .Select(e => new
                 {
                     e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                 })
                 .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employeeWithBigSalary = context.Employees
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(e => e.Salary > 50000)
                .ToList();

            foreach (var e in employeeWithBigSalary)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }    

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Department = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department} - ${e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
                
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var nakovAdress = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            nakovAdress.Address = new Models.Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10)
                .ToList();

            return string.Join(Environment.NewLine, employees );
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var empolyeeInfo = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager != null ?
                    e.Manager.FirstName : null,
                    ManagerLastName = e.Manager != null ?
                    e.Manager.LastName : null,
                    Projects = e.EmployeesProjects
                    .Select(ep => ep.Project)
                    .Where(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003)
                    .Select(p => new
                    {
                        p.Name,
                        p.StartDate,
                        p.EndDate
                    })
                    .ToList(),
                })
                .Take(10)
                .ToList();

            foreach (var e in empolyeeInfo)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    string startDateFormated = p.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                    string endDateFormated = p.EndDate.HasValue ?
                        p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";
                    sb.AppendLine($"--{p.Name} - {startDateFormated} - {endDateFormated}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
