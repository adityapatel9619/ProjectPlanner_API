using System.ComponentModel.DataAnnotations;

namespace ProjectPlanner_API.Models
{
    public class NewEmployeeModel
    {
        [Required]
        public string sName { get; set; }
        [Required]
        public string sEmail { get; set; }
        [Required]
        [MaxLength(10,ErrorMessage = "Max Length is 10")]
        public string sPhone { get; set; }
        public string nDept { get; set; }
    }
}
