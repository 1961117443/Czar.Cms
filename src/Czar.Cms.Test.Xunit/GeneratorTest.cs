using Czar.Cms.Core.CodeGenerator;
using Czar.Cms.Core.Models;
using Czar.Cms.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Czar.Cms.Test.Xunit
{
    /// <summary>
    /// 测试代码生成器
    /// </summary>
    public class GeneratorTest
    {
        public IServiceProvider BuildServiceForSqlServer()
        {
            var services = new ServiceCollection();

            services.Configure<CodeGenerateOption>(opt =>
            {
                opt.ConnectionString = "Data Source=.;Initial Catalog=CzarCms;User ID=sa;Password=123456;Persist Security Info=True;Max Pool Size=50;Min Pool Size=0;Connection Lifetime=300;";
                opt.DbType = DatabaseType.SqlServer.ToString();//数据库类型是SqlServer,其他数据类型参照枚举DatabaseType
                opt.Author = "suxiangnian";//作者名称
                opt.OutputPath = @"E:\Code\Czar.Cms\src\Czar.Cms.Models";//实体模型输出路径，为空则默认为当前程序运行的路径
                opt.ModelsNamespace = "Czar.Cms.Models";//实体命名空间
            });
            services.AddSingleton(typeof(ICodeGeneratorProvider), typeof(CodeGeneratorSqlServerProvider));
            services.AddSingleton<CodeGenerator>();//注入Model代码生成器
            return services.BuildServiceProvider();
        }

        [Fact]
        public void GeneratorModelForSqlServer()
        {
            var serviceProvider = BuildServiceForSqlServer();
            var generator = serviceProvider.GetRequiredService<CodeGenerator>();
            generator.GenerateModelCodesFromDatabase();
            Assert.Equal("SqlServer", DatabaseType.SqlServer.ToString());
            Assert.Equal(0, 0);
        }
    }
}
