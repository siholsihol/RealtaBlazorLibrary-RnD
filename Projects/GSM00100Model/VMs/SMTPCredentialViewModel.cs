using GSM00100Common.DTOs;
using GSM00100Common.Requests;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System;
using System.Threading.Tasks;

namespace GSM00100Model.VMs
{
    public class SMTPCredentialViewModel
    {
        public GetSMTPCredentialDTO Credential { get; set; } = new GetSMTPCredentialDTO();
        private readonly GSM00100Model _gsm00100Model = new GSM00100Model();

        public async Task GetSMTPCredential(string pcSMTPId)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new GetSMTPCredentialRequest() { CSMTP_ID = pcSMTPId };
                var loResult = await _gsm00100Model.GetSMTPCredentialAsync(loParam);

                Credential = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void ValidationCredential(GetSMTPCredentialDTO poCredential)
        {
            var loEx = new R_Exception();

            try
            {
                if (string.IsNullOrWhiteSpace(poCredential.CSMTP_CREDENTIAL_USER))
                    loEx.Add(GetErrorFromResource("_err004"));

                if (string.IsNullOrWhiteSpace(poCredential.CSMTP_CREDENTIAL_PASSWORD))
                    loEx.Add(GetErrorFromResource("_err006"));
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private R_Error GetErrorFromResource(string pcMessageId)
        {
            return R_FrontUtility.R_GetError(typeof(GSM00100FrontResources.Resources_Dummy_Class), pcMessageId);
        }
    }
}
