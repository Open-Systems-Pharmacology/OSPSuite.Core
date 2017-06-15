using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Chart.Mappers
{
   public interface ICurveChartToCurveChartTemplateMapper : IMapper<CurveChart, CurveChartTemplate>
   {
   }

   public class CurveChartToCurveChartTemplateMapper : ICurveChartToCurveChartTemplateMapper
   {
      private readonly ICloneManager _cloneManager;

      public CurveChartToCurveChartTemplateMapper(ICloneManager cloneManager)
      {
         _cloneManager = cloneManager;
      }

      public CurveChartTemplate MapFrom(CurveChart curveChart)
      {
         var template = new CurveChartTemplate();
         if (curveChart == null)
            return template;

         template.IncludeOriginData = curveChart.IncludeOriginData;
         template.ChartSettings.UpdatePropertiesFrom(curveChart.ChartSettings, _cloneManager);
         template.FontAndSize.UpdatePropertiesFrom(curveChart.FontAndSize, _cloneManager);
         curveChart.Curves.Each(curve => template.Curves.Add(curveFrom(curve)));
         curveChart.Axes.Each(axis => template.AddAxis(axis.Clone()));

         return template;
      }

      private CurveTemplate curveFrom(Curve curve)
      {
         var curveTemplate = new CurveTemplate
         {
            Name = curve.Name,
         };

         updateCurveData(curveTemplate.xData, curve.xData);
         updateCurveData(curveTemplate.yData, curve.yData);
         curveTemplate.CurveOptions.UpdateFrom(curve.CurveOptions);
         return curveTemplate;
      }

      private void updateCurveData(CurveDataTemplate data, DataColumn dataColumn)
      {
         data.QuantityType = dataColumn.QuantityInfo.Type;
         data.Path = dataColumn.TemplatePath;
         data.RepositoryName = dataColumn.Repository?.Name;
      }
   }
}