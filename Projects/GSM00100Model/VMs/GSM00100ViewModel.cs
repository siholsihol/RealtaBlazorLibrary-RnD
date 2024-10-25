using GSM00100Common.DTOs;
using GSM00100Common.Requests;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GSM00100Model.VMs
{
    public class GSM00100ViewModel : R_ViewModel<GSM00100DTO>
    {
        private readonly GSM00100Model _gsm00100Model = new GSM00100Model();

        public ObservableCollection<GetSMTPListDTO> SMTPList { get; set; } = new ObservableCollection<GetSMTPListDTO>();
        public GSM00100DTO CurrentSMTP { get; set; } = new GSM00100DTO();
        public GetSMTPCredentialDTO Credential { get; set; }
        public string SALT_KEY_CREDENTIAL { get; } = "R34LT4";
        public bool EditCredential { get; set; }
        public bool GridEnabled { get; set; } = true;

        public async Task GetSMTPListAsync(string pcSmtpId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _gsm00100Model.GetSMTPListAsync();

                var loData = loResult.Data;
                if (!string.IsNullOrWhiteSpace(pcSmtpId))
                    loData = loData.Where(x => x.CSMTP_ID == pcSmtpId).ToList();

                SMTPList = new ObservableCollection<GetSMTPListDTO>(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetSMTPAsync(string pcSmtpId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _gsm00100Model.R_ServiceGetRecordAsync(new GSM00100DTO { CSMTP_ID = pcSmtpId });

                CurrentSMTP = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void ValidationSaveSMTP(GSM00100DTO poEntity, bool plIsAddMode)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = poEntity;

                if (string.IsNullOrWhiteSpace(loData.CSMTP_ID))
                    loEx.Add(GetErrorFromResource("_err001"));

                if (string.IsNullOrWhiteSpace(loData.CSMTP_SERVER))
                    loEx.Add(GetErrorFromResource("_err002"));

                if (string.IsNullOrWhiteSpace(loData.CSMTP_PORT))
                    loEx.Add(GetErrorFromResource("_err003"));

                if (string.IsNullOrWhiteSpace(loData.CGENERAL_EMAIL_ADDRESS))
                {
                    loEx.Add(GetErrorFromResource("_err007"));
                }
                else
                {
                    if (!IsEmail(loData.CGENERAL_EMAIL_ADDRESS))
                        loEx.Add(GetErrorFromResource("_err008"));
                }

                if (Credential == null && plIsAddMode)
                    loEx.Add(GetErrorFromResource("_err013"));

                if (Credential != null)
                {
                    if (string.IsNullOrWhiteSpace(Credential.CSMTP_CREDENTIAL_USER))
                        loEx.Add(GetErrorFromResource("_err004"));

                    if (string.IsNullOrWhiteSpace(Credential.CSMTP_CREDENTIAL_PASSWORD))
                        loEx.Add(GetErrorFromResource("_err006"));
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveSMTPAsync(GSM00100DTO poNewEntity, R_eConductorMode conductorMode)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _gsm00100Model.R_ServiceSaveAsync(poNewEntity, (eCRUDMode)conductorMode);

                CurrentSMTP = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task<bool> BeforeDeleteSMTPAsync(string pcSMTPId)
        {
            var loEx = new R_Exception();

            try
            {
                var loRequest = new CheckDeleteRequest { CSMTP_ID = pcSMTPId };
                var loResult = await _gsm00100Model.CheckDeleteAsync(loRequest);

                return loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return false;
        }

        public async Task DeleteSMTPAsync(string pcSmtpId)
        {
            var loEx = new R_Exception();

            try
            {
                await _gsm00100Model.R_ServiceDeleteAsync(new GSM00100DTO { CSMTP_ID = pcSmtpId });
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

        private bool IsEmail(string pcEmail)
        {
            return R_FrontUtility.IsValidEmail(pcEmail);
        }

        public async Task TestSendEmailAsync(string pcCompanyId, string pcUserId, string pcEmailTo)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new TestSendEmailDTO
                {
                    CSMTP_ID = CurrentSMTP.CSMTP_ID,
                    CCOMPANY_ID = pcCompanyId,
                    CUSER_ID = pcUserId,
                    CEMAIL_TO = pcEmailTo
                };

                await _gsm00100Model.TestSendEmailAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task<bool> LockUnlockAsync(GSM00100DTO poData, string pcCompanyId, string pcUserId, bool plLock)
        {
            var loEx = new R_Exception();

            try
            {
                return await _gsm00100Model.LockUnlockAsync(poData, pcCompanyId, pcUserId, plLock);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return false;
        }
    }
}
