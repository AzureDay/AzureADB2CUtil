using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADB2C.GraphService
{
    public class GraphAccountModel
    {
        public string id { get; set; }
        public string mail { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }

        public static GraphAccountModel Parse(string JSON)
        {
            return JsonConvert.DeserializeObject(JSON, typeof(GraphAccountModel)) as GraphAccountModel;
        }
    }
}
