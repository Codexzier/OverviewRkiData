using System;

namespace OverviewRkiData.Components.Ui.EventBus
{
    public class RegisterContainer
    {
        public RegisterContainer(Type typeView, Func<IMessageContainer, bool> receiverMethod)
        {
            this.View = typeView;
            this.EventMethod = receiverMethod;
        }

        public Type View { get; }

        public Func<IMessageContainer, bool> EventMethod { get; }
    }
}
