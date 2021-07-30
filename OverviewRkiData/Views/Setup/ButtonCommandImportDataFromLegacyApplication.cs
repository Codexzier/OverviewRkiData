using Codexzier.Wpf.ApplicationFramework.Commands;
using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Views.Dialog;

namespace OverviewRkiData.Views.Setup
{
    internal class ButtonCommandImportDataFromLegacyApplication : BaseCommand
    {
        private readonly SetupViewModel _viewModel;
        public ButtonCommandImportDataFromLegacyApplication(SetupViewModel viewModel)
        {
            this._viewModel = viewModel;
        }


        public override void Execute(object parameter)
        {
            if (EventBusManager.IsViewOpen<DialogView>(10))
            {
                return;
            }

            EventBusManager.OpenView<DialogView>(10);

            var ddc = new DataDialogContent
            {
                Header = "Folder Browser"
            };
            EventBusManager.Send<DialogView, BaseMessage>(new BaseMessage(ddc), 10);
        }
    }
}