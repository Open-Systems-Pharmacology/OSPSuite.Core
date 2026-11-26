using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO.Charts
{
   /// <summary>
   ///    An object of the project owning chart templates (e.g. the project itself or a simulation) from which templates can
   ///    be copied
   /// </summary>
   public class ChartTemplateSourceDTO
   {
      public IWithChartTemplates Source { get; }
      public string DisplayName { get; }

      public ChartTemplateSourceDTO(IWithChartTemplates source, string displayName)
      {
         Source = source;
         DisplayName = displayName;
      }

      public override string ToString() => DisplayName;
   }
}
