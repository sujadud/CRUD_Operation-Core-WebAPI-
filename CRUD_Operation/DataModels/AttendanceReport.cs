namespace CRUD_Operation.DataModels
{
    public class AttendanceReport
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string MonthName { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalOffday { get; set; }

    }
}
