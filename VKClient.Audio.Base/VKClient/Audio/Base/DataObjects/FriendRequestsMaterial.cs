using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VKClient.Common.Backend.DataObjects;

namespace VKClient.Audio.Base.DataObjects
{
    public class FriendRequestsMaterial
    {
        public int count { get; set; }
        public List<FriendRequestItem> items { get; set; }
        public List<User> profiles { get; set; }

        public class FriendRequestItem
        {
            public string message { get; set; }
            public long user_id { get; set; }
        }
    }
}
