using System.Collections.Generic;

namespace VKClient.Common.Backend.DataObjects
{
    public sealed class AccountBaseInfo
    {
        /*
         * "info":{
         * "country":"RU",
         * "https_required":1,
         * "intro":0,
         * "lang":0,
         * "support_url":"https:\/\/m.vk.com\/support?api_view=2f62d565ec908813906270bf53dd60&lang=ru",
         * "money_p2p_params":{"min_amount":100,"max_amount":75000,"currency":"RUB"},
         * "audio_ads":{"day_limit":100,"track_limit":1,"types_allowed":["preroll"],"sections":["my","user_playlists","group_playlists","my_playlists","recent","audio_feed","recs","recs_audio","recs_album","search","global_search","group_list","user_list","user_wall","group_wall","feed","other"]},
         * "profiler_settings":{"api_requests":true,"download_patterns":[{"type":"story_video_antivsplesk","pattern":"^https:\\\/\\\/(story\\d)\\.(vk\\.me|vk\\-cdn\\.net|userapi\\.com|vkuservideo\\.net|vkuservideo\\.com).*\\.mp4.*$","probability":0.010000,"error_probability":1.000000},{"type":"photo","pattern":"^https:\\\/\\\/(?:cs\\d+|pp|ppv4|sun\\d+-\\d+)\\.(?:vk\\.me|userapi\\.com)\\\/(c\\d+)\\\/.*\\.jpg.*$","probability":0.010000,"error_probability":1.000000},{"type":"hls_chunk","pattern":"^https:\\\/\\\/(cs[0-9\\-v]+)\\.(vk\\.me|vk\\-cdn\\.net|userapi\\.com|vkuservideo\\.net|vkuservideo\\.com).*\\.ts.*$","probability":0.010000,"error_probability":1.000000},{"type":"hls_playlist","pattern":"^https:\\\/\\\/(cs[0-9\\-v]+)\\.(vk\\.me|vk\\-cdn\\.net|userapi\\.com|vkuservideo\\.net|vkuservideo\\.com).*\\.m3u8.*$","probability":0.010000,"error_probability":1.000000},{"type":"mp4","pattern":"^https:\\\/\\\/(cs[0-9\\-v]+)\\.(vk\\.me|vk\\-cdn\\.net|userapi\\.com|vkuservideo\\.net|vkuservideo\\.com).*\\.mp4.*$","probability":0.010000,"error_probability":1.000000},{"type":"hls_live_chunk","pattern":"^https:\\\/\\\/(live\\d+)\\.vkuserlive\\.com\\\/\\d+\\\/+(live|abr|liveabr)\\\/[-_a-zA-Z0-9]+\\\/(abr\\\/[-_a-zA-Z0-9]+_(source|\\d+p)\\\/)?l_\\d+_\\d+_\\d+\\.ts$","probability":0.050000,"error_probability":1.000000},{"type":"hls_live_playlist","pattern":"^https:\\\/\\\/(live\\d+)\\.vkuserlive\\.com\\\/\\d+\\\/+(live|abr|liveabr)\\\/[-_a-zA-Z0-9]+\\\/(abr\\\/[-_a-zA-Z0-9]+_(source|\\d+p)\\\/)?(playlist|chunks)\\.m3u8$","probability":0.050000,"error_probability":1.000000},{"type":"mp3","pattern":"^https:\\\/\\\/([cs0-9\\-pv]+)\\.(vk\\.me|vk\\-cdn\\.net|userapi\\.com|vkuservideo\\.net|vkuservideo\\.com).*\\.mp3.*$","probability":0.010000,"error_probability":1.000000}]},"raise_to_record_enabled":true,"music_intro":true,"settings":[{"name":"audio_ads","available":true},{"name":"audio_background_limit","available":true,"value":"60"},{"name":"gif_autoplay","available":true},{"name":"money_clubs_p2p","available":true},{"name":"money_p2p","available":true,"value":"can_send"},{"name":"audio_restrictions","available":true},{"name":"stories","available":true},{"name":"masks","available":true},{"name":"video_autoplay","available":true},{"name":"vklive_app","available":true}],"community_comments":false},
         * */

        public string country { get; set; }
        public int https_required { get; set; }
        public int intro { get; set; }
        public int lang { get; set; }

        public string support_url { get; set; }

        public MoneyTransfersSettings money_p2p_params { get; set; }

        public Audio_ads audio_ads { get; set; }

        public List<AccountBaseInfoSettingsEntry> settings { get; set; }
        public AccountProfileSettings profiler_settings { get; set; }

        public bool community_comments { get; set; }
        public bool music_intro { get; set; }
        public bool raise_to_record_enabled { get; set; }
    }
}
