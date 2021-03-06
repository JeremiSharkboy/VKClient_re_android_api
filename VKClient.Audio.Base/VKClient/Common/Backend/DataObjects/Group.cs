using System.Collections.Generic;
using System.IO;
using VKClient.Audio.Base.DataObjects;
using VKClient.Common.Framework;

namespace VKClient.Common.Backend.DataObjects
{/*
    public class StatusGroup
    {
        public string text { get; set; }//todo:bug?move fields?
    }
    */
    public class Group : IProfile, IBinarySerializable, IProfileData
    {
        private string _description = "";
        private Counters _counters = new Counters();
        private string _name;
        private string _type;

        public long id { get; set; }

        public string activity { get; set; }

        public string status { get; set; }

        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = (value ?? "").ForUI();
            }
        }

        public string first_name_gen
        {
            get
            {
                return this.name;
            }
        }

        public string first_name
        {
            get
            {
                return this.name;
            }
        }

        public string deactivated { get; set; }

        public string link { get; set; }

        public string description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = (value ?? "").ForUI();
            }
        }

        public string wiki_page { get; set; }

        public int members_count { get; set; }

        public Counters counters
        {
            get
            {
                return this._counters;
            }
            set
            {
                this._counters = value;
            }
        }

        public Place place { get; set; }

        public string screen_name { get; set; }

        public string type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
                if (!string.IsNullOrEmpty(this._type))
                {
                    string type = this._type;
                    if (!(type == "group"))
                    {
                        if (!(type == "page"))
                        {
                            if (type == "event")
                            {
                                this.GroupType = GroupType.Event;
                                return;
                            }
                        }
                        else
                        {
                            this.GroupType = GroupType.PublicPage;
                            return;
                        }
                    }
                    else
                    {
                        this.GroupType = GroupType.Group;
                        return;
                    }
                }
                this.GroupType = GroupType.Group;
            }
        }

        public string photo
        {
            get
            {
                return this.photo_50;
            }
            set
            {
                this.photo_50 = value;
            }
        }

        public string photo_medium
        {
            get
            {
                return this.photo_100;
            }
            set
            {
                this.photo_100 = value;
            }
        }

        public string photo_big
        {
            get
            {
                return this.photo_200;
            }
            set
            {
                this.photo_200 = value;
            }
        }

        public string photo_50 { get; set; }

        public string photo_100 { get; set; }

        public string photo_200 { get; set; }

        public string photo_max { get; set; }

        public int start_date { get; set; }

        public int finish_date { get; set; }

        public long invited_by { get; set; }

        public int fixed_post { get; set; }

        public int SuggestedPostsCount { get; set; }

        public int PostponedPostsCount { get; set; }

        public GroupMembershipType MembershipType { get; set; }

        public GroupType GroupType { get; private set; }

        public int is_subscribed { get; set; }

        public bool IsSubscribed
        {
            get
            {
                return this.is_subscribed == 1;
            }
            set
            {
                this.is_subscribed = value ? 1 : 0;
            }
        }

        public int is_favorite { get; set; }

        public bool IsFavorite
        {
            get
            {
                return this.is_favorite == 1;
            }
            set
            {
                this.is_favorite = value ? 1 : 0;
            }
        }

        public int verified { get; set; }

        public bool IsVerified
        {
            get
            {
                return this.verified == 1;
            }
            set
            {
                this.verified = value ? 1 : 0;
            }
        }

        public int can_post { get; set; }

        public bool CanPost
        {
            get
            {
                return this.can_post == 1;
            }
            set
            {
                this.can_post = value ? 1 : 0;
            }
        }

        public int can_see_all_posts { get; set; }

        public bool CanSeeAllPosts
        {
            get
            {
                return this.can_see_all_posts == 1;
            }
            set
            {
                this.can_see_all_posts = value ? 1 : 0;
            }
        }

        public int can_see_gifts
        {
            get
            {
                return 0;
            }
        }

        public int is_closed { get; set; }

        public GroupPrivacy Privacy
        {
            get
            {
                return (GroupPrivacy)this.is_closed;
            }
            set
            {
                this.is_closed = (int)value;
            }
        }

        public bool CanJoin
        {
            get
            {
                if ((this.MembershipType != GroupMembershipType.Member && this.MembershipType != GroupMembershipType.NotSure && this.Privacy != GroupPrivacy.Private || this.MembershipType == GroupMembershipType.InvitationReceived) && (this.ban_info == null || this.ban_info.end_date > 0L))
                    return string.IsNullOrEmpty(this.deactivated);
                return false;
            }
        }

        private int is_admin { get; set; }

        public int admin_level { get; set; }

        public int is_member { get; set; }

        public bool IsMember
        {
            get
            {
                return this.is_member == 1;
            }
            set
            {
                this.is_member = value ? 1 : 0;
            }
        }

        public int member_status { get; set; }

        public int can_upload_video { get; set; }

        public bool CanUploadVideo
        {
            get
            {
                return this.can_upload_video == 1;
            }
            set
            {
                this.can_upload_video = value ? 1 : 0;
            }
        }

        public int can_create_topic { get; set; }

        public bool CanCreateTopic
        {
            get
            {
                return this.can_create_topic == 1;
            }
            set
            {
                this.can_create_topic = value ? 1 : 0;
            }
        }

        public int can_message { get; set; }

        public int is_messages_blocked { get; set; }

        public BanInfo ban_info { get; set; }

        public string site { get; set; }

        public List<GroupLink> links { get; set; }

        public List<GroupContact> contacts { get; set; }

        public City city { get; set; }

        public Country country { get; set; }

        public long main_album_id { get; set; }

        public int main_section { get; set; }

        public ProfileMainSectionType MainSection { get; set; }

        public Market market { get; set; }

        public AppButton app_button { get; set; }

        public Cover cover { get; set; }

        public void Write(BinaryWriter writer)
        {
            writer.Write(1);
            writer.Write(this.id);
            writer.WriteString(this.name);
            writer.WriteString(this.photo_200);
        }

        public void Read(BinaryReader reader)
        {
            reader.ReadInt32();
            this.id = reader.ReadInt64();
            this.name = reader.ReadString();
            this.photo_200 = reader.ReadString();
        }













        public string Activity
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        public int AdminLevel
        {
            get
            {
                return admin_level;
            }
        }

        public int areFriendsStatus
        {
            get
            {
                return 0;
            }
        }

        public List<AudioObj> audios { get; set; }

        public MediaSectionsSettings mediaSectionsSettings { get; set; }
        public bool CanAddAudios
        {
            get
            {
                if (this.mediaSectionsSettings != null)
                    return this.CanAddMediaData(this.mediaSectionsSettings.audio);
                return false;
            }
        }
        private bool CanAddMediaData(int sectionSettings)
        {
            if (sectionSettings > 1)
                return true;
            if (sectionSettings > 0)
                return this.AdminLevel > 1;
            return false;
        }

        public bool CanAddDocs
        {
            get
            {
                if (this.mediaSectionsSettings != null)
                    return this.CanAddMediaData(this.mediaSectionsSettings.docs);
                return false;
            }
        }

        public bool CanAddPhotos
        {
            get
            {
                if (this.mediaSectionsSettings != null)
                    return this.CanAddMediaData(this.mediaSectionsSettings.photos);
                return false;
            }
        }

        public bool CanAddProducts
        {
            get
            {
                return false;
            }
        }

        public bool CanAddTopics
        {
            get
            {
                if (this.mediaSectionsSettings != null)
                    return this.CanAddMediaData(this.mediaSectionsSettings.topics);
                return false;
            }
        }

        public bool CanAddVideos
        {
            get
            {
                if (this.mediaSectionsSettings != null)
                    return this.CanAddMediaData(this.mediaSectionsSettings.video);
                return false;
            }
        }

        public bool CanSuggestAPost
        {
            get
            {
                if (!this.CanPost)
                    return this.GroupType == GroupType.PublicPage;
                return false;
            }
        }

        public string FirstName
        {
            get
            {
                return this.name;
            }
        }

        public VKList<User> followers { get; set; }

        public VKList<GiftItemData> gifts { get; set; }

        public long Id
        {
            get
            {
                return this.id;
            }
        }

        public VKList<Group> invites { get; set; }

        public bool IsDeactivated
        {
            get
            {
                return !string.IsNullOrEmpty(this.deactivated);
            }
        }

        public bool isMarketMainAlbumEmpty { get; set; }

        public Doc lastDoc { get; set; }

        public Photo lastPhoto { get; set; }

        public Album mainAlbum { get; set; }

        public ProfileMainSectionType MainSectionType
        {
            get
            {
                return this.MainSection;
            }
        }

        public string Name
        {
            get
            {
                
                return this.name;
            }
        }

        public string NameGen
        {
            get
            {
                return "";
            }
        }

        public string PhotoMax
        {
            get
            {
                return this.photo_max ?? this.photo_200;
            }
            set
            {
                this.photo_max = value;
            }
        }

        public VKList<Photo> photos { get; set; }

        public int postponedPostsCount { get; set; }

        public List<Topic> topics { get; set; }

        public List<Video> videos { get; set; }

        public VKList<Product> goods { get; set; }//public List<Product> products { get; set; }

        public bool ShowAllPostsByDefault
        {
            get
            {
                return true;
            }
        }

        public VKList<SubscriptionObj> subscriptions { get; set; }

        public int suggestedPostsCount { get; set; }

        public List<User> friends { get; set; }//todo:need this?
    }
}
