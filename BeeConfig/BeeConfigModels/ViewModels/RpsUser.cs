using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BeeConfigModels.ViewModels
{
  public  class RpsUser
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public int Status { get; set; }
    }
}
