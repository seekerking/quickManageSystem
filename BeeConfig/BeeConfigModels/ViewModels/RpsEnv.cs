using System;
using System.Collections.Generic;
using System.Text;

namespace BeeConfigModels.ViewModels
{
   public class RpsEnv
    {
        public int Id { get; set; }

        public string EnvId { get; set; }

        public string EnvDesc { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
