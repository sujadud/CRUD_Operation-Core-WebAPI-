using CRUD_Operation.DataModels;
using CRUD_Operation.Models;
using CRUD_Operation.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Runtime.Intrinsics.Arm;

namespace CRUD_Operation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly CrudDbContext dbContext;

        public EmployeeController(CrudDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        // GET: api/employees/report
        [HttpGet("attendanceReport")]
        public async Task<IEnumerable<AttendanceReport>> EmployeeAttendanceReport()
        {
            List<AttendanceReport> ar = new List<AttendanceReport>();
            var e = dbContext.Employees.ToList();
            var att = dbContext.EmployeeAttendances.ToList();
            var month = new List<string>();

            foreach (var f in att)
            {
                var ToMonth = (DateTime)f.AttendanceDate;
                string monthName = ToMonth.ToString("MMMM");
                bool duplicateFoune = Copy(month, monthName);
                if (!duplicateFoune)
                    month.Add(monthName);
            }

            foreach (var m in month)
            {
                foreach (var employee in e)
                {
                    AttendanceReport a = new AttendanceReport();
                    a.EmployeeId = employee.EmployeeId;
                    a.EmployeeName = employee.EmployeeName;
                    a.MonthName = m; ar.Add(a);
                }
            }

            for (int i = 0; i < ar.Count; i++)
            {
                foreach (var attendance in att)
                {
                    if (((DateTime)attendance.AttendanceDate).ToString("MMMM") == ar[i].MonthName && attendance.EmployeeId == ar[i].EmployeeId)
                    {
                        ar[i].TotalPresent = ((bool)attendance.IsPresent) ? ar[i].TotalPresent + 1 : ar[i].TotalPresent += 0;
                        ar[i].TotalOffday = ((bool)attendance.IsOffday) ? ar[i].TotalOffday + 1 : ar[i].TotalOffday += 0;
                        ar[i].TotalAbsent = ((bool)attendance.IsAbsent && !((bool)attendance.IsOffday)) ? ar[i].TotalAbsent + 1 : ar[i].TotalAbsent += 0;
                    }
                }
            }

            return ar;
        }



        // GET: api/employees/bySalary
        [HttpGet("bySalary")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesBySalary()
        {
            //API02# Get all employee based on maximum to minimum salary

            var employees = await dbContext.Employees.OrderByDescending(e => e.EmployeeSalary).ToListAsync();
            return Ok(employees);
        }

        [HttpGet("absent")]
        public IActionResult GetEmployeesWithAbsentDays()
        {
            //API03# Find all employee who is absent at least one day
            var employees = dbContext.Employees
                .Where(e => dbContext.EmployeeAttendances.Any(a => a.EmployeeId == e.EmployeeId && a.IsAbsent))
                .ToList();

            return Ok(employees);
        }



        //GET: api/employees/{employeeId
        [HttpGet("{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int employeeId)
        {
            var employee = await dbContext.Employees.FindAsync(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployee(EmployeeData employeeData)
        {
            var employee = new Employee
            {
                EmployeeId = employeeData.EmployeeId,
                EmployeeName = employeeData.EmployeeName,
                EmployeeCode = employeeData.EmployeeCode,
                EmployeeSalary = employeeData.EmployeeSalary
            };

            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();

            var attendance = new EmployeeAttendance
            {
                EmployeeId = employeeData.EmployeeId,
                AttendanceDate = employeeData.AttendanceDate,
                IsPresent = employeeData.IsPresent,
                IsAbsent = employeeData.IsAbsent,
                IsOffday = employeeData.IsOffday
            };

            dbContext.EmployeeAttendances.Add(attendance);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetEmployeeById), new { employeeId = employee.EmployeeId }, employee);
        }


        // PUT: api/employees/{employeeId}
        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, EmployeeData empData)
        {
            var existingEmployee = dbContext.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            if (employeeId != empData.EmployeeId)
            {
                return BadRequest();
            }
            //API01# Update an employee’s Employee Code [Don't allow duplicate employee code]
            dbContext.Entry(empData).State = EntityState.Modified;

            var duplicateEmployee = dbContext.Employees.FirstOrDefault(e => e.EmployeeCode == empData.EmployeeCode);
            if (duplicateEmployee != null)
            {
                return Conflict("Employee code already exists.");
            }
            existingEmployee.EmployeeCode = empData.EmployeeCode;


            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employeeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/employees/{employeeId}
        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployee(int employeeId)
        {
            var employee = dbContext.Employees.Find(employeeId);

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                dbContext.Employees.Remove(employee);
            }
            dbContext.SaveChanges();

            return NoContent();
        }

        private bool EmployeeExists(int employeeId)
        {
            return dbContext.Employees.Any(e => e.EmployeeId == employeeId);
        }

        public static bool Copy(List<string> month, string monthName)
        {

            foreach (var x in month) if (x == monthName) return true;

            return false;
        }

    }
}
