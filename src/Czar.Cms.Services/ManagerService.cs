////////////////////////////////////////////////////////////////////
//                          _ooOoo_                               //
//                         o8888888o                              //
//                         88" . "88                              //
//                         (| ^_^ |)                              //
//                         O\  =  /O                              //
//                      ____/`---'\____                           //
//                    .'  \\|     |//  `.                         //
//                   /  \\|||  :  |||//  \                        //
//                  /  _||||| -:- |||||-  \                       //
//                  |   | \\\  -  /// |   |                       //
//                  | \_|  ''\---/''  |   |                       //
//                  \  .-\__  `-`  ___/-. /                       //
//                ___`. .'  /--.--\  `. . ___                     //
//              ."" '<  `.___\_<|>_/___.'  >'"".                  //
//            | | :  `- \`.;`\ _ /`;.`/ - ` : | |                 //
//            \  \ `-.   \_ __\ /__ _/   .-` /  /                 //
//      ========`-.____`-.___\_____/___.-`____.-'========         //
//                           `=---='                              //
//      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^        //
//                   佛祖保佑       永不宕机     永无BUG          //
////////////////////////////////////////////////////////////////////

/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员                                                    
*│　作    者：suxiangnian                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-01-11 23:16:38                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.Cms.Services                                  
*│　类    名： ManagerService                                    
*└──────────────────────────────────────────────────────────────┘
*/
using Czar.Cms.Core.Helper;
using Czar.Cms.IRepository;
using Czar.Cms.IServices;
using Czar.Cms.Models;
using Czar.Cms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Czar.Cms.Services
{
    public class ManagerService: IManagerService
    {
        private readonly IManagerRepository _repository;

        public ManagerService(IManagerRepository repository)
        {
            _repository = repository;
        }

        public BaseResult AddOrModify(ManagerAddOrModifyModel model)
        {
            throw new NotImplementedException();
        }

        public BaseResult ChangeLockStatus(ChangeStatusModel model)
        {
            throw new NotImplementedException();
        }

        public BaseResult DeleteIds(int[] Ids)
        {
            throw new NotImplementedException();
        }

        public TableDataModel LoadData(ManagerRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Manager SignIn(LoginModel model)
        {
            model.Password = AESEncryptHelper.Encode(model.Password.Trim(), CzarCmsKeys.AesEncryptKeys);
            model.UserName = model.UserName.Trim();
            string conditions = "where IsDelete=0 ";//未删除的
            conditions += $"and (UserName = @UserName or Mobile =@UserName or Email =@UserName) and Password=@Password";
            var manager = _repository.GetList(conditions, model).FirstOrDefault();
            if (manager != null)
            {
                manager.LoginLastIp = model.Ip;
                manager.LoginCount += 1;
                manager.LoginLastTime = DateTime.Now;
                _repository.Update(manager);
                //写日志
                //_managerLogRepository.Insert(new ManagerLog()
                //{
                //    ActionType = CzarCmsEnums.ActionEnum.SignIn.ToString(),
                //    AddManageId = manager.Id,
                //    AddManagerNickName = manager.NickName,
                //    AddTime = DateTime.Now,
                //    AddIp = model.Ip,
                //    Remark = "用户登录"
                //});
            }
            return manager;
        }
    }
}