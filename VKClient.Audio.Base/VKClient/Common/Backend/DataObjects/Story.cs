using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKClient.Common.Backend.DataObjects
{
    public class Story
    {
        //List<List<StoryData>>
        //List<StoryData[]>
        public int count { get; set; }
        public List<StoryData>[] items { get; set; }
        public List<User> profiles { get; set; }
        //groups


        public class StoryData
        {
            //"id":418696,"owner_id":67731396,"type":"photo","date":1507630049,"can_share":1,"can_comment":1,"preview":"
            public long id { get; set; }
            public long owner_id { get; set; }
            public string type { get; set; }
            public long date { get; set; }
            public int can_share { get; set; }
            public int can_comment { get; set; }
            public string preview { get; set; }
            public int seen { get; set; }
            public Photo photo { get; set; }
        }
    }
}
