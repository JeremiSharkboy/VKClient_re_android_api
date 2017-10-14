using System.Collections.Generic;
using VKClient.Audio.Base.DataObjects;
using System.Collections.ObjectModel;

namespace VKClient.Common.Backend.DataObjects
{
    public class NewsFeedData
    {
        /*
         * {"response":{
"lists":
[{"id":"may9","title":"9 Мая","is_hidden":true,"is_unavailable":true},
         * {"id":"hockey","title":"Хоккей","is_hidden":true,"is_unavailable":true},
         * {"id":"rio2016","title":"Рио 2016","is_hidden":true,"is_unavailable":true},
         * {"id":"1917","title":"1917","is_hidden":true},
         * {"id":"cc2017","title":"Кубок Конфедераций","is_hidden":true,"is_unavailable":true}],
"sections":[{"name":"photos","enabled":1},{"name":"videos","enabled":1},{"name":"friends","enabled":0},{"name":"groups","enabled":0},{"name":"likes","enabled":0}],

         * "feed_type":"top",
"refresh_timeout_recent":600000,
"refresh_timeout_top":600000,
"refresh_timeout_recommended":600000,
         * "items":[],
         * "profiles":[],
         * "groups":[]}}
         * */






        public List<NewsFeedLists> lists { get; set; }
        public List<NewsFeedSection> sections { get; set; }//public NewsFeedSectionsAndLists sections { get; set; }
        public string feed_type { get; set; }
        public int refresh_timeout_recent { get; set; }
        public int refresh_timeout_top { get; set; }
        public int refresh_timeout_recommended { get; set; }
        public List<NewsItem> items { get; set; }
        public List<User> profiles { get; set; }
        public List<Group> groups { get; set; }/*
        public List<Story> stories
        {
            get
            {
                return this._stories;
            }
            set
            {
                this._stories = value;
            }
        }

        private List<Story> _stories { get; set; }*/
        public Story stories { get; set; }

        public int TotalCount
        {
            get
            {
                return this.count;
            }
            set
            {
                this.count = value;
            }
        }

        public int count { get; set; }

        public int new_offset { get; set; }

        public string next_from { get; set; }

        public VKList<UserNotification> notifications { get; set; }

        public bool notifications_updated { get; set; }

        public NewsFeedType? FeedType { get; set; }

        public NewsFeedConsts consts { get; set; }

        public NewsFeedData()
        {
            this.items = new List<NewsItem>();
            this.profiles = new List<User>();
            this.groups = new List<Group>();
        }
    }
}
