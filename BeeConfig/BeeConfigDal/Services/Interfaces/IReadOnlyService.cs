using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BeeConfigModels.ViewModels;

namespace BeeConfigDal.Services.Interfaces
{
   public interface IReadOnlyService
   {
        Task<ResultListData<Select2DataResponse>> GetEnvSelect2DataList();
        Task<ResultListData<Select2DataResponse>> GetAppSelect2DataList();
   }
}
