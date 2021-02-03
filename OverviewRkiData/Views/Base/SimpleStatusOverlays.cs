using System;
using System.Threading.Tasks;
using OverviewRkiData.Components.Ui.EventBus;
using OverviewRkiData.Views.ActivityLoading;
using OverviewRkiData.Views.MessageBox;
using System.Windows;

namespace OverviewRkiData.Views.Base
{
    public static class SimpleStatusOverlays
    {
        public static void Show(string title, string message)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                EventBusManager.Send<MessageBoxView, MessageBoxMessage>(new MessageBoxMessage(title, message), 10, true);
            });
        }

        public static void ActivityOn()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                EventBusManager.OpenView<ActivityLoadingView>(10);
            });
        }

        public static void ActivityOff()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                EventBusManager.CloseView<ActivityLoadingView>(10);
            });
        }

        public static void ShowAsk(string title, string message, Action<bool> safeData)
        {
            bool result = false;
            Application.Current.Dispatcher.Invoke(delegate
            {
                EventBusManager.Send<MessageBoxView, AskBoxMessage>(new AskBoxMessage(title, message, safeData), 10, true);
            });
        }
    }
}
