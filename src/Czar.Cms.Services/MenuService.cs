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
*│　描    述：后台管理菜单                                                    
*│　作    者：suxiangnian                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-01-11 23:16:38                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.Cms.Services                                  
*│　类    名： MenuService                                    
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
    public class MenuService: IMenuService
    {
        private readonly IMenuRepository _repository;
        protected IMapper _mapper;

        public MenuService(IMenuRepository repository,IMapper mapper)
        {
            _repository = repository;
            this._mapper = mapper;
        }

        public BaseResult AddOrModify(MenuAddOrModifyModel item)
        {
            var result = new BaseResult();
            Menu model;
            if (item.Id==0)//新增
            {
                model = _mapper.Map<Menu>(item);
                model.AddManagerId = 1;
                model.IsDelete = false;
                model.AddTime = DateTime.Now;

                result = _repository.Insert(model) > 0 ? result.CommonObjectSuccess() : result.CommonException();
            }
            else
            {
                //编辑
                model = _repository.Get(item.Id);
                if (model!=null)
                {
                    _mapper.Map(item, model);
                    model.ModifyManagerId = 1;
                    model.ModifyTimes = DateTime.Now;
                    result = _repository.Update(model) > 0 ? result.CommonObjectSuccess() : result.CommonException();
                }
                else
                {
                    result = result.CommonFailNoData();
                } 
            }
            return result;
        }

        public BaseResult ChangeDisplayStatus(ChangeStatusModel item)
        {
            throw new NotImplementedException();
        }

        public BaseResult DeleteIds(int[] Ids)
        {
            var result = new BaseResult();
            if (Ids.Count()==0)
            {
                result = result.CommonModelStateInvalid();
            }
            else
            {
                var count = _repository.DeleteLogical(Ids);
                if (count>0)
                {
                    result = result.CommonObjectSuccess();
                }
                else
                {
                    result = result.CommonException();
                }
            }
            return result;
        }

        public List<Menu> GetChildListByParentId(int ParentId)
        {
            string condition = "where IsDelete=0";
            if (ParentId>0)
            {
                condition += $" and ParentId = {ParentId}";
            }
            return _repository.GetList(condition).ToList();
        }

        public BooleanResult IsExistsName(MenuAddOrModifyModel item)
        { 
            string conditon = "where Name=@Name and IsDelete=0";
            if (item.Id>0)
            {
                conditon += $" and Id<>{item.Id}";
            }
            var data =_repository.GetList(conditon, item).Count() > 0;
            return new BooleanResult
            {
                Data = data
            };
        }

        public TableDataModel LoadData(MenuRequestModel model)
        {
            string condition = "where IsDelete=0";
            if (!model.Key.IsNullOrEmpty())
            {
                condition += $" and DisplayName like '%{model.Key}%'";
            }
            var list = _repository.GetListPaged(model.Page, model.Limit, condition, "Id desc").ToList();
            return new TableDataModel()
            {
                count = _repository.RecordCount(condition),
                data = list
            };
        }
    }
}