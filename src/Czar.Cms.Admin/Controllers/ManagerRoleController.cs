using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czar.Cms.Core.Helper;
using Czar.Cms.IServices;
using Czar.Cms.ViewModels; 
using Microsoft.AspNetCore.Mvc;

namespace Czar.Cms.Admin.Controllers
{
    public class ManagerRoleController : BaseController
    {
        private readonly IManagerRoleService _service;
        public ManagerRoleController(IManagerRoleService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public string LoadData([FromQuery]ManagerRoleRequestModel model)
        {
            return JsonHelper.ObjectToJSON(_service.LoadData(model));
        }
    }
}