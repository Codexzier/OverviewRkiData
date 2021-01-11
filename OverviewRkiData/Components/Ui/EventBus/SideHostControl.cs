
using OverviewRkiData.Components.Ui.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OverviewRkiData.Components.Ui.EventBus
{
    public class SideHostControl : Control
    {
        public static IList<SideHostTypeChannel> TypeViews = new List<SideHostTypeChannel>();

        public static bool IsViewOpen(Type view, int channel) => TypeViews.Any(a => a.Channel == channel && a.TypeView == view);
        
        private ContentPresenter _presenter;

        public int Channel { get; set; }
        
        static SideHostControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(SideHostControl), new FrameworkPropertyMetadata(typeof(SideHostControl)));

        public SideHostControl()
        {
            EventBusManager.OpenViewEvent += this._eventBus_OpenViewEvent;
            EventBusManager.CloseViewEvent += this.EventBusManager_CloseViewEvent;
        }

        public override void OnApplyTemplate() => this._presenter = this.GetContentPresenter<ContentPresenter>();


        private void _eventBus_OpenViewEvent(object obj, int channel)
        {
            if (channel != this.Channel)
            {
                return;
            }

            if (this._presenter == null)
            {
                return;
            }

            if (this._presenter.Content != null &&
               this._presenter.Content is UserControl disposable)
            {
                var t = this._presenter.Content.GetType();
                EventBusManager.Deregister(t);

                //disposable.Dispose();
                this.RemoveViewFromChannel(disposable, channel);
            }

            SideHostControl.TypeViews.Add(new SideHostTypeChannel(channel, obj.GetType()));

            this._presenter.Content = (Control)obj;
        }



        private void EventBusManager_CloseViewEvent(Type view, int channel)
        {
            if (channel != this.Channel)
            {
                return;
            }

            if (this._presenter.Content == null)
            {
                return;
            }

            if (!(this._presenter.Content is UserControl))
            {
                return;
            }

            if (this.RemoveViewFromChannel(view, channel))
            {
                var t = this._presenter.Content.GetType();
                EventBusManager.Deregister(t);
                this._presenter.Content = null;
            }
        }

        private bool RemoveViewFromChannel(object obj, int channel)
        {
            var d = SideHostControl.TypeViews.FirstOrDefault(a =>
            {
                if (a.Channel != channel)
                {
                    return false;
                }

                if (a.TypeView == obj.GetType())
                {
                    return true;
                }

                if (obj is Type view)
                {
                    var presenterHasType = this._presenter.Content.GetType().Name;
                    var viewTypeName = view.Name;

                    return presenterHasType.Equals(viewTypeName);
                }

                return false;
            });

            if (d != null)
            {
                SideHostControl.TypeViews.Remove(d);
                return true;
            }

            return false;
        }
    }
}
