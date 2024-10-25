using GSM00100Common.DTOs;
using GSM00100Common.Requests;
using R_CommonFrontBackAPI;
using System.Collections.Generic;

namespace GSM00100Common
{
    public interface IGSM00100 : R_IServiceCRUDBase<GSM00100DTO>
    {
        GSM00100ResultDTO<List<GetSMTPListDTO>> GetSMTPList();
        GSM00100ResultDTO<bool> CheckDelete(CheckDeleteRequest poParameter);
        GSM00100ResultDTO TestSendEmail(TestSendEmailDTO poParameter);
        GSM00100ResultDTO<GetSMTPCredentialDTO> GetSMTPCredential(GetSMTPCredentialRequest poParameter);
    }
}
