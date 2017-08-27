using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalFourm.Models
{
    public class Question
    {
        public Question()
        {
          Comments = new List<Comments>();
            Choices = new List<Choice>();
        }
        public int Id { get; set; }
        [Required]
        public string Qstatment { get; set; }

        [Required]
        public string Qdescription { get; set; }

        [Required]
        public string Qcatagory { get; set; }

        public DateTime PublishedOn { get; set; }

        public DateTime LastUpdated { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users Usr { get; set; }

      public virtual ICollection<Comments> Comments { get; set; }

        public virtual ICollection<Choice> Choices { get; set; }
    }

}