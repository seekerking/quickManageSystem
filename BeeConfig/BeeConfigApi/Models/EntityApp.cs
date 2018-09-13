using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeConfigApi.Models
{
    public class EntityApp
    {
        public int Id { get; set; }
        public string AppId { get; set; } 
        public Guid Secret { get; set; }
    }
}
