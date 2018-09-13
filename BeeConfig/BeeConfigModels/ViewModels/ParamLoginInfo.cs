using System;
using System.Collections.Generic;
using System.Text;

namespace BeeConfigModels.ViewModels
{
  public  class ParamLoginInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

        public string Name { get; set; }
        public bool IsPersistent { get; set; }
    }
}
