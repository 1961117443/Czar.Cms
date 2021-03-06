/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理菜单接口实现                                                    
*│　作    者：suxiangnian                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2018-12-21 01:09:05                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.Cms.Repository.SqlServer                                  
*│　类    名： MenuRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Czar.Cms.Core.DbHelper;
using Czar.Cms.Core.Options;
using Czar.Cms.Core.Repository;
using Czar.Cms.IRepository;
using Czar.Cms.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Czar.Cms.Repository.SqlServer
{
    public class MenuRepository:BaseRepository<Menu,Int32>, IMenuRepository
    {
        public MenuRepository(IOptionsSnapshot<DbOpion> options)
        {
            _dbOpion =options.Get("CzarCms");
            if (_dbOpion == null)
            {
                throw new ArgumentNullException(nameof(DbOpion));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOpion.DbType, _dbOpion.ConnectionString);
        }

        public int DeleteLogical(int[] ids)
        {
            string sql = "update [Menu] set IsDelete=1 where Id in @Ids";
            return _dbConnection.Execute(sql, new { Ids = ids });
        }

        public async Task<int> DeleteLogicalAsync(int[] ids)
        { 
            string sql = "update [Menu] set IsDelete=1 where Id in @Ids";
            return await _dbConnection.ExecuteAsync(sql, new { Ids = ids });
        }
    }
}