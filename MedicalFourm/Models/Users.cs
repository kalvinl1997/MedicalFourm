using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalFourm.Models
{
    public class Users
    {
        public Users()
        {
            Questions = new List<Question>();
           Comments = new List<Comments>();
            Choices = new List<Choice>();
        }
        [Key]
        public int Id { get; set; } 
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }
    }
}