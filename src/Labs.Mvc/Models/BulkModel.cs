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
            Results = new ResultModel();
        }
        public string SearchCourses { get; set; }
        public string SearchTerm { get; set; }

        public IList<TermModel> Terms { get; set; }

        public ResultModel Results { get; set; }
    }
}
