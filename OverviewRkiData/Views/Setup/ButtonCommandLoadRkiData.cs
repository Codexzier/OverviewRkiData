using Codexzier.Wpf.ApplicationFramework.Views.Base;
using OverviewRkiData.Components.RkiCoronaLandkreise;

namespace OverviewRkiData.Views.Setup
{
    internal class ButtonCommandLoadRkiData : BaseCommand
    {
        public override void Execute(object parameter)
        {
            var component = RkiCoronaLandkreiseComponent.GetInstance();
            component.RkiDataErrorEvent += this.Component_RkiDataErrorEvent;
            component.LoadData(out var saveIt);
            saveIt(true);
            component.RkiDataErrorEvent -= this.Component_RkiDataErrorEvent;
        }

        private void Component_RkiDataErrorEvent(string message) => SimpleStatusOverlays.Show("ERROR", message);
    }
}