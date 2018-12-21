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
        public IEnumerable<string> otherMails { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
        public string city { get; set; }
        public string extension_d6245cc8578e4908b91662ccd12132e2_Company { get; set; }
        public string jobTitle { get; set; }
        
        public static GraphAccountModel Parse(string JSON)
        {
            return JsonConvert.DeserializeObject(JSON, typeof(GraphAccountModel)) as GraphAccountModel;
        }
    }
}
