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
   public class AppService:IAppService
    {
        private AppRepository _appRepository=new AppRepository();

        public async Task<bool> AddOrUpdate(ParamAppInfo param)
        {
            return await _appRepository.AddOrUpdate(param);
        }

        public async Task<bool> Delete(ParamAppInfo param)
        {
            return await _appRepository.Delete(param);
        }

        public async Task<ResultPageData<RspApp>> GetPageList(ParamPage<ParamAppSearchFilter> param)
        {
            return await _appRepository.GetPageList(param);
        }
    }
}
