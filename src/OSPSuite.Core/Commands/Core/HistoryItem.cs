using System;

namespace OSPSuite.Core.Commands.Core
{
    public interface IHistoryItem
    {
        string Id { get; set; }
        int State { get; set; }
        string User { get; }
        DateTime DateTime { get; }
        ICommand Command { get; }
    }

    public class HistoryItem : IHistoryItem
    {
        public string Id { get; set; }
        public ICommand Command { get; private set; }
        public int State { get; set; }
        public string User { get; private set; }
        public DateTime DateTime { get; private set; }
        
        public HistoryItem( string user, DateTime dateTime, ICommand command)
        {
            User = user;
            DateTime = dateTime;
            Command = command;
        }


    }

}