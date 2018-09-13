using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigDal.Entity;
using BeeConfigDal.Exntesion;
using BeeConfigModels.Dto;
using BeeConfigModels.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BeeConfigDal.Repository
{
  public  class ReqLogRepository:BaseRepository<ReqLog>
    {

        public async Task<ResultPageData<RpsReqLog>> GetPageList(ParamPage<ParamReqLogSearchFilter> param)
        {
            var query = Context.ReqLogs.AsNoTracking();
            var list=  await   query
                .WhereIf(!string.IsNullOrEmpty(param.Filter.AppId),x=>x.AppId.Contains(param.Filter.AppId))
                .WhereIf(!string.IsNullOrEmpty(param.Filter.ClientIp), x => x.ClientIp.Contains(param.Filter.ClientIp))
                .WhereIf(!string.IsNullOrEmpty(param.Filter.EnvId), x => x.AppEnv.Contains(param.Filter.EnvId))
                .OrderBy(x => x.LastDate.ToString("yyyy-MMMM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                .Select(x=>new RpsReqLog()
                {
                    Id=x.Id,
                    ClientIp = x.ClientIp,
                    AppEnv = x.AppEnv,
                    AppId = x.AppId,
                    FirstDate = x.FirstDate,
                    LastConfigDate = x.LastConfigDate,
                    LastDate = x.LastDate,
                    ReqTimes = x.ReqTimes

                })
                .GetPageList(param.Index, param.Size);

            return new ResultPageData<RpsReqLog>()
            {
                Data = list,
                Pageindex = param.Index,
                Pagesize = param.Size,
                Total = await query.CountAsync()
            };

        }
    }
}
