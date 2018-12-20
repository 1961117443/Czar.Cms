using Czar.Cms.Core.Extensions;
using Czar.Cms.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Czar.Cms.Core.DbHelper
{
    /// <summary>
    /// 数据库连接工厂类
    /// </summary>
    public class ConnectionFactory
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="conStr">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDbConnection CreateConnection(string dbtype,string strConn)
        {
            if (dbtype.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("dbtype");
            }
            if (strConn.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("strConn");
            }
            return CreateConnection(GetDataBaseType(dbtype), strConn);
        }
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="conStr">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDbConnection CreateConnection(DatabaseType databaseType,string strConn)
        {
            IDbConnection dbConnection = null;
            if (strConn.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("strConn");
            }
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    dbConnection = new SqlConnection(strConn);
                    break;
                case DatabaseType.MySQL:
                    break;
                case DatabaseType.PostgreSQL:
                    break;
                case DatabaseType.SQLite:
                    break;
                case DatabaseType.InMemory:
                    break;
                case DatabaseType.Oracle:
                    break;
                case DatabaseType.MariaDB:
                    break;
                case DatabaseType.MyCat:
                    break;
                case DatabaseType.Firebird:
                    break;
                case DatabaseType.DB2:
                    break;
                case DatabaseType.Access:
                    break;
                default:
                    break;
            }
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }

            return dbConnection;
        }
        /// <summary>
        /// 转换数据库类型
        /// </summary>
        /// <param name="dbtype">数据库类型字符串</param>
        /// <returns>数据库类型</returns>
        public static DatabaseType GetDataBaseType(string dbtype)
        {
            DatabaseType databaseType = DatabaseType.SqlServer;
            Enum.TryParse(dbtype, out databaseType);
            return databaseType;
        }
    }
}
