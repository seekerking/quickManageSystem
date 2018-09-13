using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigDal.Entity;
using BeeConfigDal.Exntesion;
using BeeConfigModels;
using BeeConfigModels.Dto;
using BeeConfigModels.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BeeConfigDal.Repository
{
   public class ConfigRepository:BaseRepository<Config>
    {

        public async Task<bool> AddOrUpdate(ParamConfigInfo param)
        {
            var entity = param.Id > 0 ? Context.Configs.Single(x => x.Id == param.Id) : Context.Configs.Attach(new Config()).Entity;
        
            entity.ConfigValue = param.ConfigValue;
            entity.ConfigDesc = param.ConfigDesc;
            entity.LastUpdate = DateTime.Now;
            entity.LastTimespan= BeeUtils.ConvertToTimeStamp(DateTime.Now);

            if (param.Id <= 0)
            {
                entity.ConfigId = param.ConfigId;
                entity.AppId = param.AppId;
                entity.EnvId = param.EnvId;
                entity.CreateDate = DateTime.Now;
                Context.Configs.Add(entity);
            }
            else
            {

                Context.Configs.Update(entity);
            }

            return (await Context.SaveChangesAsync()) > 0;

        }

        public async Task<bool> Delete(ParamConfigInfo param)
        {
            if (param.Id <= 0)
            {
                return false;
            }
            var entity = Context.Configs.Attach(new Config(){Id = param.Id,Status = 1});

            entity.Property(x => x.Status).IsModified = true;
            return (await Context.SaveChangesAsync()) > 0;
        }

        public async Task<ResultPageData<RpsConfig>> GetPageList(ParamPage<ParamConfigSearchFilter> param)
        {
            var query = Context.Configs.AsNoTracking()
                .OrderBy(x => x.CreateDate.ToString("yyyy-MMMM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                .WhereIf(!string.IsNullOrEmpty(param.Filter.AppId),x=>x.AppId.Contains(param.Filter.AppId))
                .WhereIf(!string.IsNullOrEmpty(param.Filter.EnvId),x=>x.EnvId.Contains(param.Filter.EnvId))
                .Where(x => x.Status == 0);
            var list =
               await query
                   .Join(Context.Apps.AsNoTracking(),x=>x.AppId,y=>y.AppId, (x, y) =>
                   new RpsConfig
                   {   Id = x.Id,
                       AppId = x.AppId,
                       AppName = y.AppName,
                       ConfigDesc = x.ConfigDesc,
                       ConfigId = x.ConfigId,
                       ConfigValue = x.ConfigValue,
                       CreateDate = x.CreateDate,
                       EnvId = x.EnvId,
                       LastUpdate = x.LastUpdate,
                       LastTimeSpan = x.LastTimespan,
                   })
                    .GetPageList(param.Index, param.Size);
            return new ResultPageData<RpsConfig>()
            {
                Data = list,
                Total = await query.CountAsync(),
                Pagesize = param.Size,
                Pageindex = param.Index
            };

        }
    }
}
