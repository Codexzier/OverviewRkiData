using System;
using OverviewRkiData.Commands;

namespace OverviewRkiData.Views.MessageBox
{
    internal class MessageBoxMessage : BaseMessage
    {
        public MessageBoxMessage(string title, string message) : base(message) => this.Title = title;

        public string Title { get; }
    }

    internal class AskBoxMessage : MessageBoxMessage
    {
        public Action<bool> Result { get; }

        public AskBoxMessage(string title, string message, Action<bool> result) : base(title, message) => this.Result = result;

        public void Execute(bool yes)
        {
            this.Result(yes);
        }
    }
}