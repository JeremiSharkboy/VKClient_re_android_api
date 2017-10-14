using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VKClient.Audio.Base.BackendServices;
using VKClient.Audio.Base.DataObjects;
using VKClient.Common.Backend;
using VKClient.Common.Backend.DataObjects;
using VKClient.Common.CommonExtensions;
using VKClient.Common.Emoji;
using VKClient.Common.Framework;
using VKClient.Common.Library;
using VKClient.Common.Library.VirtItems;
using VKClient.Common.Stickers.Views;

namespace VKClient.Common.Stickers.ViewModels
{
    public class StickersStoreViewModel : ViewModelStatefulBase
    {
        private readonly List<List<StockItemHeader>> _stickersPacks = new List<List<StockItemHeader>>();
        private ObservableCollection<IVirtualizable> _stickersList = new ObservableCollection<IVirtualizable>();
        private readonly long _userOrChatId;
        private readonly bool _isChat;
        private StickersStoreViewModel.CurrentSource _stickersListSource;

        public List<StoreBannerHeader> Banners { get; set; }

        public Visibility BannersVisibility
        {
            get
            {
                return (this.Banners != null && this.Banners.Count > 0).ToVisiblity();
            }
        }

        public bool IsSlideViewCycled
        {
            get
            {
                if (this.Banners != null)
                    return this.Banners.Count > 1;
                return false;
            }
        }

        public ObservableCollection<IVirtualizable> StickersList
        {
            get
            {
                return this._stickersList;
            }
            private set
            {
                this._stickersList = value;
                this.NotifyPropertyChanged("StickersList");
            }
        }

        public StickersStoreViewModel.CurrentSource StickersListSource
        {
            get
            {
                return this._stickersListSource;
            }
            set
            {
                if (this._stickersListSource == value)
                    return;
                this._stickersListSource = value;
                this.NotifyPropertyChanged<StickersStoreViewModel.CurrentSource>((System.Linq.Expressions.Expression<Func<StickersStoreViewModel.CurrentSource>>)(() => this.StickersListSource));
                this.NotifyPropertyChanged<SolidColorBrush>((System.Linq.Expressions.Expression<Func<SolidColorBrush>>)(() => this.PopularTabForeground));
                this.NotifyPropertyChanged<SolidColorBrush>((System.Linq.Expressions.Expression<Func<SolidColorBrush>>)(() => this.NewTabForeground));
                this.NotifyPropertyChanged<SolidColorBrush>((System.Linq.Expressions.Expression<Func<SolidColorBrush>>)(() => this.FreeTabForeground));
                this.ReloadItems();
            }
        }

        public SolidColorBrush PopularTabForeground
        {
            get
            {
                return this.GetForeground(StickersStoreViewModel.CurrentSource.Popular);
            }
        }

        public SolidColorBrush NewTabForeground
        {
            get
            {
                return this.GetForeground(StickersStoreViewModel.CurrentSource.New);
            }
        }

        public SolidColorBrush FreeTabForeground
        {
            get
            {
                return this.GetForeground(StickersStoreViewModel.CurrentSource.Free);
            }
        }

        public StickersStoreViewModel(long userOrChatId, bool isChat = false)
        {
            this._userOrChatId = userOrChatId;
            this._isChat = isChat;
        }

        private SolidColorBrush GetForeground(StickersStoreViewModel.CurrentSource currentSource)
        {
            if (this._stickersListSource == currentSource)
                return (SolidColorBrush)Application.Current.Resources["PhoneBlue300_GrayBlue100Brush"];
            return (SolidColorBrush)Application.Current.Resources["PhoneGray400_Gray500Brush"];
        }

        public override void Load(Action<ResultCode> callback)
        {
            StoreService.Instance.GetStickersStoreCatalog(this._isChat ? 0L : this._userOrChatId, (Action<BackendResult<StoreCatalog, ResultCode>>)(result => Execute.ExecuteOnUIThread((Action)(() =>
            {
                StoreCatalog resultData = result.ResultData;
                if ((result.ResultCode != ResultCode.Succeeded ? false : (resultData != null ? true : false)) != false)
                {                    
                    List<StoreBanner> banners = resultData.banners;
                    if (banners != null)
                        this.Banners = banners.Select<StoreBanner, StoreBannerHeader>((Func<StoreBanner, StoreBannerHeader>)(banner => new StoreBannerHeader(banner))).ToList<StoreBannerHeader>();
                    this.NotifyPropertyChanged<List<StoreBannerHeader>>(() => this.Banners);
                    this.NotifyPropertyChanged<Visibility>(() => this.BannersVisibility);
                    this.NotifyPropertyChanged<bool>(() => this.IsSlideViewCycled);
                    this._stickersPacks.Clear();
                    List<StoreSection> sections = resultData.sections;
                    if (sections != null)
                    {
                        foreach (StoreSection storeSection in sections)
                        {/*
                            List<StockItem> stockItemList1;
                            if (storeSection == null)
                            {
                                stockItemList1 = null;
                            }
                            else
                            {
                                VKList<StockItem> stickers = storeSection.stickers;
                                stockItemList1 = stickers != null ? stickers.items : null;
                            }*/
                            //List<StockItem> stockItemList2 = stockItemList1;//todo:bug?

                            //
                          //  if (stockItemList2 != null)
                          //  {

                                List<StockItemHeader> stockItemHeaderList = new List<StockItemHeader>();
                                List<StockItem> stockItems = storeSection.stickers.items;
                                //List<StockItem> source = stockItems != null ? stockItems.items : null;
                                if (stockItems != null)
                                {
                                    foreach (StockItem stockItem1 in stockItems)
                                    {
                                        StockItemHeader stockItemHeader = new StockItemHeader(stockItem1, false, this._userOrChatId, this._isChat);
                                        stockItemHeaderList.Add(stockItemHeader);
                                        StoreProduct product = stockItemHeader.StockItem.product;
                                        //
                                        if (((IAccountStickersData)resultData).Products == null)
                                            ((IAccountStickersData)resultData).Products = new VKList<StoreProduct>();
                                        if (((IAccountStickersData)resultData).StockItems == null)
                                            ((IAccountStickersData)resultData).StockItems = new VKList<StockItem>();
                                        ((IAccountStickersData)resultData).Products.items.Add(product);
                                        ((IAccountStickersData)resultData).StockItems.items.Add(stockItem1);
                                        //
                                        if (product != null)
                                        {
                                            StockItem stockItem2 = stockItems.FirstOrDefault<StockItem>((Func<StockItem, bool>)(item =>
                                            {
                                                int? nullable = item.product != null ? new int?(item.product.id) : new int?();
                                                if (nullable.GetValueOrDefault() != product.id)
                                                    return false;
                                                return nullable.HasValue;
                                            }));
                                            if (stockItem2 != null)
                                                stockItemHeader.StockItem.CanPurchaseFor = stockItem2.CanPurchaseFor;
                                        }
                                    }
                                }
                                this._stickersPacks.Add(stockItemHeaderList);
                          //  }
                            //
                            /*
                            if (stockItemList2 != null)
                            {

                                List<StockItemHeader> stockItemHeaderList = new List<StockItemHeader>();
                                VKList<StockItem> stockItems = resultData.StockItems;
                                List<StockItem> source = stockItems != null ? stockItems.items : null;
                                if (source != null)
                                {
                                    foreach (StockItem stockItem1 in stockItemList2)
                                    {
                                        StockItemHeader stockItemHeader = new StockItemHeader(stockItem1, false, this._userOrChatId, this._isChat);
                                        stockItemHeaderList.Add(stockItemHeader);
                                        StoreProduct product = stockItemHeader.StockItem.product;
                                        if (product != null)
                                        {
                                            StockItem stockItem2 = source.FirstOrDefault<StockItem>((Func<StockItem, bool>)(item =>
                                            {
                                                int? nullable = item.product != null ? new int?(item.product.id) : new int?();
                                                if (nullable.GetValueOrDefault() != product.id)
                                                    return false;
                                                return nullable.HasValue;
                                            }));
                                            if (stockItem2 != null)
                                                stockItemHeader.StockItem.CanPurchaseFor = stockItem2.CanPurchaseFor;
                                        }
                                    }
                                }
                                this._stickersPacks.Add(stockItemHeaderList);
                            }*/
                        }
                    }
                    this.ReloadItems();
                    StickersSettings.Instance.UpdateStickersDataAndAutoSuggest((IAccountStickersData)resultData);
                    AppGlobalStateManager.Current.GlobalState.NewStoreItemsCount = 0;
                    EventAggregator.Current.Publish(new NewStoreItemsCountChangedEvent());
                }
                callback(result.ResultCode);
            }))));
        }

        private void ReloadItems()
        {
            int stickersListSource = (int)this._stickersListSource;
            if (stickersListSource < 0 || stickersListSource > this._stickersPacks.Count - 1)
                return;
            this.StickersList = new ObservableCollection<IVirtualizable>(StickersStoreViewModel.CreateVirtualizableItems((IEnumerable<StockItemHeader>)this._stickersPacks[stickersListSource]));
        }

        private static List<IVirtualizable> CreateVirtualizableItems(IEnumerable<StockItemHeader> stickers)
        {
            List<IVirtualizable> virtualizableList = new List<IVirtualizable>();
            foreach (StockItemHeader sticker1 in stickers)
            {
                UCItem ucItem = new UCItem(480.0, new Thickness(), (Func<UserControlVirtualizable>)(() =>
                {
                    return (UserControlVirtualizable)new StickersPackListItemUC()
                    {
                        DataContext = sticker1
                    };
                }), (Func<double>)(() => 100.0), null, 0.0, false);
                virtualizableList.Add((IVirtualizable)ucItem);
            }
            return virtualizableList;
        }

        public enum CurrentSource
        {
            New,
            Popular,
            Free,
        }
    }
}
