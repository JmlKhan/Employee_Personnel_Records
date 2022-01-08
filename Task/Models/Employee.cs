using System.ComponentModel.DataAnnotations;

namespace Task.Models
{
    public class Employee
    {
        [Key]
        [DataType(DataType.Text)]
        [Required]
        public string Payroll_Number { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string Forenames { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string Surname { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date_of_Birth { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required]
        public int Telephone { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required]
        public int Mobile { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string Address { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string Address_2 { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string Postcode { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email_Home { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime Start_Date { get; set; }


    }
}
