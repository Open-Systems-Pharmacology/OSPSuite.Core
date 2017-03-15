using System.Drawing;

namespace OSPSuite.Presentation.Presenters.Commands
{
    public interface IHistoryBrowserProperties
    {
        Icon Icon {get; set;}
        string Caption { get; set; }

    }

    public class HistoryBrowserProperties : IHistoryBrowserProperties
    {
        public Icon Icon { get; set; }
        public string Caption { get; set; }
    }
}