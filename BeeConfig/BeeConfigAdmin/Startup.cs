using BeeConfigAdmin.Auth;
using BeeConfigDal.Services;
using BeeConfigDal.Services.Interfaces;
using BeeConfigModels.Common;
using BeeConfigModels.Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeeConfigAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //如果都是使用依赖注入的形式，这里就不需要这么写，因为Repository使用的是new的形式，所以配置接口不能注入
            ConfigHelper.Config= Configuration.Get<ApplicationConfig>();
        }

        public IConfiguration Configuration { get; }

        //这里直接使用的原始的授权登录，有条件可以转成Identity模式
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                //设置序列化默认时间格式以及首字母小写
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            });
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", options =>
                {
                    options.LoginPath = new PathString("/login.html");
                    options.AccessDeniedPath = new PathString("/login.html");
                    options.Cookie.Name = "CookieAuth";
                });
            services.AddScoped(typeof(IAppService), typeof(AppService));
            services.AddScoped(typeof(IEnvService), typeof(EnvService));
            services.AddScoped(typeof(IConfigService), typeof(ConfigService));
            services.AddScoped(typeof(IReadOnlyService), typeof(ReadOnlyService));
            services.AddScoped(typeof(IReqLogService), typeof(ReqLogService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseProtectFolder(new ProtectFolderOptions()
            {
                Path = new PathString("/api"),
                PolicyName = "secret"
            });


            app.UseMvcWithDefaultRoute();
        }
    }
}
