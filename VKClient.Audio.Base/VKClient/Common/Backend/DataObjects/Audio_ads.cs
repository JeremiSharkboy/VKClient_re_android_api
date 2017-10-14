using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKClient.Common.Backend.DataObjects
{
    public sealed class Audio_ads
    {
        /*audio_ads":{
         * "day_limit":100,
         * "track_limit":1,
         * "types_allowed":["preroll"],
         * "sections":["my","user_playlists","group_playlists","my_playlists","recent","audio_feed","recs","recs_audio","recs_album","search","global_search","group_list","user_list","user_wall","group_wall","feed","other"]},
    
         * 
         * */
        public int day_limit { get; set; }
        public int track_limit { get; set; }
        public List<string> types_allowed { get; set; }
        public List<string> sections { get; set; }
     }
}
