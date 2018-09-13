using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeeConfigDal.Services.Interfaces;
using BeeConfigModels.Dto;
using BeeConfigModels.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BeeConfigAdmin.Controllers
{
    [Route("api/BeeConfig/[action]")]
    public class BeeConfigController : BaseController
    {
        private readonly IConfigService _configService;

        public BeeConfigController(IConfigService configService)
        {
            _configService = configService;
        }

        [HttpPost]
        public async Task<IActionResult> GetPageList(ParamPage<ParamConfigSearchFilter> param)
        {
            return await ActionWrapAsync(async () => await _configService.GetPageList(param));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(ParamConfigInfo param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<bool> result = new ResultData<bool>();
                result.Data = await _configService.AddOrUpdate(param);
                return result;

            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ParamConfigInfo param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<bool> result = new ResultData<bool>();
                result.Data = await _configService.Delete(param);
                return result;

            });
        }

        [HttpPost]
        public async Task<IActionResult> Publish(ParamPublishConfig param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<bool> result = new ResultData<bool>();
                result.Data = await _configService.Publish(param);
                return result;

            });
        }
       

    }
}