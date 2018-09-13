using System;
using System.Collections.Generic;
using System.Text;

namespace BeeConfigModels.ViewModels
{
  public  class RspApp
    {
        public  int Id { get; set; }
        public string AppId { get; set; }

        public string AppName { get; set; }

        public string AppDesc { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public string AppSecret { get; set; }
    }
}
