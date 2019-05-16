using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labs.Mvc.Models
{
    public class ResultModel
    {
        public ResultModel() {
            Students = new List<StudentModel>();
            Cards = new List<CardModel>();
        }
        public IList<StudentModel> Students { get; set; }
        public IList<CardModel> Cards { get; set; }
    }
}
