using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeConfigApi.Models
{
    public class EntityConfig
    {
        public int Id { get; set; }
        public string ConfigId { get; set; }
        public string ConfigValue { get; set; }
        public string EnvId { get; set; }
        public string AppId { get; set; }
        public long LastTimespan { get; set; } 
    }
}
