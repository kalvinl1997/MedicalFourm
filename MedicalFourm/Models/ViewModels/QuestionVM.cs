using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalFourm.Models.ViewModels
{
    public class QuestionVM
    {
        public Question Question { get; set; }
        public IEnumerable<Comments> Comments { get; set; }
        public string username { get; set; }
        public int likes { get; set; }

        public int dislikes { get; set; }

        public bool isuserlike { get; set; }

        public bool isuserdislike { get; set; }
    }
}