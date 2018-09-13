using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeeConfigModels.ViewModels;

namespace BeeConfigAdmin.Extension
{
    public static class EnumrableExtension
    {
        public static IEnumerable<Select2DataResponse> Convet2DataResponse<T>(this IEnumerable<T> source,
            Func<T, string> key, Func<T, string> text)
        {
          return  source.Select(x => new Select2DataResponse()
            {
                Text = text(x),
                Value = key(x)
            });
        }
    }
}
