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
*│　描    述：后台管理员角色                                                    
*│　作    者：suxiangnian                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-01-11 23:16:38                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.Cms.Services                                  
*│　类    名： ManagerRoleService                                    
*└──────────────────────────────────────────────────────────────┘
*/
using AutoMapper;
using Czar.Cms.Core.Extensions;
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
    public class ManagerRoleService: IManagerRoleService
    {
        private readonly IManagerRoleRepository _repository;
        private readonly IMapper _mapper;

        public ManagerRoleService(IManagerRoleRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public BaseResult AddOrModify(ManagerRoleAddOrModifyModel item)
        {
            var result = new BaseResult();
            ManagerRole managerRole;
            //add
            if (item.Id==0)
            {
                managerRole = _mapper.Map<ManagerRole>(item);
                managerRole.AddManagerId = 1;
                managerRole.IsDelete = false;
                managerRole.AddTime = DateTime.Now;
                if (_repository.Insert(managerRole)>0)
                {
                    result.CommonObjectSuccess(); 
                }
                else
                {
                    result.CommonException();
                }
            }
            else
            {
                //modify
                managerRole = _repository.Get(item.Id);
                if (managerRole!=null)
                {
                    _mapper.Map(item, managerRole);
                    managerRole.ModifyManagerId = 1;
                    managerRole.ModifyTime = DateTime.Now;
                    if (_repository.Update(managerRole)>0)
                    {
                        result.CommonObjectSuccess(); 
                    }
                    else
                    {
                        result.CommonException();
                    }
                }
                else
                {
                    result.CommonFailNoData(); 
                }
            }
            return result;
        }

        public BaseResult DeleteIds(int[] roleId)
        {
            var result = new BaseResult();
            if (roleId.Count()==0)
            {
                result.CommonModelStateInvalid();
            }
            else
            {
                var count = _repository.DeleteLogical(roleId);
                if (count>0)
                {
                    result.CommonObjectSuccess();
                }
                else
                {
                    result.CommonException();
                }
            }
            return result;
        }

        public List<ManagerRole> GetListByCondition(ManagerRoleRequestModel model)
        {
            string conditions = "where IsDelete=0";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and RoleName like '{model.Key}'";
            }
            return _repository.GetList(conditions).ToList();
        }

        public TableDataModel LoadData(ManagerRoleRequestModel model)
        {
            string conditions = "where IsDelete=0 ";//未删除的
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and RoleName like '%{model.Key}%'";
            }
            return new TableDataModel
            {
                count = _repository.RecordCount(conditions),
                data = _repository.GetListPaged(model.Page, model.Limit, conditions, "Id desc"),
            };
        }
    }
}