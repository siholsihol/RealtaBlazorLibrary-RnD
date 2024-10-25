using System;

namespace GSM00100Common.DTOs
{
    public class GSM00100DTO
    {
        public string CSMTP_ID { get; set; }
        public string CSMTP_SERVER { get; set; }
        public string CSMTP_PORT { get; set; }
        public bool LSUPPORT_SSL { get; set; }
        public string CSMTP_CREDENTIAL_USER { get; set; }
        public string CSMTP_CREDENTIAL_PASSWORD { get; set; }
        public string CGENERAL_EMAIL_ADDRESS { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime? DUPDATE_DATE { get; set; }
        public string CCREATE_BY { get; set; }
        public DateTime? DCREATE_DATE { get; set; }

        public string CUSER_ID { get; set; }
        public string CDATENOW { get; set; }

        public bool LAPI { get; set; }

        public bool LEDIT_CREDENTIAL { get; set; }
    }
}
