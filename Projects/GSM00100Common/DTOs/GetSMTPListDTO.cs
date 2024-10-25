namespace GSM00100Common.DTOs
{
    public class GetSMTPListDTO
    {
        public string CSMTP_ID { get; set; }
        public string CSMTP_SERVER { get; set; }
        public string CSMTP_PORT { get; set; }
        public bool LSUPPORT_SSL { get; set; }
        public string CGENERAL_EMAIL_ADDRESS { get; set; }
    }
}
