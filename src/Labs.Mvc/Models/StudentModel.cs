using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labs.Mvc.Models
{
    public class StudentModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Id { get; set; }

        public string LoginId { get; set; }

        //TODO add email, program type
    }
}
