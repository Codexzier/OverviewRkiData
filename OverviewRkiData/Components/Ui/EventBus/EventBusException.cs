using System;

namespace OverviewRkiData.Components.Ui.EventBus
{
    public class EventBusException : Exception
    {
        public EventBusException(string message) : base(message)
        {
        }
    }
}