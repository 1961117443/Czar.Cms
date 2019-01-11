using System;
using System.Collections.Generic;
using System.Text;

namespace Czar.Cms.ViewModels
{
    public static class BaseResultExtendsion
    {
        public static BaseResult CommonObjectSuccess(this BaseResult result)
        {
            result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
            result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
            return result;
        }
        public static BaseResult CommonException(this BaseResult result)
        {
            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
            result.ResultMsg = ResultCodeAddMsgKeys.CommonExceptionMsg;
            return result;
        }
        public static BaseResult CommonFailNoData(this BaseResult result)
        {
            result.ResultCode = ResultCodeAddMsgKeys.CommonFailNoDataCode;
            result.ResultMsg = ResultCodeAddMsgKeys.CommonFailNoDataMsg;
            return result;
        }

        public static BaseResult CommonModelStateInvalid(this BaseResult result)
        {
            result.ResultCode = ResultCodeAddMsgKeys.CommonModelStateInvalidCode;
            result.ResultMsg = ResultCodeAddMsgKeys.CommonModelStateInvalidMsg;
            return result;
        }

        


    }
}
