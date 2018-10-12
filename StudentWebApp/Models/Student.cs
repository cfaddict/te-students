using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TechElevator.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public Track Track { get; set; }

        public Location Location { get; set; }
 
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public Student()
        {

        }

        public override string ToString()
        {
            return $"Student: {FullName}";
        }
    }
}
