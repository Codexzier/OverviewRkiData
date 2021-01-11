using System;

namespace OverviewRkiData.Components.Ui.EventBus
{
    public class SideHostTypeChannel
    {
        public SideHostTypeChannel(int channel, Type typeView)
        {
            this.Channel = channel;
            this.TypeView = typeView;
        }

        public int Channel { get; private set; }

        public Type TypeView { get; private set; }
    }
}