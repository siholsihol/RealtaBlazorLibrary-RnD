using GSM00100Common;
using GSM00100Common.DTOs;
using GSM00100Common.Requests;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;
using R_LockingFront;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSM00100Model
{
    internal class GSM00100Model : R_BusinessObjectServiceClientBase<GSM00100DTO>, IGSM00100
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM00100";
        private const string DEFAULT_MODULE = "GS";

        public GSM00100Model()
            : base(DEFAULT_HTTP_NAME, DEFAULT_SERVICEPOINT_NAME, DEFAULT_MODULE, true, true)
        {
        }

        #region GetSMTPList
        public GSM00100ResultDTO<List<GetSMTPListDTO>> GetSMTPList()
        {
            throw new System.NotImplementedException();
        }

        public async Task<GSM00100ResultDTO<List<GetSMTPListDTO>>> GetSMTPListAsync()
        {
            var loEx = new R_Exception();
            GSM00100ResultDTO<List<GetSMTPListDTO>> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM00100ResultDTO<List<GetSMTPListDTO>>>(
                    _RequestServiceEndPoint,
                    nameof(IGSM00100.GetSMTPList),
                    _ModuleName,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

        #region CheckDelete
        public GSM00100ResultDTO<bool> CheckDelete(CheckDeleteRequest poParameter)
        {
            throw new System.NotImplementedException();
        }

        public async Task<GSM00100ResultDTO<bool>> CheckDeleteAsync(CheckDeleteRequest poParameter)
        {
            var loEx = new R_Exception();
            GSM00100ResultDTO<bool> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM00100ResultDTO<bool>, CheckDeleteRequest>(
                    _RequestServiceEndPoint,
                    nameof(IGSM00100.CheckDelete),
                    poParameter,
                    _ModuleName,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

        #region TestSendEmail
        public GSM00100ResultDTO TestSendEmail(TestSendEmailDTO poParameter)
        {
            throw new NotImplementedException();
        }

        public async Task<GSM00100ResultDTO> TestSendEmailAsync(TestSendEmailDTO poParameter)
        {
            var loEx = new R_Exception();
            GSM00100ResultDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM00100ResultDTO, TestSendEmailDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM00100.TestSendEmail),
                    poParameter,
                    _ModuleName,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

        #region GetSMTPCredential
        public GSM00100ResultDTO<GetSMTPCredentialDTO> GetSMTPCredential(GetSMTPCredentialRequest poParameter)
        {
            throw new NotImplementedException();
        }

        public async Task<GSM00100ResultDTO<GetSMTPCredentialDTO>> GetSMTPCredentialAsync(GetSMTPCredentialRequest poParameter)
        {
            var loEx = new R_Exception();
            GSM00100ResultDTO<GetSMTPCredentialDTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM00100ResultDTO<GetSMTPCredentialDTO>, GetSMTPCredentialRequest>(
                    _RequestServiceEndPoint,
                    nameof(IGSM00100.GetSMTPCredential),
                    poParameter,
                    _ModuleName,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

        #region Locking
        public async Task<bool> LockUnlockAsync(GSM00100DTO poData, string pcCompanyId, string pcUserId, bool plLock)
        {
            var loEx = new R_Exception();
            R_LockingFrontResult loLockResult = null;
            var llResult = false;

            try
            {
                var loClient = new R_LockingServiceClient(pcModuleName: _ModuleName,
                    plSendWithContext: _SendWithContext,
                    plSendWithToken: _SendWithToken,
                    pcHttpClientName: _HttpClientName);

                if (plLock)
                {
                    var loLockPar = new R_ServiceLockingLockParameterDTO
                    {
                        Company_Id = pcCompanyId,
                        User_Id = pcUserId,
                        Program_Id = "GSM00100",
                        Table_Name = "GSM_SMTP_CONFIG",
                        Key_Value = string.Join("|", pcCompanyId, poData.CSMTP_ID)
                    };

                    loLockResult = await loClient.R_Lock(loLockPar);
                }
                else
                {
                    var loUnlockPar = new R_ServiceLockingUnLockParameterDTO
                    {
                        Company_Id = pcCompanyId,
                        User_Id = pcUserId,
                        Program_Id = "GSM00100",
                        Table_Name = "GSM_SMTP_CONFIG",
                        Key_Value = string.Join("|", pcCompanyId, poData.CSMTP_ID)
                    };
                    loLockResult = await loClient.R_UnLock(loUnlockPar);
                }

                llResult = loLockResult.IsSuccess;
                if (!loLockResult.IsSuccess && loLockResult.Exception != null)
                    throw loLockResult.Exception;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return llResult;
        }
        #endregion
    }
}
