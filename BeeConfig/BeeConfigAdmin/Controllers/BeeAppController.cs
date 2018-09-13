using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigDal.Services.Interfaces;
using BeeConfigModels.Dto;
using BeeConfigModels.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BeeConfigAdmin.Controllers
{
    [Route("api/BeeApp/[action]")]
    public class BeeAppController:BaseController
    {
        private readonly IAppService _appService;

        public BeeAppController(IAppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(ParamAppInfo param)
        {

            return await ActionWrapAsync(async () => {
              ResultData<bool> result=new ResultData<bool>();
               result.Data= await _appService.AddOrUpdate(param);
                return result;
            });

        }

        [HttpPost]
        public async Task<IActionResult> GetPageList(ParamPage<ParamAppSearchFilter> param)
        {
            return await ActionWrapAsync(async ()=>
            {
                ResultPageData<RspApp> result=new ResultPageData<RspApp>();
                result = await _appService.GetPageList(param);
                return result;
            });
        }

        [HttpPost]

        public async Task<IActionResult> Delete(ParamAppInfo param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<bool> result = new ResultData<bool>();
                result.Data = await _appService.Delete(param);
                return result;
            });
        }
    }
}
