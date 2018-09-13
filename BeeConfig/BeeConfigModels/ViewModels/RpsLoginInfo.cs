using System;
using System.Collections.Generic;
using System.Text;

namespace BeeConfigModels.ViewModels
{
  public  class RpsLoginInfo
    {
        public bool Success { get; set; }
        
        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }
    }
}
