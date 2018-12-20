/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员角色                                                    
*│　作    者：suxiangnian                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2018-12-21 01:09:05                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.Cms.IRepository                                   
*│　接口名称： IManagerRoleRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Czar.Cms.Core.Repository;
using Czar.Cms.Models;
using System;

namespace Czar.Cms.IRepository
{
    public interface IManagerRoleRepository : IBaseRepository<ManagerRole, Int32>
    {
    }
}