using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labs.Mvc.Models
{
    public class BulkModel
    {
        public BulkModel()
        {
            Terms = new List<TermModel>();
            StudentsWithoutCards = new List<StudentXCardModel>();
            StudentsWithCards = new List<StudentXCardModel>();
        }
        public string SearchCourses { get; set; }
        public string SearchTerm { get; set; }

        public IList<TermModel> Terms { get; set; }

        public IList<StudentXCardModel> StudentsWithoutCards { get; set; }
        public IList<StudentXCardModel> StudentsWithCards { get; set; }
        public IList<StudentXCardModel> StudentsWithProblems { get; set; }
    }
}
