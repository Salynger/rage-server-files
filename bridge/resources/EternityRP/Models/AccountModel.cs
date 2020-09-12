using System;

namespace EternityRP.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string SocialName { get; set; }
        public string Email { get; set; }
        public Int16 Status { get; set; }
        public bool Status_Mail { get; set; }
        public bool Bay_Pers { get; set; }
        public AccountModel() { }

        public AccountModel(int id, string socialName, string eMail, bool status_mail, Int16 status, bool bay_pers)
        {
            this.Id = id;
            this.SocialName = socialName;
            this.Email = eMail;
            this.Status = status;
            this.Status_Mail = status_mail;
            this.Bay_Pers = bay_pers;
        }
    }
}
