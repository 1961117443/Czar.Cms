using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Czar.Cms.Admin.Validation;
using Czar.Cms.Core.Extensions;
using Czar.Cms.Core.Helper;
using Czar.Cms.IServices;
using Czar.Cms.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Czar.Cms.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly string CaptchaCodeSessionName = "CaptchaCode";
        private readonly string ManagerSignInErrorTimes = "ManagerSignInErrorTimes";
        private readonly int MaxErrorTimes = 3;
        private readonly IManagerService _service;

        public AccountController(IManagerService service)
        {
            this._service = service;
        }

        public IActionResult Index(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "";
            }
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["SessionID"] = HttpContext.Session.Id;
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken,Route("Account/SignIn")]
        public async Task<string> SignInAsync(LoginModel model)
        {
            BaseResult result = new BaseResult();

            //判断验证码
            if (!ValidateCaptchaCode(model.CaptchaCode))
            {
                result.ResultCode = ResultCodeAddMsgKeys.SignInCaptchaCodeErrorCode;
                result.ResultMsg = ResultCodeAddMsgKeys.SignInCaptchaCodeErrorMsg;
                return JsonHelper.ObjectToJSON(result); 
            }
            #region 判断错误次数
            var ErrorTimes = HttpContext.Session.GetInt32(ManagerSignInErrorTimes);
            if (ErrorTimes == null)
            {
                HttpContext.Session.SetInt32(ManagerSignInErrorTimes, 1);
                ErrorTimes = 1;
            }
            else
            {
                HttpContext.Session.SetInt32(ManagerSignInErrorTimes, ErrorTimes.Value + 1);
            }
            if (ErrorTimes > MaxErrorTimes)
            {
                result.ResultCode = ResultCodeAddMsgKeys.SignInErrorTimesOverTimesCode;
                result.ResultMsg = ResultCodeAddMsgKeys.SignInErrorTimesOverTimesMsg;
                return JsonHelper.ObjectToJSON(result);
            }
            #endregion
            #region 再次属性判断
            LoginModelValidation validation = new LoginModelValidation();
            ValidationResult results = validation.Validate(model);
            if (!results.IsValid)
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonModelStateInvalidCode;
                result.ResultMsg = results.ToString("||");
            }
            #endregion

            model.Ip = HttpContext.GetClientUserIp();
            var manager = _service.SignIn(model);
            if (manager==null)
            {
                result.ResultCode = ResultCodeAddMsgKeys.SignInPasswordOrUserNameErrorCode;
                result.ResultMsg = ResultCodeAddMsgKeys.SignInPasswordOrUserNameErrorMsg;
            }
            else if (manager.IsLock)
            {
                result.ResultCode = ResultCodeAddMsgKeys.SignInUserLockedCode;
                result.ResultMsg = ResultCodeAddMsgKeys.SignInUserLockedMsg;
            }
            else
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,manager.Email),
                    new Claim(ClaimTypes.MobilePhone,manager.Mobile),
                    new Claim(ClaimTypes.Role,manager.RoleId.ToString()),
                    new Claim("NickName",manager.NickName),
                    new Claim("LoginCount",manager.LoginCount.ToString()),
                    new Claim("LoginLastIp",manager.LoginLastIp),
                    new Claim("LoginLastTime",manager.LoginLastTime?.ToString("yyyy-MM-dd HH:mm:ss")),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }
            return JsonHelper.ObjectToJSON(result);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCaptchaImage()
        {
            var code = CaptchaHelper.GenerateCaptchaCode();
            var result = CaptchaHelper.GetImage(116, 36, code);
            HttpContext.Session.SetString(CaptchaCodeSessionName, code);
            HttpContext.Session.Set("abc", "123".ToBytes());
            var sm = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(sm, "image/png");
        }
        /// <summary>
        /// 校验验证码是否正确
        /// </summary>
        /// <param name="code">前端传过来的验证码</param>
        /// <returns></returns>
        public bool ValidateCaptchaCode(string code)
        {
            var realCode = HttpContext.Session.GetString(CaptchaCodeSessionName);
            HttpContext.Session.Remove(CaptchaCodeSessionName);
            return code.Equals(realCode, StringComparison.OrdinalIgnoreCase);
        }
    }
}