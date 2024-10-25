using GSM00100Common.DTOs;
using GSM00100Model.VMs;
using R_BlazorFrontEnd.Exceptions;

namespace GSM00100Front
{
    public partial class SMTPCredential
    {
        private SMTPCredentialViewModel _credentialViewModel = new SMTPCredentialViewModel();

        protected override async Task R_Init_From_Master(object poParameter)
        {
            if (poParameter != null)
            {
                var loParam = (GSM00100DTO)poParameter;

                await _credentialViewModel.GetSMTPCredential(loParam.CSMTP_ID);
            }
        }

        private async Task CloseOnClick(bool plIsOK)
        {
            var loEx = new R_Exception();

            try
            {
                if (plIsOK)
                    _credentialViewModel.ValidationCredential(_credentialViewModel.Credential);

                await this.Close(plIsOK, _credentialViewModel.Credential);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
