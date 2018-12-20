using Czar.Cms.Core.CodeGenerator;
using Czar.Cms.Core.Models;
using Czar.Cms.Core.Options;
using Czar.Cms.IRepository;
using Czar.Cms.Models;
using Czar.Cms.Repository.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
                opt.OutputPath = @"E:\Code\Czar.Cms\CzarCmsCodeGenerator";//实体模型输出路径，为空则默认为当前程序运行的路径
                opt.ModelsNamespace = "Czar.Cms.Models";//实体命名空间
                opt.IRepositoryNamespace = "Czar.Cms.IRepository";//仓储接口命名空间
                opt.RepositoryNamespace = "Czar.Cms.Repository.SqlServer";//仓储命名空间
            });
            services.AddSingleton(typeof(ICodeGeneratorProvider), typeof(CodeGeneratorSqlServerProvider));
            services.Configure<DbOpion>("CzarCms", GetConfiguration().GetSection("DbOption"));
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            services.AddSingleton<CodeGenerator>();//注入Model代码生成器
            return services.BuildServiceProvider();
        }

        public IConfiguration GetConfiguration()
        { 
               var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }

        [Fact]
        public void GeneratorModelForSqlServer()
        {
            var serviceProvider = BuildServiceForSqlServer();
            var generator = serviceProvider.GetRequiredService<CodeGenerator>();
            generator.GenerateTemplateCodesFromDatabase(true);
            Assert.Equal("SqlServer", DatabaseType.SqlServer.ToString());
            Assert.Equal(0, 0);
        }

        [Fact()]
        public void TestBaseFactory()
        {
            IServiceProvider serviceProvider = BuildServiceForSqlServer();
            IArticleCategoryRepository articleCategoryRepository = serviceProvider.GetService<IArticleCategoryRepository>();

            var category = new ArticleCategory
            {
                Title = "随笔",
                ParentId = 0,
                ClassList = "",
                ClassLayer = 0,
                Sort = 0,
                ImageUrl = "",
                SeoTitle = "随笔的SEOTitle",
                SeoKeywords = "随笔的SeoKeywords",
                SeoDescription = "随笔的SeoDescription",
                IsDeleted = false,
            };
            var categoryId = articleCategoryRepository.Insert(category);
            var list = articleCategoryRepository.GetList();
            Assert.True(3 == list.Count()); 
            Assert.Equal("随笔", list.FirstOrDefault().Title);
            Assert.Equal("SQLServer", DatabaseType.SqlServer.ToString(), ignoreCase: true);
            articleCategoryRepository.Delete(categoryId.Value);
            var count = articleCategoryRepository.RecordCount(); 
            Assert.True(2 == count);
        }

        [Fact]
        public async void TestBaseFactoryAsync()
        {
            IServiceProvider serviceProvider = BuildServiceForSqlServer();
            IArticleCategoryRepository articleCategoryRepository = serviceProvider.GetService<IArticleCategoryRepository>();
            var count = await articleCategoryRepository.RecordCountAsync();
            Assert.True(2 == count);

        }

    }
}
