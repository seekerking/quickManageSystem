using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeConfigDal.Entity;
using BeeConfigDal.Exntesion;
using BeeConfigModels.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BeeConfigDal.Repository
{
    public class EnvRepository : BaseRepository<Env>
    {
        public async Task<bool> AddOrUpdate(ParamEnvInfo param)
        {
            var entity = param.Id > 0
                ? Context.Envs.Single(x => x.Id == param.Id)
                : Context.Envs.Attach(new Env()).Entity;

            entity.EnvId = param.EnvId;
            entity.EnvDesc = param.EnvDesc;
            entity.LastUpdate = DateTime.Now;

            if (param.Id <= 0)
            {
                entity.CreateDate = DateTime.Now;
                Context.Envs.Add(entity);
            }
            else
            {
                if (!entity.EnvId.Equals(param.EnvId))
                {
                    var isUsed = Context.Configs.Any(x => x.EnvId == entity.EnvId && x.Status == 0);
                    if (isUsed)
                    {
                        throw new Exception("在Config该Env已经被使用了,不能修改");
                    }
                }

                Context.Envs.Update(entity);
            }

            return (await Context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> Delete(ParamEnvInfo param)
        {
            if (param.Id <= 0)
            {
                return false;
            }

            var entity = Context.Envs.Single(x => x.Id == param.Id);
            var isUsed = Context.Configs.Any(x => x.EnvId == entity.EnvId && x.Status == 0);
            if (isUsed)
            {
                throw new Exception("在Config该Env已经被使用了，不能删除");
            }

            entity.Status = 1;
            Context.Envs.Update(entity);
            return (await Context.SaveChangesAsync()) > 0;
        }

        public async Task<ResultPageData<RpsEnv>> GetPageList(ParamPage<object> param)
        {
            var list =
                await Context.Envs.AsNoTracking()
                    .OrderBy(x => x.CreateDate.ToString("yyyy-MMMM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                    .Select(x => new RpsEnv()
                    {
                        Id = x.Id,
                        EnvId = x.EnvId,
                        EnvDesc = x.EnvDesc,
                        CreateDate = x.CreateDate,
                        LastUpdate = x.LastUpdate
                    })
                    .GetPageList(param.Index, param.Size);
            return new ResultPageData<RpsEnv>()
            {
                Data = list,
                Total = await Context.Envs.CountAsync(),
                Pagesize = param.Size,
                Pageindex = param.Index
            };
        }
    }
}