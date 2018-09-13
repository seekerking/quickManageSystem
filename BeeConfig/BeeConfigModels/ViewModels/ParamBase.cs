using System;
using System.Collections.Generic;
using System.Text;
using BeeConfigModels.ViewModels;

namespace BeeConfigModels.ViewModels
{
    public class ParamBase
    {
         
    }

    public class ParamPage<T> : ParamBase where T:class 
    {
        public int Size { get; set; }
        public int Index { get; set; }
        public T Filter { get; set; }
}
}
