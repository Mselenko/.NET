using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment1.Models
{
    public class EmployeeEditContactFormViewModel
    {
        [Key]
        public int EmployeeId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Title { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        [DataType(DataType.PostalCode)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string Fax { get; set; }

        [StringLength(60)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Range(1, 4, ErrorMessage = "Number of Weeks of Vacation must be between 1 and 4")]
        [Display(Name = "Number of Weeks of Vacation")]
        public int WeeksOfVacation { get; set; }
    }
}

