using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeConfigApi.Models
{
    public class EntityPublish
    {
       
        public int Id { get; set; }
        public string EnvId { get; set; }
        public string AppId { get; set; }
        public long PublishTimespan { get; set; }

        public string Data { get; set; }
    }
}
