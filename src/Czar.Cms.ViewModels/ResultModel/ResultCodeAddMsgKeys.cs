﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Czar.Cms.ViewModels
{
    public class ResultCodeAddMsgKeys
    {
        #region 通用 100
        /// <summary>
        /// 通用成功编码
        /// </summary>
        public const int CommonObjectSuccessCode = 0;
        /// <summary>
        /// 通用操作成功信息
        /// </summary>
        public const string CommonObjectSuccessMsg = "操作成功";
        /// <summary>
        /// 通用Form验证失败错误码
        /// </summary>
        public const int CommonModelStateInvalidCode = 101;
        /// <summary>
        /// 通用Form验证失败错误码
        /// </summary>
        public const string CommonModelStateInvalidMsg = "请求数据校验失败";
        /// <summary>
        /// 数据为空的编码
        /// </summary>
        public const int CommonFailNoDataCode = 102;
        /// <summary>
        /// 数据为空的信息
        /// </summary>
        public const string CommonFailNoDataMsg = "数据不存在";
        /// <summary>
        /// 数据状态发生变化的编码
        /// </summary>
        public const int CommonDataStatusChangeCode = 103;
        /// <summary>
        /// 数据状态发生变化的信息
        /// </summary>
        public const string CommonDataStatusChangeMsg = "数据状态已发生变化，请刷新后再进行操作";

        /// <summary>
        /// 通用失败，系统异常错误码
        /// </summary>
        public const int CommonExceptionCode = 106;
        /// <summary>
        /// 通用失败，系统异常信息
        /// </summary>
        public const string CommonExceptionMsg = "系统异常";
        #endregion
    }
}
