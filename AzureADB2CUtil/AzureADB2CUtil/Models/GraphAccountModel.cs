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
        public string Id { get; set; }
        public IEnumerable<string> OtherMails { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }

        public static GraphAccountModel Parse(string JSON)
        {
            return JsonConvert.DeserializeObject(JSON, typeof(GraphAccountModel)) as GraphAccountModel;
        }
    }
}
