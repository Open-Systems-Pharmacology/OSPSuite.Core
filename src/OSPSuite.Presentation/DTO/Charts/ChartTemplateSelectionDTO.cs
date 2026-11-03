using OSPSuite.Core.Chart;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Presentation.DTO.Charts
{
   public class ChartTemplateSelectionDTO : Notifier
   {
      private bool _selected;

      public CurveChartTemplate Template { get; }

      public ChartTemplateSelectionDTO(CurveChartTemplate template)
      {
         Template = template;
      }

      public string Name => Template.Name;

      public bool Selected
      {
         get => _selected;
         set => SetProperty(ref _selected, value);
      }
   }
}
