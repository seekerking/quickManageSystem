using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeConfigApi.Models
{
    public class ParamConfig
    {
        public string AppId { get; set; }
        public string Env { get; set; }
        public long Lastupdate { get; set; }
        public string Sign { get; set; }

        public string Current { get; set; }

        public bool IsValidate(string appSign)
        {
            if (string.IsNullOrWhiteSpace(AppId)
               || string.IsNullOrWhiteSpace(Env)
               || string.IsNullOrWhiteSpace(Sign)
               || string.IsNullOrWhiteSpace(Current))
                return false;

            var sign = BeeUtils.GetMD5($"{AppId}_{Env}_{Lastupdate}_{appSign}_{Current}");
            return sign.Equals(Sign);
        }
    }
}
