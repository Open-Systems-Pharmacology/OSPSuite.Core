using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Extensions
{
   public static class WithChartTemplatesExtensions
   {
      /// <summary>
      ///    Returns the default template among the templates defined for the given <paramref name="chartType" />:
      ///    the first one flagged as default, otherwise the first one by name. Returns <c>null</c> if no template
      ///    is defined for <paramref name="chartType" />.
      /// </summary>
      public static CurveChartTemplate DefaultChartTemplateFor(this IWithChartTemplates withChartTemplates, CurveChartTypes chartType)
      {
         var templatesForChartType = withChartTemplates.ChartTemplates.Where(x => x.ChartType == chartType).ToList();
         return templatesForChartType.FirstOrDefault(x => x.IsDefault) ?? templatesForChartType.OrderBy(x => x.Name).FirstOrDefault();
      }
   }
}
