using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeeConfigDal.Repository;
using BeeConfigDal.Services.Interfaces;
using BeeConfigModels.Dto;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services
{
   public class ReqLogService:IReqLogService
    {
        private readonly ReqLogRepository _reqLogRepository=new ReqLogRepository();

        public async Task<ResultPageData<RpsReqLog>> GetPageList(ParamPage<ParamReqLogSearchFilter> param)
        {
            return await _reqLogRepository.GetPageList(param);
        }
    }
}
