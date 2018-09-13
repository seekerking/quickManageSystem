using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeConfigApi.Models
{
    public class EntityReq
    {
        public string ClientIp { get; set; }
        public string AppId { get; set; }
        public string AppEnv { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; } 
        public DateTime LastConfigDate { get; set; }
    }
}
