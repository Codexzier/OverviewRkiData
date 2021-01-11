using System;

namespace OverviewRkiData.Components.Ui.EventBus
{
    internal class ViewChildItem
    {
        public ViewChildItem(Type type, int channel)
        {
            this.Type = type;
            this.Channel = channel;
        }

        public Type Type { get; }
        public int Channel { get; }
    }
}