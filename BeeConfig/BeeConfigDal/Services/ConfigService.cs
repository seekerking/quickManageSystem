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
  public  class ConfigService:IConfigService
    {

        private readonly  ConfigRepository _configRepository=new ConfigRepository();
        private readonly PublishRepository _publishRepository=new PublishRepository();
        public async Task<bool> AddOrUpdate(ParamConfigInfo param)
        {
            return await _configRepository.AddOrUpdate(param);
        }

        public async Task<bool> Delete(ParamConfigInfo param)
        {
            return await _configRepository.Delete(param);
        }

        public async Task<ResultPageData<RpsConfig>> GetPageList(ParamPage<ParamConfigSearchFilter> param)
        {
            return await _configRepository.GetPageList(param);
        }

        public async Task<bool> Publish(ParamPublishConfig param)
        {
            return await _publishRepository.Add(param);
        }
    }
}
