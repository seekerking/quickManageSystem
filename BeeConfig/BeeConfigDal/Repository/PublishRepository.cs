using System;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigDal.Entity;
using BeeConfigModels.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BeeConfigDal.Repository
{
    public class PublishRepository : BaseRepository<Publish>
    {
        public async Task<bool> Add(ParamPublishConfig param)
        {
            if (param.Id > 0) return false;

            var publishConfigs = await Context.Configs.AsNoTracking()
                   .Where(x => x.AppId == param.AppId && x.EnvId == param.EnvId && x.Status == 0)
                   .ToListAsync();
            var lastConfig = publishConfigs.OrderByDescending(x => x.LastTimespan).FirstOrDefault();
            if (!publishConfigs.Any())
            {
                throw new Exception("没有配置无须发布");
            }

            var isAlreadyPublish = await Context.Publishs.AnyAsync(x =>
                    x.AppId == param.AppId && x.EnvId == param.EnvId && x.PublishTimeSpan == lastConfig.LastTimespan);
            if (isAlreadyPublish)
            {
                throw new Exception("已经发布过最新了，无须再发布");
            }


            var entity = new Publish()
            {
                AppId = param.AppId,
                EnvId = param.EnvId,
                PublishTimeSpan = lastConfig?.LastTimespan ?? 0L,
                Data = JsonConvert.SerializeObject(publishConfigs)
            };

            Context.Publishs.Add(entity);

            return await (Context.SaveChangesAsync()) > 0;
        }
    }
}