using Codexzier.Wpf.ApplicationFramework.Commands;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Views.Citizens;

namespace OverviewRkiData.Views.Menu
{
    internal class ButtonCommandOpenLandkreise : BaseCommand
    {
        public override void Execute(object parameter)
        {
            if (EventBusManager.IsViewOpen<CitizensView>(0))
            {
                return;
            }

            EventBusManager.OpenView<CitizensView>(0);
            EventBusManager.Send<CitizensView, BaseMessage>(new BaseMessage(""), 0);
        }
    }
}