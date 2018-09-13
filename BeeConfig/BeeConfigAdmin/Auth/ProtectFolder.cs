using System.Threading.Tasks;
using BeeConfigModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BeeConfigAdmin.Auth
{
    public class ProtectFolder
    {
        private readonly RequestDelegate _next;
        private readonly PathString _path;
        private readonly string _policyName;
            
        public ProtectFolder(RequestDelegate next, ProtectFolderOptions options)
        {
            _next = next;
            _path = options.Path;
            _policyName = options.PolicyName;
        }

        public async Task Invoke(HttpContext httpContext,
            IAuthorizationService authorizationService)
        {

            if (httpContext.Request.Path.StartsWithSegments(_path)&&!httpContext.Request.Path.StartsWithSegments(_path+"/beehome/login"))
            {

                if (httpContext.User.Identity.IsAuthenticated)
                {
                    await _next(httpContext);
                }
                else
                {
                    ResultData<RpsLoginInfo> result = new ResultData<RpsLoginInfo>();
                    result.Status = 2;
                    result.Msg = "没有登录，无法获取数据";
                    result.Data =new RpsLoginInfo()
                    {
                        Success = false,
                        ErrorMessage = "没有登录，无法获取数据",
                        ReturnUrl = "login.html"
                    };
                    httpContext.Response.ContentType = "application/json";
                   await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result,new JsonSerializerSettings()
                   {
                       ContractResolver = new CamelCasePropertyNamesContractResolver(),
                       Formatting = Formatting.Indented
                   }));
                }
            }
            else
            {
                await _next(httpContext);
            }




        }
    }
}
