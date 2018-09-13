using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BeeConfigDal.Services.Interfaces;
using BeeConfigModels.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;


namespace BeeConfigAdmin.Auth
{
    public class AuthBll
    {
        private readonly IUserService _userService;
        private readonly HttpContext _httpContext;
        public AuthBll(IUserService userService,HttpContext httpContext)
        {
            _userService = userService;
            _httpContext = httpContext;
        }

        public async Task<RpsLoginInfo> AuthUser(ParamLoginInfo loginUser)
        {
            RpsLoginInfo result=new RpsLoginInfo()
            {
                ReturnUrl = loginUser.ReturnUrl,
                Success = false
            };
            try
            {
                var user = await ValidateUser(loginUser.UserName, loginUser.Password);
                if (user != null)
                {

                    loginUser.Id = user.Id;
                    loginUser.Name = user.Name;
                    await SignIn(loginUser);
                    result.Success = true;
                    return result;
                }
                result.Success = false;
                result.ErrorMessage = "没有该用户";
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;

            }

            return result;
        }

        private async Task<RpsUser> ValidateUser(string userId, string pwd)
        {
            return (await _userService.IsUserExist(userId)) ? await _userService.GetUser(userId, pwd) : null;
        }

       private async Task SignIn(ParamLoginInfo loginUser)
       {
            var claims = new List<Claim>(){
                new Claim(ClaimTypes.Name,loginUser.Name),
                new Claim("UserId",loginUser.UserName),
                new Claim("Id",loginUser.Id.ToString())
            };                //init the identity instances 
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Operator"));                //signin 
           
            await _httpContext.SignInAsync("CookieAuth", userPrincipal, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddDays(1),
                IsPersistent = loginUser.IsPersistent,
                AllowRefresh = false
            });
        }
    }
}
