using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexinea.Autofac.Extensions.DependencyInjection;
using Autofac;
using AutoMapper;
using Czar.Cms.Core.Options;
using Czar.Cms.IRepository;
using Czar.Cms.IServices;
using Czar.Cms.Repository.SqlServer;
using Czar.Cms.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Czar.Cms.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<DbOpion>("CzarCms", Configuration.GetSection("DbOption"));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false; //true 的时候 sessionId 总是会变化
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = "/Account/Index";
                    opt.LogoutPath = "/Account/Logout";
                    opt.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                });
            

            //services.AddScoped<IManagerRoleService, ManagerRoleService>();
            //services.AddScoped<IManagerRoleRepository, ManagerRoleRepository>();
            #region 设置防伪选项 要在 AddMvc 前面
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiforgeryKey_xiaosu";
                options.HeaderName = "X-CSRF-TOKEN-xiaosu"; //X-CSRF-TOKEN-xiaosu
                options.SuppressXFrameOptionsHeader = false;
            });
            #endregion
            services.AddDistributedMemoryCache();
            services.AddSession(opt =>
            {
                opt.IOTimeout = TimeSpan.FromHours(15);
                opt.IdleTimeout = TimeSpan.FromHours(15);
                opt.Cookie.Name = "aiyu";
                opt.Cookie.HttpOnly = true;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region AutoMapper
            //依赖注入AutoMapper中需要用到的服务，其中包括AutoMapper的配置类Profile
            services.AddAutoMapper();
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterAssemblyTypes(typeof(ManagerRoleRepository).Assembly).Where(w => w.Name.EndsWith("Repository")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(ManagerRoleService).Assembly).Where(w => w.Name.EndsWith("Service")).AsImplementedInterfaces();
            #endregion

           

            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
