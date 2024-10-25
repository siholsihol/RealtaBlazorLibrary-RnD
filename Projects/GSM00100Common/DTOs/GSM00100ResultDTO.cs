using R_APICommonDTO;

namespace GSM00100Common.DTOs
{
    public class GSM00100ResultDTO<T> : R_APIResultBaseDTO
    {
        public T Data { get; set; }
    }

    public class GSM00100ResultDTO : R_APIResultBaseDTO
    {
    }
}
