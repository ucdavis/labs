using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labs.Mvc.Models
{
    public class StudentXCardModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Id { get; set; }

        public string CardId { get; set; }
        
        public string nCardholderId { get; set; }

        public string Department { get; set; }
        
        public string strEncodedCardNumber { get; set; }
        
        public DateTime dtExpirationDate { get; set; }
        
        public bool nActive { get; set; }
        
        public string Access1 { get; set; }
        
        public string Access2 { get; set; }
        
        public string nFacilityCode { get; set; }

        public string strCardFormatName { get; set; }

        public string Program { get; set; }

        public string Email { get; set; }
    }
}
