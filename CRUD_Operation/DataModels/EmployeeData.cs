using System.ComponentModel.DataAnnotations;

namespace CRUD_Operation.ViewModels
{
    public class EmployeeData
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public double EmployeeSalary { get; set; }
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsOffday { get; set; }

        internal object OrderByDescending(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
