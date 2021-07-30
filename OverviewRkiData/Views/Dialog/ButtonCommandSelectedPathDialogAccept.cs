using Codexzier.Wpf.ApplicationFramework.Components.Ui.EventBus;
using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Components.LegacyData;
using System.IO;

namespace OverviewRkiData.Views.Dialog
{
    internal class ButtonCommandSelectedPathDialogAccept : BaseCommand
    {
        private readonly DialogViewModel _viewModel;

        public ButtonCommandSelectedPathDialogAccept(DialogViewModel viewModel) => this._viewModel = viewModel;

        public override void Execute(object parameter)
        {
            EventBusManager.CloseView<DialogView>(10);
            
            var selectedFolder = this._viewModel.SelectedDirectoryPath.FolderName;
            
            if (string.IsNullOrEmpty(selectedFolder))
            {
                SimpleStatusOverlays.Show("Import Fehler", "Kein Ordner ausgewählt");
                return;
            }

            if (!Directory.Exists(selectedFolder))
            {
                SimpleStatusOverlays.Show("Import Fehler", $"Dieser Ordner ist nicht gültig! {selectedFolder}");
                return;
            }
            
            var count = new LegacyDataConverter().Run(selectedFolder);
            
            SimpleStatusOverlays.Show("Import abgeschlossen", $"Es wurden {count} Dateien importiert aus dem Ordner '{selectedFolder}'");
        }
    }
}