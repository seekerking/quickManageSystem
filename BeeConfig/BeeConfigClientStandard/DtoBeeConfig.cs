using System;
using System.Collections.Generic;
using System.Text;

namespace BeeConfigClientStandard
{
    public class DtoBeeConfig
    {
        public string BeeConfigAppId { get; set; }
        public string BeeConfigEnvironment { get; set; }
        public long BeeConfigLastUpdate { get; set; }
        public Dictionary<string, string> BeeConfigData { get; set; }
    }
}
