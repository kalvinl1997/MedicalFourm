using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalFourm.Models
{
    public class Choice
    { 
        public int Id { get; set; }
        public bool? IsLiked { get; set; }
        public bool? IsDisliked { get; set; }

        public int UserId { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users Usr { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Quiz { get; set; }



    }
}