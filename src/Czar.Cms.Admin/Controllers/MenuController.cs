using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czar.Cms.Admin.Validation;
using Czar.Cms.Core.Helper;
using Czar.Cms.IServices;
using Czar.Cms.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Czar.Cms.Admin.Controllers
{
    public class MenuController : Controller
    {
        protected IMenuService Service;
        public MenuController(IMenuService service)
        {
            Service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string LoadData([FromQuery]MenuRequestModel model)
        {
            return JsonHelper.ObjectToJSON(Service.LoadData(model));
        }
        [HttpGet]
        public IActionResult AddOrModify()
        {
            var list = Service.GetChildListByParentId(0);
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string AddOrModify([FromForm]MenuAddOrModifyModel model)
        {
            var result = new BaseResult();
            var validation = new MenuValidation().Validate(model);
            if (validation.IsValid)
            {
                result = Service.AddOrModify(model);
            }
            else
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonModelStateInvalidCode;
                result.ResultMsg = validation.ToString("||");
            }

            return JsonHelper.ObjectToJSON(result);
        }

        [HttpGet]
        public string IsExistsName([FromQuery]MenuAddOrModifyModel model)
        {
            var result = Service.IsExistsName(model);
            return JsonHelper.ObjectToJSON(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Delete(int[] menuId)
        {
            return JsonHelper.ObjectToJSON(Service.DeleteIds(menuId));
        }
    }
}