using System;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigAdmin.Auth;
using BeeConfigDal.Entity;
using BeeConfigDal.Services.Interfaces;
using BeeConfigModels.Dto;
using BeeConfigModels.Filter;
using BeeConfigModels.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeeConfigAdmin.Controllers
{
    [Route("api/BeeHome/[action]")]
    public class BeeHomeController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserService _userService;

        public BeeHomeController(IHostingEnvironment hostingEnvironment, IUserService userService)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(ParamLoginInfo loginInfo)
        {
            return await ActionWrapAsync(async () =>
            {
                AuthBll auth = new AuthBll(_userService, HttpContext);
                ResultData<RpsLoginInfo> result = new ResultData<RpsLoginInfo>();
                result.Data = await auth.AuthUser(loginInfo);

                return result;
            });
        }



        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(ParamUserInfo param)
        {
            return await ActionWrapAsync(async () =>
            {
              
                ResultData<bool> result = new ResultData<bool>();
                result.Data = await _userService.AddOrUpdate(param);
                return result;
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPwd(ParamUsetRestPwdInfo param)
        {
            return await ActionWrapAsync(async () =>
            {

                ResultData<bool> result = new ResultData<bool>();
                result.Data = await _userService.ResetPassWord(param);
                return result;
            });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(ParamUserInfo param)
        {
            return await ActionWrapAsync(async () =>
            {

                ResultData<bool> result = new ResultData<bool>();
                result.Data = await _userService.DeleteUser(param);
                return result;
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetUserInfo()
        {
            return await ActionWrapAsync(async () =>
            {

                ResultData<RpsUser> result = new ResultData<RpsUser>();
                string id= HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("未登录");
                }
                result.Data = await _userService.GetUserInfo(int.Parse(id));
                return result;
            });

        }

        [HttpPost]
        public async Task<IActionResult> GetPageList(ParamPage<ParamUserSearchFilter> param)
        {
            return await ActionWrapAsync(async () => await _userService.GetPageList(param));
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
             await HttpContext.SignOutAsync();

             return Redirect("../../login.html");
        }
    }
}