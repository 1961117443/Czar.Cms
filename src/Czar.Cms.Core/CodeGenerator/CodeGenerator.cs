using Czar.Cms.Core.Extensions;
using Czar.Cms.Core.Models;
using Czar.Cms.Core.Options;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Czar.Cms.Core.CodeGenerator
{
    /// <summary>
    /// 代码生成器
    /// </summary>
    public class CodeGenerator  
    {
        private readonly string Delimiter = "\\";//分隔符，默认为windows下的\\分隔符

        private ICodeGeneratorProvider _provider;

        private static CodeGenerateOption _options;
        public CodeGenerator(IOptions<CodeGenerateOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            _options = options.Value;
            if (_options.ConnectionString.IsNullOrWhiteSpace())
                throw new ArgumentNullException("不指定数据库连接串就生成代码，你想上天吗？");
            if (_options.DbType.IsNullOrWhiteSpace())
                throw new ArgumentNullException("不指定数据库类型就生成代码，你想逆天吗？");
            if (_options.DbType != DatabaseType.SqlServer.ToString())
                throw new ArgumentNullException("这是我的错，目前只支持MSSQL数据库的代码生成！后续更新MySQL");

            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (_options.OutputPath.IsNullOrWhiteSpace())
                _options.OutputPath = path;
            var flag = path.IndexOf("/bin");
            if (flag > 0)
                Delimiter = "/";//如果可以取到值，修改分割符
        }

        public CodeGenerator(IOptions<CodeGenerateOption> options,ICodeGeneratorProvider provider):this(options)
        {
            _provider = provider;
        }

        /// <summary>
        /// 根据数据库连接字符串生成数据库表对应的Model层代码
        /// </summary>
        /// <param name="isCoveredExsited">是否覆盖已存在的同名文件</param>
        public void GenerateModelCodesFromDatabase(bool isCoveredExsited = true)
        {
            List<DbTable> tbs = _provider?.GetModelFromDatabase(_options);
            if (tbs != null && tbs.Any())
            {
                foreach (var table in tbs)
                {
                    if (table.Columns.Any(c => c.IsPrimaryKey))
                    {
                        GenerateEntity(table, isCoveredExsited);
                    }
                }
            }
        }

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <param name="table"></param>
        /// <param name="isCoveredExsited"></param>
        private void GenerateEntity(DbTable table, bool isCoveredExsited = true)
        {
            var modelPath = _options.OutputPath;
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            var fullPath = modelPath + Delimiter + table.TableName + ".cs";
            if (File.Exists(fullPath) && !isCoveredExsited)
                return;

            var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
            var sb = new StringBuilder();
            foreach (var column in table.Columns)
            {
                var tmp = GenerateEntityProperty(column);
                if (sb.Length==0 && tmp.StartsWith("\t\t"))
                {
                    tmp = tmp.Substring(2);
                }
                sb.AppendLine(tmp);
            }
            var content = ReadTemplate("ModelTemplate.txt");
            content = content.Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ModelsNamespace}", _options.ModelsNamespace)
                .Replace("{Author}", _options.Author)
                .Replace("{Comment}", table.TableComment)
                .Replace("{ModelName}", table.TableName)
                .Replace("{ModelProperties}", sb.ToString());
            WriteAndSave(fullPath, content);
        }

        /// <summary>
        /// 生成属性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">列</param>
        /// <returns></returns>
        private static string GenerateEntityProperty(DbTableColumn column)
        {
            var sb = new StringBuilder();
            if (!column.Comment.IsNullOrWhiteSpace())
            { 
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine("\t\t/// " + column.Comment);
                sb.AppendLine("\t\t/// </summary>");
            }
            var colType = column.CSharpType;
            if (colType.ToLower() != "string" && colType.ToLower() != "byte[]" && colType.ToLower() != "object" &&
                column.IsNullable)
            {
                colType = colType + "?";
            }
            sb.AppendLine($"\t\tpublic {colType} {column.ColName} " + "{get;set;}");
            return sb.ToString();
        }

        /// <summary>
        /// 从代码模板中读取内容 文件的生成操作必须选择"嵌入的资源"
        /// </summary>
        /// <param name="templateName">模板名称，应包括文件扩展名称。比如：template.txt</param>
        /// <returns></returns>
        private string ReadTemplate(string templateName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var content = string.Empty;
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.CodeTemplate.{templateName}"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        content = reader.ReadToEnd();
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        private static void WriteAndSave(string fileName, string content)
        {
            //实例化一个文件流--->与写入文件相关联
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                //实例化一个StreamWriter-->与fs相关联
                using (var sw = new StreamWriter(fs))
                {
                    //开始写入
                    sw.Write(content);
                    //清空缓冲区
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
                }
            }
        }

    }
}
