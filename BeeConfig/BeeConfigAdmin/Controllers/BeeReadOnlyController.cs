using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigDal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeeConfigAdmin.Controllers
{
    [Route("api/BeeReadOnly/[action]")]
    public class BeeReadOnlyController:BaseController
    {
        private readonly IReadOnlyService _readOnlyService;

        public BeeReadOnlyController(IReadOnlyService readOnlyService)
        {
            _readOnlyService = readOnlyService;
        }

        public async Task<IActionResult> GetEnvSelect2DataList()
        {
            return await ActionWrapAsync(async () =>
            {
                var result = await _readOnlyService.GetEnvSelect2DataList();
                return result;
            });
        }

        public async Task<IActionResult> GetAppSelect2DataList()
        {
            return await ActionWrapAsync(async () =>
            {
                var result = await _readOnlyService.GetAppSelect2DataList();
                return result;
            });
        }
    }
}

