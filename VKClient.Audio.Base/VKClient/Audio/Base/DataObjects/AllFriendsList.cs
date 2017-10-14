using System.Collections.Generic;
using VKClient.Common.Backend.DataObjects;

namespace VKClient.Audio.Base.DataObjects
{
    public sealed class AllFriendsList
    {
        /*
         * {"response":
         * {"items":[{"user_id":460389}],"count":1,"profiles":[{"id":460389,"first_name":"Макс","last_name":"Гутиков","city":{"id":2,"title":"Санкт-Петербург"},"country":{"id":1,"title":"Россия"},"photo_50":"https:\/\/pp.userapi.com\/c637424\/v637424389\/1abb2\/_7F9BmhjdFQ.jpg","photo_100":"https:\/\/pp.userapi.com\/c637424\/v637424389\/1abb1\/XALXqadLZxE.jpg","photo_200":"https:\/\/pp.userapi.com\/c637424\/v637424389\/1abb0\/RnkBYW_Ucjw.jpg","online":1,"university":57,"university_name":"СПбГТИ (ТУ)","faculty":799508,"faculty_name":"Факультет экономики и менеджмента","graduation":2015,"education_form":"Заочное отделение","education_status":"Студент (бакалавр)"}],"cities":[],"countries":[],"mc":null}}
         * */
        //public List<User> friends { get; set; }

        //public FriendRequests requests { get; set; }

        //
        public int count { get; set; }
        public List<User> items { get; set; }
        
        //public List<User> profiles { get; set; }
        //
        //public FriendRequests requests { get; set; }//todo:bug:empty

        public List<FriendsLists> lists { get; set; }


        //public AllFriendsList()
        //{
        //    this.requests = new FriendRequests();
        //}
    }

    //public class FriendClass
    //{
    //    public int user_id { get; set; }
    //}
    public class FriendsLists
    {
        public string name { get; set; }
        public int id { get; set; }
    }
}
