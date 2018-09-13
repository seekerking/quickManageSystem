using System;
using System.Collections.Generic;
using System.Text;

namespace BeeConfigModels.ViewModels
{
   public class RpsReqLog
    {
        public int Id { get; set; }

        public string ClientIp { get; set; }

        public string AppId { get; set; }

        public string AppEnv { get; set; }

        public DateTime FirstDate { get; set; }

        public DateTime LastDate { get; set; }

        public int ReqTimes { get; set; }

        public DateTime LastConfigDate { get; set; }
    }
}
