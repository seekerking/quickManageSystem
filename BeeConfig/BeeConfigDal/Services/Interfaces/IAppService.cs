using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeeConfigModels.Dto;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services.Interfaces
{
  public  interface IAppService
  {
      Task<bool> AddOrUpdate(ParamAppInfo param);
      Task<bool> Delete(ParamAppInfo param);
      Task<ResultPageData<RspApp>> GetPageList(ParamPage<ParamAppSearchFilter> param);
  }
}
