using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Views.County;
using OverviewRkiData.Views.Setup;

namespace OverviewRkiData.Views.Menu
{
    internal class ButtonCommandOpenSetup : BaseCommand
    {

        public ButtonCommandOpenSetup() { }

        public override void Execute(object parameter)
        {
            if (EventBusManager.IsViewOpen<SetupView>(0))
            {
                return;
            }

            if (EventBusManager.IsViewOpen<CountyView>(1))
            {
                EventBusManager.CloseView<CountyView>(1);
            }

            EventBusManager.OpenView<SetupView>(0);
        }
    }
}