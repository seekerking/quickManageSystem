using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigApi.Models;
using BeeConfigApi.Services; 
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BeeConfigApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BeeConfigController : Controller
    {
        private readonly BeeConfigService _service;

        public BeeConfigController(BeeConfigService beeConfigService)
        {
            _service = beeConfigService;
        }


        [HttpGet]
        public async Task<IActionResult> GetUpdate([FromQuery] ParamConfig param)
        {

            string sign = string.Empty;
            if (!string.IsNullOrEmpty(param.AppId))
            {
                sign = await _service.GetAppSign(param.AppId);
            }

            if (!param.IsValidate(sign))
            {
                return Unauthorized();
            }

            var list = await _service.GetAllPublishs();

            var temp = (from item in list
                where item.AppId.Equals(param.AppId)
                      && item.EnvId.Equals(param.Env)
                select item).ToList();
            var lastPublish = temp.OrderByDescending(x => x.PublishTimespan).FirstOrDefault();

            await _service.SaveRequest(new EntityReq
            {
                AppEnv = param.Env,
                AppId = param.AppId,
                ClientIp = GetClientIp(),
                FirstDate = DateTime.Now,
                LastDate = DateTime.Now,
                LastConfigDate =
                    lastPublish == null ? DateTime.MinValue : BeeUtils.ConvertToDateTime(lastPublish.PublishTimespan)
            });

            if (lastPublish==null)
            {
                return StatusCode(202);
            }

            if (lastPublish.PublishTimespan > param.Lastupdate)
            {

                Dictionary<string, string> dicConfig = new Dictionary<string, string>();
               var configs= JsonConvert.DeserializeObject<List<EntityConfig>>(lastPublish.Data);
                configs.ForEach(item =>
                {
                    dicConfig.Add(item.ConfigId, item.ConfigValue);
                });


                return Ok(new DtoBeeConfig
                {
                    BeeConfigAppId = param.AppId,
                    BeeConfigData = dicConfig,
                    BeeConfigEnvironment = param.Env,
                    BeeConfigLastUpdate = lastPublish.PublishTimespan
                });
            }

            return StatusCode(304);
        }

        protected string GetClientIp()
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            return ip;
        }
    }
}