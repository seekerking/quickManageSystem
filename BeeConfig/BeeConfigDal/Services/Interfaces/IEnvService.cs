using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services.Interfaces
{
  public  interface IEnvService
    {
        Task<bool> AddOrUpdate(ParamEnvInfo param);
        Task<bool> Delete(ParamEnvInfo param);
        Task<ResultPageData<RpsEnv>> GetPageList(ParamPage<object> param);
    }
}
