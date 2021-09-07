using System;

namespace OverviewRkiData.Components.Database
{
    public class DatabaseQueryCreatorException : Exception
    {
        public DatabaseQueryCreatorException(string message) : base(message)
        {
        }
    }
}