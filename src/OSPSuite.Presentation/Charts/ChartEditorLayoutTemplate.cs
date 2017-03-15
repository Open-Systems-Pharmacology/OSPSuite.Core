using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Charts
{
   public class ChartEditorLayoutTemplate : IWithName
   {
      public string Name { get; set; }
      public ChartEditorAndDisplaySettings Settings { get; set; }

      public override string ToString()
      {
         return Name;
      }
   }
}