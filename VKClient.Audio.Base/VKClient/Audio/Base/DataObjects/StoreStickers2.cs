using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace VKClient.Audio.Base.DataObjects
{
    public class StoreStickers2
    {
        public int count { get; set; }
        public List<Sticker2> items { get; set; }

        public class Sticker2
        {
            public StoreProduct2 product { get; set; }
            public string description { get; set; }
            public string author { get; set; }
            public int can_purchase { get; set; }
            public int free { get; set; }
            public string payment_type { get; set; }
            public int price { get; set; }
            public string price_str { get; set; }
            public string merchant_product_id { get; set; }
            public string photo_35 { get; set; }
            public string photo_70 { get; set; }
            public string photo_140 { get; set; }
            public string photo_296 { get; set; }
            public string photo_592 { get; set; }
            public string background { get; set; }
            List<string> demo_photos_560 { get; set; }
        }

        public class StoreProduct2
        {
            public int id { get; set; }
            public string type { get; set; }
            public int purchased { get; set; }
            public int active { get; set; }
            public int purchase_date { get; set; }
            public string title { get; set; }
            public string base_url { get; set; }
            public StoreStickers stickers { get; set; }
        }
    }
}
