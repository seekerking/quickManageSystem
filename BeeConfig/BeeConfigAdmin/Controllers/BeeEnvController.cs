using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigDal.Services.Interfaces;
using BeeConfigModels.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BeeConfigAdmin.Controllers
{

    [Route("api/BeeEnv/[action]")]
    public class BeeEnvController:BaseController
    {
        private readonly IEnvService _envService;

        public BeeEnvController(IEnvService envService)
        {
            _envService = envService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(ParamEnvInfo param)
        {

            return await ActionWrapAsync(async () => {
                ResultData<bool> result = new ResultData<bool>();
                result.Data = await _envService.AddOrUpdate(param);
                return result;
            });

        }

        [HttpPost]
        public async Task<IActionResult> GetPageList(ParamPage<object> param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultPageData<RpsEnv> result = new ResultPageData<RpsEnv>();
                result = await _envService.GetPageList(param);
                return result;
            });
        }

        [HttpPost]

        public async Task<IActionResult> Delete(ParamEnvInfo param)
        {
            return await ActionWrapAsync(async () =>
            {
                ResultData<bool> result = new ResultData<bool>();
                result.Data = await _envService.Delete(param);
                return result;
            });
        }
    }
}
