/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：操作日志接口实现                                                    
*│　作    者：suxiangnian                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2018-12-21 01:09:05                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.Cms.Repository.SqlServer                                  
*│　类    名： ManagerLogRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Czar.Cms.Core.DbHelper;
using Czar.Cms.Core.Options;
using Czar.Cms.Core.Repository;
using Czar.Cms.IRepository;
using Czar.Cms.Models;
using Microsoft.Extensions.Options;
using System;

namespace Czar.Cms.Repository.SqlServer
{
    public class ManagerLogRepository:BaseRepository<ManagerLog,Int32>, IManagerLogRepository
    {
        public ManagerLogRepository(IOptionsSnapshot<DbOpion> options)
        {
            _dbOpion =options.Get("CzarCms");
            if (_dbOpion == null)
            {
                throw new ArgumentNullException(nameof(DbOpion));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOpion.DbType, _dbOpion.ConnectionString);
        }

    }
}