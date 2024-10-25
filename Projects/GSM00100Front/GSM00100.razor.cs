using BlazorClientHelper;
using GSM00100Common.DTOs;
using GSM00100Model.VMs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Notification;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CrossPlatformSecurity;

namespace GSM00100Front
{
    public partial class GSM00100 : R_Page
    {
        private readonly GSM00100ViewModel _gsm00100VM = new();
        private R_Conductor _conductorRef;
        private R_Grid<GetSMTPListDTO> _gridRef;
        private R_TextBox _textBoxSMTPId;

        [Inject] private IClientHelper _globalVar { get; set; }
        [Inject] private R_NotificationService _notificationService { get; set; }
        [Inject] private R_ISymmetricJSProvider _encryptProvider { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridRef.R_RefreshGrid(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _gsm00100VM.GetSMTPListAsync((string)eventArgs.Parameter);

                eventArgs.ListEntityResult = _gsm00100VM.SMTPList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Conductor
        #region ADD
        private async Task Conductor_R_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loData = (GSM00100DTO)eventArgs.Data;

            loData.LSUPPORT_SSL = true;
            loData.CCREATE_BY = _globalVar.UserId;
            loData.CUPDATE_BY = _globalVar.UserId;
            loData.DCREATE_DATE = DateTime.Now;
            loData.DUPDATE_DATE = DateTime.Now;

            await _textBoxSMTPId.FocusAsync();
        }
        #endregion

        #region Delete
        private async Task Conductor_R_BeforeDelete(R_BeforeDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var lcSMTPId = ((GSM00100DTO)eventArgs.Data).CSMTP_ID;
                var llResult = await _gsm00100VM.BeforeDeleteSMTPAsync(lcSMTPId);
                eventArgs.Cancel = llResult;

                if (llResult)
                    _notificationService.Error($"{lcSMTPId} is used.");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Conductor_R_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (GSM00100DTO)eventArgs.Data;

                await _gsm00100VM.DeleteSMTPAsync(loData.CSMTP_ID);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Conductor_R_AfterDelete()
        {
            _notificationService.Success("Success delete SMTP.");
        }
        #endregion

        private void Conductor_R_SetOther(R_SetEventArgs eventArgs)
        {
            _gsm00100VM.GridEnabled = eventArgs.Enable;
        }

        private async Task Conductor_R_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<GSM00100DTO>(eventArgs.Data);

                await _gsm00100VM.GetSMTPAsync(loParam.CSMTP_ID);
                eventArgs.Result = _gsm00100VM.CurrentSMTP;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region SAVE
        private void Conductor_R_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _gsm00100VM.ValidationSaveSMTP((GSM00100DTO)eventArgs.Data, eventArgs.ConductorMode == R_eConductorMode.Add);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Conductor_R_Saving(R_SavingEventArgs eventArgs)
        {
            var loParam = (GSM00100DTO)eventArgs.Data;

            loParam.CUSER_ID = _globalVar.UserId;
            loParam.CDATENOW = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (eventArgs.ConductorMode == R_eConductorMode.Add)
            {
                loParam.CSMTP_CREDENTIAL_USER = _gsm00100VM.Credential.CSMTP_CREDENTIAL_USER;
                loParam.CSMTP_CREDENTIAL_PASSWORD = await _encryptProvider.TextEncrypt(_gsm00100VM.Credential.CSMTP_CREDENTIAL_PASSWORD, _gsm00100VM.SALT_KEY_CREDENTIAL);
            }

            if (eventArgs.ConductorMode == R_eConductorMode.Edit && _gsm00100VM.EditCredential)
            {
                loParam.LEDIT_CREDENTIAL = true;
                loParam.CSMTP_CREDENTIAL_USER = _gsm00100VM.Credential.CSMTP_CREDENTIAL_USER;
                loParam.CSMTP_CREDENTIAL_PASSWORD = await _encryptProvider.TextEncrypt(_gsm00100VM.Credential.CSMTP_CREDENTIAL_PASSWORD, _gsm00100VM.SALT_KEY_CREDENTIAL);
            }
        }

        private async Task Conductor_R_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _gsm00100VM.SaveSMTPAsync((GSM00100DTO)eventArgs.Data, eventArgs.ConductorMode);

                eventArgs.Result = _gsm00100VM.CurrentSMTP;

                if (eventArgs.ConductorMode == R_eConductorMode.Edit && _gsm00100VM.EditCredential)
                {
                    _gsm00100VM.EditCredential = false;
                    _gsm00100VM.Credential = null;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion
        #endregion

        #region POPUP
        private void R_Before_Open_Popup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(SMTPCredential);
            eventArgs.Parameter = _conductorRef.R_ConductorMode == R_eConductorMode.Add ? null : _gsm00100VM.CurrentSMTP;
        }

        private void R_After_Open_Popup(R_AfterOpenPopupEventArgs eventArgs)
        {
            if (eventArgs.Success)
            {
                if (_conductorRef.R_ConductorMode == R_eConductorMode.Edit)
                    _gsm00100VM.EditCredential = true;

                _gsm00100VM.Credential = (GetSMTPCredentialDTO)eventArgs.Result;
            }
        }

        private static void R_Before_Open_Popup_Test_Send_Email(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(InputEmail);
        }

        private async Task R_After_Open_Popup_Test_Send_Email(R_AfterOpenPopupEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                if (eventArgs.Success)
                {
                    await _gsm00100VM.TestSendEmailAsync(_globalVar.CompanyId, _globalVar.UserId, (string)eventArgs.Result);

                    _notificationService.Success("Success send email.");
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        protected override async Task<bool> R_LockUnlock(R_LockUnlockEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = eventArgs.Data as GSM00100DTO;
                var llLock = eventArgs.Mode == R_eLockUnlock.Lock ? true : false;

                return await _gsm00100VM.LockUnlockAsync(loData, _globalVar.CompanyId, _globalVar.UserId, llLock);
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
