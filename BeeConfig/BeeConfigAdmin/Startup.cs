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

            //�������ʹ������ע�����ʽ������Ͳ���Ҫ��ôд����ΪRepositoryʹ�õ���new����ʽ���������ýӿڲ���ע��
            ConfigHelper.Config= Configuration.Get<ApplicationConfig>();
        }

        public IConfiguration Configuration { get; }

        //����ֱ��ʹ�õ�ԭʼ����Ȩ��¼������������ת��Identityģʽ
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                //�������л�Ĭ��ʱ���ʽ�Լ�����ĸСд
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
