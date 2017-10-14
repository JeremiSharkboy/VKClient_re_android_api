using System;
using System.Diagnostics;
using System.Windows;
using VKClient.Common.Library.VirtItems;

using System.Windows.Media;
using VKClient.Common.Library;

namespace VKMessenger.Views
{
    public class ConversationHeaderShareUC : UserControlVirtualizable
    {
        private bool _contentLoaded;
        //
        internal RectangleGeometry rectangleGeometry;

        public bool IsLookup { get; set; }

        public ConversationHeaderShareUC()
        {
            this.InitializeComponent();
            //
            //this.rectangleGeometry.RadiusX = this.rectangleGeometry.RadiusY = AppGlobalStateManager.Current.GlobalState.UserAvatarRadius * this.rectangleGeometry.Rect.Width / 10.0 / 2.0;
        }

        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent(this, new Uri("/VKMessenger;component/Views/ConversationHeaderShareUC.xaml", UriKind.Relative));
            //
            this.rectangleGeometry = (RectangleGeometry)base.FindName("rectangleGeometry");
        }
    }
}
