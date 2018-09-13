using System;
using System.Collections.Generic;
using System.Text;

namespace BeeConfigModels.ViewModels
{
   public class ParamConfigInfo
    {
        public int Id { get; set; }
        public string ConfigId { get; set; }

        public string ConfigValue { get; set; }

        public string EnvId { get; set; }

        public string AppId { get; set; }

        public string ConfigDesc { get; set; }

        public int Status { get; set; }
    }
}
