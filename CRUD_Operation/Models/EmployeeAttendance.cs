using System;
using System.Collections.Generic;

namespace CRUD_Operation.Models
{
    public partial class EmployeeAttendance
    {
        public int SlId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsOffday { get; set; }
    }
}
