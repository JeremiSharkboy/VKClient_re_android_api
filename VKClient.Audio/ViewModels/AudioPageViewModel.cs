using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using VKClient.Common.Backend;
using VKClient.Common.Library;
using System.Threading;
using Newtonsoft.Json;
using VKClient.Common.Backend.DataObjects;
using VKClient.Common.Framework;
using VKClient.Common.Library.Events;
using System.IO;
using System.Windows.Media;
using System.Windows;
using VKClient.Audio.Base;
using VKClient.Audio.Base.BLExtensions;
using System.Linq.Expressions;
using System.ComponentModel;

namespace VKClient.Audio.ViewModels
{
    public class AudioPageViewModel : ViewModelBase,  IBinarySerializable, IHandle<AudioTrackDownloadProgress>
    {
        public string Title
        {
            get
            {
                return ((string)VKClient.Common.Localization.CommonResources.Audio).ToUpperInvariant();
            }
        }
        public Audios audios { get; set; }
        public Playlists playlists { get; set; }
        public CatalogViewModel Catalog { get; set; }
        //public ObservableCollection<CatalogViewModel.CatalogData> Catalog_Items { get { return this.Catalog.items; } }

        public void Notify()
        {
 //           base.NotifyPropertyChanged<ObservableCollection<CatalogViewModel.CatalogData>>(() => this.Catalog_Items);
        }


        public AudioPageViewModel()
        {
            this.audios = new Audios();
            this.playlists = new Playlists();
            this.Catalog = new CatalogViewModel();
        }

        public bool LoadFromCache()
        {
            return CacheManager.TryDeserialize((IBinarySerializable)this, "AudioPageViewModel", CacheManager.DataType.CachedData);
        }

        public void RefreshAudioSection()
        {
            Action<BackendResult<AudioPageViewModel, ResultCode>> callback = (Action<BackendResult<AudioPageViewModel, ResultCode>>)(tt =>
            {
                if (tt.ResultCode == ResultCode.Succeeded)
                {
                    this.audios = tt.ResultData.audios;
                    this.playlists = tt.ResultData.playlists;
                    base.NotifyPropertyChanged<Audios>(() => this.audios);
                    base.NotifyPropertyChanged<Playlists>(() => this.playlists);

                    CacheManager.TrySerialize(this, "AudioPageViewModel", false, CacheManager.DataType.CachedData);
                }
            });

           
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["owner_id"] = AppGlobalStateManager.Current.LoggedInUserId.ToString();
                parameters["need_owner"] = "1";
                parameters["need_playlists"] = "1";
                parameters["playlists_count"] = "12";
                parameters["audio_offset"] = "0";
                parameters["audio_count"] = "100";
                VKRequestsDispatcher.DispatchRequestToVK<AudioPageViewModel>("execute.getMusicPage", parameters, callback,
                    (Func<string, AudioPageViewModel>)(jsonStr =>
                    {
                        return JsonConvert.DeserializeObject<GenericRoot<AudioPageViewModel>>(jsonStr).response;
                    }),
                false, true, new CancellationToken?(), null);
            
        }

        public void GetAudioByOwner(long owner_id)
        {
            Action<BackendResult<AudioPageViewModel, ResultCode>> callback = (Action<BackendResult<AudioPageViewModel, ResultCode>>)(tt =>
            {
                if (tt.ResultCode == ResultCode.Succeeded)
                {
                    this.audios = tt.ResultData.audios;
                    this.playlists = tt.ResultData.playlists;
                    base.NotifyPropertyChanged<Audios>(() => this.audios);
                    base.NotifyPropertyChanged<Playlists>(() => this.playlists);
                }
            });

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["owner_id"] = owner_id.ToString();
            parameters["need_owner"] = "1";
            parameters["need_playlists"] = "1";
            parameters["playlists_count"] = "12";
            parameters["audio_offset"] = "0";
            parameters["audio_count"] = "100";
            VKRequestsDispatcher.DispatchRequestToVK<AudioPageViewModel>("execute.getMusicPage", parameters, callback,
                (Func<string, AudioPageViewModel>)(jsonStr =>
                {
                    return JsonConvert.DeserializeObject<GenericRoot<AudioPageViewModel>>(jsonStr).response;
                }),
            false, true, new CancellationToken?(), null);
        }

        public void RefreshRecomendationSection( Action<bool> call )
        {
            Action<BackendResult<CatalogViewModel, ResultCode>> callback = (Action<BackendResult<CatalogViewModel, ResultCode>>)(tt =>
            {
                if (tt.ResultCode == ResultCode.Succeeded)
                {
                    Execute.ExecuteOnUIThread(delegate
                    {
                        this.Catalog.items.Clear();
                        foreach (CatalogViewModel.CatalogData c in tt.ResultData.items)
                        {
                            this.Catalog.items.Add(c);
                        }
                    });
                    
                    //base.NotifyPropertyChanged<ObservableCollection<CatalogViewModel.CatalogData>>(() => this.Catalog.items);

                    //CacheManager.TrySerialize(this, "AudioPageViewModel", false, CacheManager.DataType.CachedData);
                }

                if (call != null)
                    call(tt.ResultCode == ResultCode.Succeeded);
            });

            //VKRequestsDispatcher.RefreshToken((a) =>
            //{
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["fields"] = "first_name_gen,photo_50,photo_100,photo_200";
                VKRequestsDispatcher.DispatchRequestToVK<CatalogViewModel>("audio.getCatalog", parameters, callback,
                    (Func<string, CatalogViewModel>)(jsonStr =>
                    {
                        return JsonConvert.DeserializeObject<GenericRoot<CatalogViewModel>>(jsonStr).response;
                    }),
                false, true, new CancellationToken?(), null);
            //});
        }

        public class CatalogViewModel
        {
            private ObservableCollection<CatalogData> _items = new ObservableCollection<CatalogData>();
            public ObservableCollection<CatalogData> items
            {
                get { return this._items; }
            }
            
            public class CatalogData
            {
                public long id { get; set; }
                public string title { get; set; }
                public string subtitle { get; set; }
                public string type { get; set; }
                public int count { get; set; }
                public string source { get; set; }
                public ObservableCollection<Audios.AudioData2> audios
                {
                    get { return this._audios; }
                }
                public ObservableCollection<Audios.ThumbData> thumbs
                {
                    get { return this._thumbs; }
                }

                private ObservableCollection<Audios.AudioData2> _audios = new ObservableCollection<Audios.AudioData2>();
                private ObservableCollection<Audios.ThumbData> _thumbs = new ObservableCollection<Audios.ThumbData>();
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.audios);
            writer.Write(this.playlists);
        }

        public void Read(BinaryReader reader)
        {
            this.audios = reader.ReadGeneric<Audios>();
            this.playlists = reader.ReadGeneric<Playlists>();
            base.NotifyPropertyChanged<Audios>(() => this.audios);
            base.NotifyPropertyChanged<Playlists>(() => this.playlists);
        }

        public class Audios : IBinarySerializable
        {
            public int count { get; set; }
            private ObservableCollection<AudioData2> _items = new ObservableCollection<AudioData2>();
            public ObservableCollection<AudioData2> items
            {
                get { return this._items; }
            }            

            public void Write(BinaryWriter writer)
            {
                writer.Write(this.count);
                writer.WriteList<AudioData2>(this._items.ToList <AudioData2>(), 10000);
            }

            public void Read(BinaryReader reader)
            {
                this.count = reader.ReadInt32();
                List<AudioData2> temp = reader.ReadList<AudioData2>();
                foreach(AudioData2 a in temp)
                {
                    this._items.Add(a);
                }
            }

            public class AudioData2 : IBinarySerializable, IHandle<AudioPlayerStateChanged>, INotifyPropertyChanged
            {
                public long id { get; set; }
                public long owner_id { get; set; }
                public string artist { get; set; }
                public string title { get; set; }
                public int duration { get; set; }
                public long date { get; set; }
                public string url { get; set; }
                public int genre_id { get; set; }



                public void Handle(AudioPlayerStateChanged message)
                {
                    this.NotifyPropertyChanged<SolidColorBrush>(() => this.TitleBrush);
                    this.NotifyPropertyChanged<SolidColorBrush>(() => this.SubtitleBrush);
                }

                public AudioData2()
                {
                    EventAggregator.Current.Subscribe(this);//todo:жрёт сильно ресурсы
                }

                public AlbumData album { get; set; }

                public bool is_licensed { get; set; }
                public bool is_hq { get; set; }
                public int track_genre_id { get; set; }
                public string access_key { get; set; }

                public string UniqueId
                {
                    get { return string.Format("{0}_{1}|{2}", owner_id,id,duration); }//owner_id.ToString() + "_" + id.ToString() + "|" + duration.ToString(); }
                }

                public double DownloadProgress { get; set; }

                public SolidColorBrush TitleBrush
                {
                    get
                    {
                        //if (this.IsCurrentTrack)
                        //    return (SolidColorBrush)Application.Current.Resources["PhoneSidebarSelectedIconBackgroundBrush"];
                        //return (SolidColorBrush)Application.Current.Resources["PhoneContrastTitleBrush"];
                        return (SolidColorBrush)Application.Current.Resources[this.IsCurrentTrack ? "PhoneSidebarSelectedIconBackgroundBrush" : "PhoneContrastTitleBrush"];
                    }
                }

                public SolidColorBrush SubtitleBrush
                {
                    get
                    {
                        if (this.IsCurrentTrack)
                            return (SolidColorBrush)Application.Current.Resources["PhoneSidebarSelectedIconBackgroundBrush"];
                        return (SolidColorBrush)Application.Current.Resources["PhoneVKSubtleBrush"];
                    }
                }

                public bool IsCurrentTrack
                {
                    get
                    {
                        try
                        {
                            Microsoft.Phone.BackgroundAudio.AudioTrack track = BGAudioPlayerWrapper.Instance.Track;
                            return track != null && track.Tag == this.UniqueId;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }

                public void Write(BinaryWriter writer)
                {
                    writer.Write(this.id);
                    writer.Write(this.owner_id);
                    writer.Write(this.artist);
                    writer.Write(this.title);
                    writer.Write(this.duration);
                    writer.Write(this.url);

                    writer.Write(this.album);
                }

                public void Read(BinaryReader reader)
                {
                    this.id = reader.ReadInt64();
                    this.owner_id = reader.ReadInt64();
                    this.artist = reader.ReadString();
                    this.title = reader.ReadString();
                    this.duration = reader.ReadInt32();
                    this.url = reader.ReadString();

                    this.album = reader.ReadGeneric<AlbumData>();
                }

                

                public event PropertyChangedEventHandler PropertyChanged;

                protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertyExpression)
                {
                    if (propertyExpression.Body.NodeType != ExpressionType.MemberAccess)
                        return;
                    this.RaisePropertyChanged((propertyExpression.Body as MemberExpression).Member.Name);
                }

                private void RaisePropertyChanged(string property)
                {
                    if (this.PropertyChanged == null)
                        return;
                    Execute.ExecuteOnUIThread((Action)(() =>
                    {
                        if (this.PropertyChanged == null)
                            return;
                        this.PropertyChanged(this, new PropertyChangedEventArgs(property));
                    }));
                }
            }

            public class ThumbData : IBinarySerializable
            {
                public string photo_34 { get; set; }
                public string photo_68 { get; set; }
                public string photo_135 { get; set; }
                public string photo_270 { get; set; }
                public string photo_300 { get; set; }
                public string photo_600 { get; set; }
                public int width { get; set; }
                public int height { get; set; }

                public void Write(BinaryWriter writer)
                {
                    writer.Write(this.photo_34);
                    writer.Write(this.photo_68);
                    writer.Write(this.photo_135);
                    writer.Write(this.photo_270);
                    writer.Write(this.photo_300);
                    writer.Write(this.photo_600);
                    writer.Write(this.width);
                    writer.Write(this.height);
                }

                public void Read(BinaryReader reader)
                {
                    this.photo_34 = reader.ReadString();
                    this.photo_68 = reader.ReadString();
                    this.photo_135 = reader.ReadString();
                    this.photo_270 = reader.ReadString();
                    this.photo_300 = reader.ReadString();
                    this.photo_600 = reader.ReadString();
                    this.width = reader.ReadInt32();
                    this.height = reader.ReadInt32();
                }
            }

            public class AlbumData : IBinarySerializable
            {
                public long id { get; set; }
                public long owner_id { get; set; }
                public string title { get; set; }
                public ThumbData thumb { get; set; }

                public void Write(BinaryWriter writer)
                {
                    writer.Write(this.id);
                    writer.Write(this.owner_id);
                    writer.Write(this.title);
                    writer.Write(this.thumb);
                }

                public void Read(BinaryReader reader)
                {
                    this.id = reader.ReadInt64();
                    this.owner_id = reader.ReadInt64();
                    this.title = reader.ReadString();

                    this.thumb = reader.ReadGeneric<ThumbData>();
                }
            }

        }

        public class Playlists : IBinarySerializable
        {
            public int count { get; set; }
            private ObservableCollection<PlaylistData> _items = new ObservableCollection<PlaylistData>();
            public ObservableCollection<PlaylistData> items
            {
                get
                {
                    return this._items;
                }
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(this.count);
                writer.WriteList<PlaylistData>(this._items.ToList<PlaylistData>(), 10000);
            }

            public void Read(BinaryReader reader)
            {
                this.count = reader.ReadInt32();
                List<PlaylistData> temp = reader.ReadList<PlaylistData>();
                foreach (PlaylistData a in temp)
                {
                    this._items.Add(a);
                }
            }

            public class PlaylistData : IBinarySerializable
            {
                public long id { get; set; }
                public long owner_id { get; set; }
                public int type { get; set; }
                public string title { get; set; }
                public string description { get; set; }
                //genres
                //artists
                public int count { get; set; }
                public bool is_following { get; set; }
                public int followers { get; set; }
                public int plays { get; set; }
                public long create_time { get; set; }
                public long update_time { get; set; }
                public AudioPageViewModel.Audios.ThumbData photo { get; set; }

                public void Write(BinaryWriter writer)
                {
                    writer.Write(this.id);
                    writer.Write(this.owner_id);
                    writer.Write(this.type);
                    writer.Write(this.title);
                    writer.Write(this.description);
                }

                public void Read(BinaryReader reader)
                {
                    this.id = reader.ReadInt64();
                    this.owner_id = reader.ReadInt64();
                    this.type = reader.ReadInt32();
                    this.title = reader.ReadString();
                    this.description = reader.ReadString();
                }
            }
        }

        public void Handle(AudioTrackDownloadProgress message)
        {
            Audios.AudioData2 ad = this.audios.items.First<Audios.AudioData2>((a) => { return a.UniqueId == message.Id; });
            if(ad!=null)
            {
                ad.DownloadProgress = message.Progress / 100.0;
                this.NotifyPropertyChanged<double>(() => ad.DownloadProgress);
            }            
        }

    }
}
