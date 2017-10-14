using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using VKClient.Common.Framework;
using VKClient.Audio.ViewModels;
using VKClient.Audio.Base;
using VKClient.Common.Library;
using VKClient.Common.Backend.DataObjects;
using VKClient.Audio.Base.BLExtensions;
using System.Collections;
using VKClient.Common.AudioManager;
using VKClient.Audio.Library;
using VKClient.Audio.Base.Library;

namespace VKClient.Audio
{
    public partial class AudioPage2 : PageBase
    {
        private bool _isInitialized;
        private bool _catalog_loaded;

        public AudioPage2()
        {
            InitializeComponent();
            this.ucPullToRefresh.TrackListBox(this.allTracks);
            this.allTracks.OnRefresh = delegate
            {
                (base.DataContext as AudioPageViewModel).RefreshAudioSection();
            };

            this.ucHeader.Tap += ucHeader_Tap;
        }

        void ucHeader_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            (base.DataContext as AudioPageViewModel).RefreshAudioSection();
        }

        private void mainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot p = sender as Pivot;
            if(p.SelectedIndex==1)
            {
                if (!this._catalog_loaded)
                {
                    (base.DataContext as AudioPageViewModel).RefreshRecomendationSection();
                    _catalog_loaded = true;
                }
                /*
                AudioPageViewModel m = this.DataContext as AudioPageViewModel;
                AudioPageViewModel.CatalogViewModel.CatalogData c = new AudioPageViewModel.CatalogViewModel.CatalogData();
                c.title = "TEST";

                AudioPageViewModel.Audios.AudioData2 a = new AudioPageViewModel.Audios.AudioData2();
                a.title = "tit";
                a.artist = "artist";

                c.audios.Add(a);
                m.Catalog.items.Add(c);*/
//                m.Notify();
            }
            
            
            
            //m.Catalog.items
            /*
            AudioPageViewModel.Playlists.PlaylistData d = new AudioPageViewModel.Playlists.PlaylistData();
            d.id = 444;
            d.title = "LOL";
            d.description = "Testtttdsfsdfsdf";
            m.playlists.items.Add(d);*/
            /*
            AudioPageViewModel.Audios.AudioData a = new AudioPageViewModel.Audios.AudioData();
            a.title = "sdsdsd";
            m.audios.items.Add(a);*/
        }

        private void Initialize()
        {
            this.DataContext = new AudioPageViewModel();
            if(!(base.DataContext as AudioPageViewModel).LoadFromCache())
            {
                (base.DataContext as AudioPageViewModel).RefreshAudioSection();
            }
        }

        protected override void HandleOnNavigatedTo(NavigationEventArgs e)
        {
            base.HandleOnNavigatedTo(e);
            if (!this._isInitialized)
            {
                this.Initialize();
                this._isInitialized = true;
            }
        }

        private void allTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExtendedLongListSelector longListSelector = sender as ExtendedLongListSelector;
            if (longListSelector != null)
            {
                AudioPageViewModel.Audios.AudioData2 selectedItem = longListSelector.SelectedItem as AudioPageViewModel.Audios.AudioData2;
                if (selectedItem == null)
                    return;

                this.TrackAction(selectedItem);
            }
            longListSelector.SelectedItem = null;
        }

        private void TrackAction(AudioPageViewModel.Audios.AudioData2 item)
        {
            if (BGAudioPlayerWrapper.Instance.Track != null && item.UniqueId == BGAudioPlayerWrapper.Instance.Track.Tag)
            {
                BGAudioPlayerWrapper.Instance.Pause();
            }
            else
            {
                AudioObj a = new AudioObj();
                a.duration = item.duration.ToString();
                a.artist = item.artist;
                a.title = item.title;
                a.url = item.url;
                a.id = item.id;
                a.owner_id = item.owner_id;
                BGAudioPlayerWrapper.Instance.Track = AudioTrackHelper.CreateTrack(a);
                BGAudioPlayerWrapper.Instance.Play();






                //
                List<AudioObj> tracks = new List<AudioObj>();

                foreach (AudioPageViewModel.Audios.AudioData2 adata in (base.DataContext as AudioPageViewModel).audios.items)
                {
                    AudioObj o = new AudioObj();
                    //AudioHeader ah = new AudioHeader(o);
                    o.artist = adata.artist;
                    o.duration = adata.duration.ToString();
                    o.id = adata.id;
                    o.owner_id = adata.owner_id;
                    o.title = adata.title;
                    o.url = adata.url;
                    tracks.Add(o);
                }

                PlaylistManager.SetAudioAgentPlaylist(tracks, CurrentMediaSource.AudioSource);
                AudioHeader track = new AudioHeader(a);
                if (!track.TryAssignTrack())
                    return;
                //if (need_navigate)
                //    Navigator.Current.NavigateToAudioPlayer(true);
                //else
                //    BGAudioPlayerWrapper.Instance.Play();
                //
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox box = sender as ListBox;
            if (box != null)
            {
                AudioPageViewModel.Audios.AudioData2 selectedItem = box.SelectedItem as AudioPageViewModel.Audios.AudioData2;
                if (selectedItem == null)
                    return;

                this.TrackAction(selectedItem);
            }
        }
    }
}