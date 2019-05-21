using System;

namespace Labs.Mvc.Models
{
    public class CardModel
    {
        public string CardsId { get; set; }
        
        public string nCardholderId { get; set; }
        
        public string strEmployeeId { get; set; }
        
        public string strFirstName { get; set; }
        
        public string strLastName { get; set; }
        
        public string Department { get; set; }
        
        public string strEncodedCardNumber { get; set; }
        
        public DateTime dtExpirationDate { get; set; }
        
        public bool nActive { get; set; }
        
        public string Access1 { get; set; }
        
        public string Access2 { get; set; }
        
        public string nFacilityCode { get; set; }

        public string strCardFormatName { get; set; }
    }
}