using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeeConfigDal.Repository;
using BeeConfigDal.Services.Interfaces;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services
{
   public class ReadOnlyService:IReadOnlyService
    {
        private readonly ReadOnlyRepository _readOnlyRepository=new ReadOnlyRepository();

        public async Task<ResultListData<Select2DataResponse>> GetAppSelect2DataList()
        {
            return await _readOnlyRepository.GetAppSelect2DataList();
        }

        public async Task<ResultListData<Select2DataResponse>> GetEnvSelect2DataList()
        {
            return await _readOnlyRepository.GetEnvSelect2DataList();
        }
    }
}
