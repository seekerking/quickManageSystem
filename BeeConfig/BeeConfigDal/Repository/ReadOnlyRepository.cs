using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeConfigModels.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BeeConfigDal.Repository
{
   public class ReadOnlyRepository
    {
        protected readonly BaseContext Context = new BaseContext();


        public async Task<ResultListData<Select2DataResponse>> GetAppSelect2DataList()
        {

          var list=  await Context.Apps.AsNoTracking().Select(x => new Select2DataResponse()
            {
                Text = x.AppName,
                Value = x.AppId
            }).ToListAsync();
            return new ResultListData<Select2DataResponse>()
            {
                Data = list
            };

        }

        public async Task<ResultListData<Select2DataResponse>> GetEnvSelect2DataList()
        {

            var list = await Context.Envs.AsNoTracking().Select(x => new Select2DataResponse()
            {
                Text = x.EnvId,
                Value = x.EnvId
            }).ToListAsync();
            return new ResultListData<Select2DataResponse>()
            {
                Data = list
            };

        }

    }
}
