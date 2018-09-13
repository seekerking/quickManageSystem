using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeConfigDal.Entity;
using BeeConfigDal.Exntesion;
using BeeConfigModels.Dto;
using BeeConfigModels.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BeeConfigDal.Repository
{
    public class AppRepository : BaseRepository<App>
    {

        public async Task<bool> AddOrUpdate(ParamAppInfo param)
        {
            var entity = param.Id > 0 ? Context.Apps.Single(x => x.Id == param.Id) : Context.Apps.Attach(new App()).Entity;
            entity.AppId = param.AppId;
            entity.AppName = param.AppName;
            entity.AppDesc = param.AppDesc;
            entity.LastUpdate = DateTime.Now;

            if (param.Id <= 0)
            {
                entity.CreateDate = DateTime.Now;
                entity.Secret=Guid.NewGuid();
                Context.Apps.Add(entity);
            }
            else
            {
                if (!entity.AppId.Equals(param.AppId))
                {
                    var isUsed = Context.Configs.Any(x => x.AppId == entity.AppId && x.Status == 0);
                    if (isUsed)
                    {
                        throw new Exception("在Config该App已经被使用了,不能修改");
                    }
                }
                Context.Apps.Update(entity);
            }

            return (await Context.SaveChangesAsync()) > 0;

        }

        public async Task<bool> Delete(ParamAppInfo param)
        {
            if (param.Id <= 0)
            {
                return false;
            }
            var entity = Context.Apps.Single(x=>x.Id==param.Id);
            var isUsed = Context.Configs.Any(x => x.AppId == entity.AppId && x.Status == 0);
            if (isUsed)
            {
                throw new Exception("在Config该App已经被使用了,不能删除");
            }

            entity.Status = 1;
            Context.Apps.Update(entity);
            return (await Context.SaveChangesAsync()) > 0;
        }

        public async Task<ResultPageData<RspApp>> GetPageList(ParamPage<ParamAppSearchFilter> param)
        {
            var list =
               await Context.Apps.AsNoTracking()
                    .OrderBy(x => x.CreateDate.ToString("yyyy-MMMM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                   .WhereIf(!string.IsNullOrEmpty(param.Filter.AppId), x => x.AppId.Contains(param.Filter.AppId))
                    .Select(x => new RspApp()
                    {
                        Id = x.Id,
                        AppId = x.AppId,
                        AppName = x.AppName,
                        AppDesc = x.AppDesc,
                        CreateDate = x.CreateDate,
                        LastUpdate = x.LastUpdate,
                        AppSecret = x.Secret.ToString()

                    })
                    .GetPageList(param.Index, param.Size);
            return new ResultPageData<RspApp>()
            {
                Data = list,
                Total = await Context.Apps.CountAsync(),
                Pagesize = param.Size,
                Pageindex = param.Index
            };

        }
            
    }
}
