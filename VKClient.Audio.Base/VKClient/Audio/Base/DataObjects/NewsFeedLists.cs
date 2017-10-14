using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKClient.Audio.Base.DataObjects
{//"id":"may9","title":"9 Мая","is_hidden":true,"is_unavailable":true},
    public class NewsFeedLists
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool is_hidden { get; set; }
        public bool is_unavailable { get; set; }
    }
}
