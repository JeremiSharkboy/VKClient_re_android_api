using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VKClient.Audio.Base.DataObjects;
using VKClient.Common.Backend.DataObjects;
using VKClient.Common.Localization;
using VKClient.Common.UC;

namespace VKClient.Common.Library
{
    public class NewsSources
    {
        public static PickableNewsfeedSourceItemViewModel NewsFeed = new PickableNewsfeedSourceItemViewModel(new PickableItem() { ID = -10L, Name = CommonResources.NEWS.ToLowerInvariant(), ImageSource = new Uri("/Resources/NotUsed", UriKind.Relative) });
        public static PickableNewsfeedSourceItemViewModel Suggestions = new PickableNewsfeedSourceItemViewModel(new PickableItem() { ID = -9L, Name = CommonResources.NewsFeedSuggestions, Alias = CommonResources.NewsFeedSuggestions });
        public static PickableNewsfeedSourceItemViewModel Photos = new PickableNewsfeedSourceItemViewModel(new PickableItem() { ID = -101L, Name = CommonResources.MainMenu_Photos.ToLowerInvariant(), Alias = CommonResources.MainMenu_Photos.ToLowerInvariant() });
        public static PickableNewsfeedSourceItemViewModel Videos = new PickableNewsfeedSourceItemViewModel(new PickableItem() { ID = -100L, Name = CommonResources.MainMenu_Videos.ToLowerInvariant(), Alias = CommonResources.MainMenu_Videos.ToLowerInvariant() });
        public static PickableNewsfeedSourceItemViewModel Friends = new PickableNewsfeedSourceItemViewModel(new PickableItem() { ID = -8L, Name = CommonResources.FriendsLowCase, Alias = CommonResources.FriendsLowCase });
        public static PickableNewsfeedSourceItemViewModel Groups = new PickableNewsfeedSourceItemViewModel(new PickableItem() { ID = -7L, Name = CommonResources.Groups.ToLowerInvariant(), Alias = CommonResources.Groups });

        public static ObservableCollection<PickableNewsfeedSourceItemViewModel> GetPredefinedNewsSources()
        {
            ObservableCollection<PickableNewsfeedSourceItemViewModel> observableCollection = new ObservableCollection<PickableNewsfeedSourceItemViewModel>();
            observableCollection.Add(NewsSources.NewsFeed);
            observableCollection.Add(NewsSources.Suggestions);
            return observableCollection;
        }

        public static ObservableCollection<PickableNewsfeedSourceItemViewModel> GetAllPredefinedNewsSources()
        {
            ObservableCollection<PickableNewsfeedSourceItemViewModel> observableCollection = new ObservableCollection<PickableNewsfeedSourceItemViewModel>();
            
            observableCollection.Add(NewsSources.NewsFeed);
            observableCollection.Add(NewsSources.Suggestions);
            observableCollection.Add(NewsSources.Friends);
            observableCollection.Add(NewsSources.Photos);
            observableCollection.Add(NewsSources.Videos);
            return observableCollection;
        }

        public static ObservableCollection<PickableNewsfeedSourceItemViewModel> GetNewsSources(NewsFeedSectionsAndLists sectionsAndLists, PickableItem currentNewsSource = null)
        {
            ObservableCollection<PickableNewsfeedSourceItemViewModel> predefinedNewsSources = NewsSources.GetPredefinedNewsSources();
            if (currentNewsSource != null)
            {
                switch ((NewsSourcesPredefined)currentNewsSource.ID)
                {
                    case NewsSourcesPredefined.Friends:
                        predefinedNewsSources.Add(NewsSources.Friends);
                        break;
                    case NewsSourcesPredefined.Communities:
                        predefinedNewsSources.Add(NewsSources.Groups);
                        break;
                    case NewsSourcesPredefined.Photo:
                        predefinedNewsSources.Add(NewsSources.Photos);
                        break;
                    case NewsSourcesPredefined.Video:
                        predefinedNewsSources.Add(NewsSources.Videos);
                        break;
                }
            }
            List<NewsFeedSection> newsFeedSectionList = (sectionsAndLists != null ? sectionsAndLists.sections : null) ?? new List<NewsFeedSection>();
            if (newsFeedSectionList != null)
            {
                foreach (NewsFeedSection newsFeedSection in newsFeedSectionList)
                {
                    if (newsFeedSection.enabled == 1)
                    {
                        string name = newsFeedSection.name;
                        
                        if(name == "photos" && !predefinedNewsSources.Contains(NewsSources.Photos))
                        {
                            predefinedNewsSources.Insert(Math.Min(2, predefinedNewsSources.Count), NewsSources.Photos);
                        }
                        if (name == "videos" && !predefinedNewsSources.Contains(NewsSources.Videos))
                        {
                            predefinedNewsSources.Insert(Math.Min(3, predefinedNewsSources.Count), NewsSources.Videos);
                        }
                        if (name == "friends" && !predefinedNewsSources.Contains(NewsSources.Friends))
                        {
                            predefinedNewsSources.Insert(Math.Min(4, predefinedNewsSources.Count), NewsSources.Friends);
                        }
                        if (name == "groups" && !predefinedNewsSources.Contains(NewsSources.Groups))
                        {
                            predefinedNewsSources.Insert(Math.Min(5, predefinedNewsSources.Count), NewsSources.Groups);//todo:4?
                        }
                    }
                }
            }
            List<NewsFeedLists> newsFeedListList = (sectionsAndLists != null ? sectionsAndLists.lists : null) ?? new List<NewsFeedLists>();
            if (newsFeedListList != null)
            {
                foreach (NewsFeedLists newsFeedList in newsFeedListList)
                {
                    if (newsFeedList.is_unavailable || newsFeedList.is_hidden)
                    {
                        continue;
                    }
                    predefinedNewsSources.Add(new PickableNewsfeedSourceItemViewModel(new PickableItem()
                    {
                        //ID = long.Parse(newsFeedList.id),//todo:bug
                        Name = newsFeedList.title,
                        Alias = newsFeedList.title
                    }));
                }
            }
            return predefinedNewsSources;
        }
    }
}
