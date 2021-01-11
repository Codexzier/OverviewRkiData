using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OverviewRkiData.Components.Ui.EventBus
{
    public static class EventBusManager
    {
        public static int RegisteredCount => EventBusManagerInternal.GetInstance().RegisteredCount;

        public static int RegisteredCountAll => EventBusManagerInternal.GetInstance().RegisteredCountAll;

        public static int RegisteredCountByView<TView>() where TView : DependencyObject => EventBusManagerInternal.GetInstance().RegisteredCountByView<TView>();
        
        internal static ViewOpen GetViewOpened(int channel) => EventBusManagerInternal.GetInstance().GeViewOpenend(channel);


        /// <summary>
        /// create new internal instance host for message event. 
        /// </summary>
        /// <typeparam name="TView">Registered the View (UserControl or inherit dependencyObject).</typeparam>
        /// <typeparam name="TMessage">Registered the message object (Must inherit BaseMessage)</typeparam>
        /// <param name="receiverMethod">Set the method to be executed when a message is received.</param>
        public static void Register<TView, TMessage>(Action<IMessageContainer> receiverMethod)
            where TView : DependencyObject
            where TMessage : IMessageContainer => EventBusManagerInternal.GetInstance().Register<TView, TMessage>(receiverMethod);

        /// <summary>
        /// Registers an associated view that is located in another channel. 
        /// Enables the view to be removed when the associated main view is closed.
        /// </summary>
        /// <typeparam name="TViewParent">Main view</typeparam>
        /// <typeparam name="TViewChild">Child view</typeparam>
        /// <param name="channel">The channel of the child view.</param>
        public static void AddRegisterChildView<TViewParent, TViewChild>(int channel)
        {
            EventBusManagerInternal.GetInstance().AddRegisterChildView<TViewParent, TViewChild>(channel);
        }

        /// <summary>
        /// Close target view.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="channel"></param>
        public static void CloseView<TView>(int channel) => EventBusManagerInternal.GetInstance().CloseView<TView>(channel);

        /// <summary>
        /// Deregister closing content. 
        /// Obsolete: Every view need an dispose interface to cleanup events and unused references.
        /// </summary>
        /// <typeparam name="TView">Set the view for deregister</typeparam>
        public static void Deregister<TView>() => EventBusManagerInternal.GetInstance().Deregister<TView>();

        /// <summary>
        /// Deregister closing content.
        /// </summary>
        /// <param name="view">Set the view for deregister</param>
        public static void Deregister(Type view) => EventBusManagerInternal.GetInstance().Deregister(view);

        internal static bool IsViewOpen<TView>(int channel) => EventBusManagerInternal.GetInstance().IsViewOpen(typeof(TView), channel);
        internal static bool IsViewOpen(Type type, int channel) => EventBusManagerInternal.GetInstance().IsViewOpen(type, channel);

        /// <summary>
        /// Open new instance of a view. The view must setup the viewModel to the DataContext.
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        public static void OpenView<TView>(int channel) => EventBusManagerInternal.GetInstance().OpenView<TView>(channel);

        public static bool Send<TView, TMessageType>(TMessageType message, int channel, bool openView = false) where TMessageType : IMessageContainer => EventBusManagerInternal.GetInstance().Send<TView, TMessageType>(message, channel, openView);

        /// <summary>
        /// Eventbus singleton. can only one instance exist for the application
        /// </summary>
        private class EventBusManagerInternal
        {
            private static EventBusManagerInternal _eventBus;

            private readonly IDictionary<Type, IList<IMessageEventHost>> _viewsWithMessageEventHosts = new Dictionary<Type, IList<IMessageEventHost>>();
            private readonly IDictionary<Type, IList<ViewChildItem>> _viewsWithChildren = new Dictionary<Type, IList<ViewChildItem>>();

            private EventBusManagerInternal() { }

            public static EventBusManagerInternal GetInstance() => _eventBus ??= new EventBusManagerInternal();

            public int RegisteredCount => this._viewsWithMessageEventHosts.Count;

            public int RegisteredCountAll => this._viewsWithMessageEventHosts.Sum(c => c.Value.Count);

            public int RegisteredCountByView<TView>() 
                where TView : DependencyObject => this._viewsWithMessageEventHosts.Count == 0 ? 0 : this._viewsWithMessageEventHosts[typeof(TView)].Count;

            /// <summary>
            /// create new internal instance host for message event. 
            /// </summary>
            /// <typeparam name="TView"></typeparam>
            /// <typeparam name="TMessage"></typeparam>
            /// <param name="receiverMethod"></param>
            public void Register<TView, TMessage>(Action<IMessageContainer> receiverMethod)
                where TView : DependencyObject
                where TMessage : IMessageContainer
            {

                var host = new MessageEventHost<TView, TMessage>();
                host.Subscribe(receiverMethod);

                if (!this._viewsWithMessageEventHosts.ContainsKey(host.ViewType))
                {
                    this._viewsWithMessageEventHosts.Add(host.ViewType, new List<IMessageEventHost>());
                }

                if (this._viewsWithMessageEventHosts[host.ViewType].Any(a => a.MessageType == host.MessageType))
                {
                    throw new EventBusException($"can not register one moretime to the viewType {host.ViewType.Name} about message type: {host.MessageType.Name}");
                }

                this._viewsWithMessageEventHosts[host.ViewType].Add(host);
            }

            /// <summary>
            /// Registers an associated view that is located in another channel. 
            /// Enables the view to be removed when the associated main view is closed.
            /// </summary>
            /// <typeparam name="TViewParent">Main view</typeparam>
            /// <typeparam name="TViewChild">Child view</typeparam>
            /// <param name="channel">The channel of the child view.</param>
            public void AddRegisterChildView<TViewParent, TViewChild>(int channel)
            {
                if (!this._viewsWithChildren.ContainsKey(typeof(TViewParent)))
                {
                    this._viewsWithChildren.Add(typeof(TViewParent), new List<ViewChildItem>());
                }

                this._viewsWithChildren[typeof(TViewParent)].Add(new ViewChildItem(typeof(TViewChild), channel));
            }


            /// <summary>
            /// Deregister closing content. Every view need an dispose interface to cleanup events and unused references.
            /// </summary>
            /// <typeparam name="TView"></typeparam>
            public void Deregister<TView>() => this.Deregister(typeof(TView));

            public void Deregister(Type view)
            {
                if (this._viewsWithMessageEventHosts.ContainsKey(view))
                {
                    foreach (var item in this._viewsWithMessageEventHosts[view])
                    {
                        item.RemoveEventMethod();
                    }
                    this._viewsWithMessageEventHosts.Remove(view);
                }

                if (this._viewsWithChildren.ContainsKey(view))
                {
                    foreach (var item in this._viewsWithChildren[view])
                    {
                        this.CloseView(item.Type, item.Channel);
                    }
                }
            }

            internal bool IsViewOpen(Type type, int channel)
            {
                return SideHostControl.IsViewOpen(type, channel);
            }

            internal ViewOpen GeViewOpenend(int channel)
            {
                if (SideHostControl.TypeViews.All(a => a.Channel != channel) ||
                    !SideHostControl.TypeViews.Any(a => a.Channel == channel && a.TypeView.Name.EndsWith("View")))
                {
                    return ViewOpen.Nothing;
                }

                var typeView = SideHostControl.TypeViews.FirstOrDefault(f => f.Channel == channel);

                var viewName = typeView.TypeView.Name.Remove(typeView.TypeView.Name.Length - 4);

                if (Enum.TryParse(typeof(ViewOpen), viewName, out object result))
                {
                    return (ViewOpen)result;
                }

                throw new Exception($"Type has no enum: {typeView.TypeView.Name}");
            }

            /// <summary>
            /// Open new instance of a view. The view must setup the viewModel to the DataContext.
            /// </summary>
            /// <typeparam name="TView"></typeparam>
            public void OpenView<TView>(int channel)
            {
                this.OpenViewEvent?.Invoke((TView)Activator.CreateInstance(typeof(TView)), channel);
            }

            internal void CloseView<TView>(int channel)
            {
                this.CloseView(typeof(TView), channel);
            }

            internal void CloseView(Type view, int channel)
            {
                this.CloseViewEvent?.Invoke(view, channel);
            }

            public bool Send<TView, TMessageType>(TMessageType message, int channel = 0, bool openView = false) where TMessageType : IMessageContainer
            {
                foreach (var itemEventHosts in this._viewsWithMessageEventHosts)
                {
                    if (itemEventHosts.Key != typeof(TView))
                    {
                        continue;
                    }

                    foreach (var itemEventHost in itemEventHosts.Value)
                    {
                        if (itemEventHost.MessageType != message.GetType())
                        {
                            continue;
                        }

                        itemEventHost.Send(message);
                        return true;
                    }
                }

                if (this.OpenNewView<TView, TMessageType>(message, openView, channel))
                {
                    return true;
                }

                throw new EventBusException($"Not found or registered. View: {typeof(TView).Name}, {typeof(TMessageType).Name}");
            }

            private bool OpenNewView<TView, TMessageType>(TMessageType message, bool openView, int channel) where TMessageType : IMessageContainer
            {
                if (openView)
                {
                    this.OpenView<TView>(channel);

                    if (this.Send<TView, TMessageType>(message, channel))
                    {
                        return true;
                    }
                }

                return false;
            }

            public event OpenViewEventHandler OpenViewEvent;

            public event CloseViewEventHandler CloseViewEvent;
        }

        public delegate void OpenViewEventHandler(object obj, int channel);

        public static event OpenViewEventHandler OpenViewEvent
        {
            add { EventBusManagerInternal.GetInstance().OpenViewEvent += value; }
            remove { EventBusManagerInternal.GetInstance().OpenViewEvent -= value; }
        }

        public delegate void CloseViewEventHandler(Type view, int channel);
        public static event CloseViewEventHandler CloseViewEvent
        {
            add { EventBusManagerInternal.GetInstance().CloseViewEvent += value; }
            remove { EventBusManagerInternal.GetInstance().CloseViewEvent -= value; }
        }
    }
}
