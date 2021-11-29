using Codexzier.Wpf.ApplicationFramework.Commands;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using System;
using System.Windows.Controls;

namespace OverviewRkiData.Views.CitizensEdit
{
    /// <summary>
    /// Interaction logic for CitizensEditView.xaml
    /// </summary>
    public partial class CitizensEditView : UserControl
    {
        public CitizensEditView()
        {
            InitializeComponent();

            EventBusManager.Register<CitizensEditView, BaseMessage>(this.BaseMessageEvent);
        }

        private void BaseMessageEvent(IMessageContainer obj) => throw new NotImplementedException();
    }
}
