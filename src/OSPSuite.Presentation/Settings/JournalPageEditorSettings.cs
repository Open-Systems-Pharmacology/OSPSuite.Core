using System.Drawing;

namespace OSPSuite.Presentation.Settings
{
   public class JournalPageEditorSettings : ViewSettings
   {
      public bool ShowTableGridLines { get; set; }

      public JournalPageEditorSettings()
      {
         Location = new Point(100, 100);
         Size = new Size(600, 400);
         ShowTableGridLines = false;
      }
   }
}
