using System.Threading.Tasks;
using BeeConfigDal.Repository;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services.Interfaces
{
    public class EnvService : IEnvService
    {
        private readonly EnvRepository _envRepository=new EnvRepository();
        public async Task<bool> AddOrUpdate(ParamEnvInfo param)
        {
            return await _envRepository.AddOrUpdate(param);
        }

        public async Task<bool> Delete(ParamEnvInfo param)
        {
            return await _envRepository.Delete(param);
        }

        public async Task<ResultPageData<RpsEnv>> GetPageList(ParamPage<object> param)
        {
            return await _envRepository.GetPageList(param);
        }
    }
}
