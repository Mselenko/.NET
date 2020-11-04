using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;

namespace Assignment1.Models
{
    public class EmployeeBaseViewModel : EmployeeAddViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
    }
}