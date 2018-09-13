using System;
using System.Collections.Generic;
using System.Text;

namespace BeeConfigModels.ViewModels
{
   public class ParamUserInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string Pwd { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
