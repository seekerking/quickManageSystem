using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BeeConfigModels.ViewModels
{
  public  class RpsConfig
    {
        public int Id { get; set; }

        public string ConfigId { get; set; }

        public string ConfigValue { get; set; }

        public string ConfigDesc { get; set; }
        public string AppId { get; set; }

        public string AppName { get; set; }
        public string EnvId { get; set; }
        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }

        [JsonIgnore]
        public int Status { get; set; }

        [JsonIgnore]
        public long LastTimeSpan { get; set; }
    }
}
