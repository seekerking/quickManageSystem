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
    [Route("api/BeeReqLog/[action]")]
    public class BeeReqLogController:BaseController
    {
        private readonly IReqLogService _reqLogService;

        public BeeReqLogController(IReqLogService reqLogService)
        {
            _reqLogService = reqLogService;
        }

        public async Task<IActionResult> GetPageList(ParamPage<ParamReqLogSearchFilter> param)
        {
            return await ActionWrapAsync(async () => await _reqLogService.GetPageList(param));
        }
    }
}
