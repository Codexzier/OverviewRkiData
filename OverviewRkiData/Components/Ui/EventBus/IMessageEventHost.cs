using System;

namespace OverviewRkiData.Components.Ui.EventBus
{
    public interface IMessageEventHost
    {
        Type ViewType { get; }
        Type MessageType { get; }

        void Send(IMessageContainer message);

        void Subscribe(Action<IMessageContainer> receiverMethod);
        void RemoveEventMethod();
    }
}