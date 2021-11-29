using Codexzier.Wpf.ApplicationFramework.Commands;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OverviewRkiData.Views.Citizens
{
    /// <summary>
    /// Interaction logic for CitizensView.xaml
    /// </summary>
    public partial class CitizensView : UserControl
    {
        public CitizensView()
        {
            InitializeComponent();

            EventBusManager.Register<CitizensView, BaseMessage>(this.BaseMessageEvent);
        }

        private void BaseMessageEvent(IMessageContainer obj)
        {
            // TODO: load alle Landkreise
        }
    }
}
