using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.SlideCaptcha.Core.Resources
{
    public class Resource
    {
        public Resource()
        { 
            
        }

        public Resource(string type, string data)
        {
            Data = data;
            Type = type;
        }

        public string Data { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> Extras { get; set; }
    }
}
