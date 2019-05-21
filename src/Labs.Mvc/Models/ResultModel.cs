using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labs.Mvc.Models
{
    public class ResultModel
    {
        public ResultModel() {
            StudentsWithoutCards = new List<StudentXCardModel>();
            StudentsWithCards = new List<StudentXCardModel>();
        }
        public IList<StudentXCardModel> StudentsWithoutCards { get; set; }
        public IList<StudentXCardModel> StudentsWithCards { get; set; }
    }
}
