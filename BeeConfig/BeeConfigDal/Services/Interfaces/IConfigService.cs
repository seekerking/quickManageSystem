using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeeConfigModels.Dto;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services.Interfaces
{
   public interface IConfigService
    {
        Task<bool> AddOrUpdate(ParamConfigInfo param);
        Task<bool> Delete(ParamConfigInfo param);
        Task<ResultPageData<RpsConfig>> GetPageList(ParamPage<ParamConfigSearchFilter> param);

        Task<bool> Publish(ParamPublishConfig param);
    }
}
