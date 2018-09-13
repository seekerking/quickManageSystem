using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeeConfigModels.Dto;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services.Interfaces
{
  public  interface IReqLogService
  {
      Task<ResultPageData<RpsReqLog>> GetPageList(ParamPage<ParamReqLogSearchFilter> param);
  }
}
