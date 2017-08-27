using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalFourm.Models
{
    public class Comments
    {
        public int Id { get; set; }
        [Required]
        public string CommentText { get; set; }

        public DateTime CommetedOn { get; set; }

        public DateTime LastUpdatedOn {get;set;}

        public int UserId { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users Usr { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Quiz { get; set; }

    }
}