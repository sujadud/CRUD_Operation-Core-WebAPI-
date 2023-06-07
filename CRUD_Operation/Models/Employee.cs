using System;
using System.Collections.Generic;

namespace CRUD_Operation.Models
{
    public partial class Employee
    {
        public int SlId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public double EmployeeSalary { get; set; }
    }
}
