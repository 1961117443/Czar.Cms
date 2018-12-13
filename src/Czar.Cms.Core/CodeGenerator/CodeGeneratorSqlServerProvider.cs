using Czar.Cms.Core.Models;
using Czar.Cms.Core.Options;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace Czar.Cms.Core.CodeGenerator
{
    /// <summary>
    /// SqlServer数据库代码生成器
    /// </summary>
    public class CodeGeneratorSqlServerProvider : ICodeGeneratorProvider
    {
        public List<DbTable> GetModelFromDatabase(CodeGenerateOption options, bool isCoveredExsited = true)
        { 
            DatabaseType dbType = DatabaseType.SqlServer;
            string strGetAllTables = @"SELECT DISTINCT d.name as TableName, f.value as TableComment
FROM      sys.syscolumns AS a LEFT OUTER JOIN
                sys.systypes AS b ON a.xusertype = b.xusertype INNER JOIN
                sys.sysobjects AS d ON a.id = d.id AND d.xtype = 'U' AND d.name <> 'dtproperties' LEFT OUTER JOIN
                sys.syscomments AS e ON a.cdefault = e.id LEFT OUTER JOIN
                sys.extended_properties AS g ON a.id = g.major_id AND a.colid = g.minor_id LEFT OUTER JOIN
                sys.extended_properties AS f ON d.id = f.major_id AND f.minor_id = 0";
            List<DbTable> tables = null;
            using (var conn = new SqlConnection(options.ConnectionString))
            {
                tables = conn.Query<DbTable>(strGetAllTables).ToList();
                tables.ForEach(item =>
                {
                    string strGetTableColumns = @"SELECT   a.name AS ColName, CONVERT(bit, (CASE WHEN COLUMNPROPERTY(a.id, a.name, 'IsIdentity') 
                = 1 THEN 1 ELSE 0 END)) AS IsIdentity, CONVERT(bit, (CASE WHEN
                    (SELECT   COUNT(*)
                     FROM      sysobjects
                     WHERE   (name IN
                                         (SELECT   name
                                          FROM      sysindexes
                                          WHERE   (id = a.id) AND (indid IN
                                                              (SELECT   indid
                                                               FROM      sysindexkeys
                                                               WHERE   (id = a.id) AND (colid IN
                                                                                   (SELECT   colid
                                                                                    FROM      syscolumns
                                                                                    WHERE   (id = a.id) AND (name = a.name))))))) AND (xtype = 'PK')) 
                > 0 THEN 1 ELSE 0 END)) AS IsPrimaryKey, b.name AS ColumnType, COLUMNPROPERTY(a.id, a.name, 'PRECISION') 
                AS ColumnLength, CONVERT(bit, (CASE WHEN a.isnullable = 1 THEN 1 ELSE 0 END)) AS IsNullable, ISNULL(e.text, '') 
                AS DefaultValue, ISNULL(g.value, ' ') AS Comment
FROM      sys.syscolumns AS a LEFT OUTER JOIN
                sys.systypes AS b ON a.xtype = b.xusertype INNER JOIN
                sys.sysobjects AS d ON a.id = d.id AND d.xtype = 'U' AND d.name <> 'dtproperties' LEFT OUTER JOIN
                sys.syscomments AS e ON a.cdefault = e.id LEFT OUTER JOIN
                sys.extended_properties AS g ON a.id = g.major_id AND a.colid = g.minor_id LEFT OUTER JOIN
                sys.extended_properties AS f ON d.id = f.class AND f.minor_id = 0
WHERE   (b.name IS NOT NULL) AND (d.name = @TableName)
ORDER BY a.id, a.colorder";
                    item.Columns = conn.Query<DbTableColumn>(strGetTableColumns, new
                    {
                        TableName = item.TableName
                    }).ToList();

                    item.Columns.ForEach(x =>
                    {
                        var csharpType = DbColumnTypeCollection.DbColumnDataTypes.FirstOrDefault(t =>
                            t.DatabaseType == dbType && t.ColumnTypes.Split(',').Any(p =>
                                p.Trim().Equals(x.ColumnType, StringComparison.OrdinalIgnoreCase)))?.CSharpType;
                        if (string.IsNullOrEmpty(csharpType))
                        {
                            throw new SqlTypeException($"未从字典中找到\"{x.ColumnType}\"对应的C#数据类型，请更新DbColumnTypeCollection类型映射字典。");
                        }

                        x.CSharpType = csharpType;
                    });
                });
            }

            return tables;
        }
    }
}
