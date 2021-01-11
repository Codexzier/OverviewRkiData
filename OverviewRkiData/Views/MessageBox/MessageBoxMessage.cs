using OverviewRkiData.Commands;

namespace OverviewRkiData.Views.MessageBox
{
    internal class MessageBoxMessage : BaseMessage
    {
        public MessageBoxMessage(string title, string message) : base(message)
        {
            this.Title = title;
        }

        public string Title { get; }
    }
}