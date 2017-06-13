using System;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.UI.Binders;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Services
{
   public class CurveBinderFactory : ICurveBinderFactory
   {
      private readonly ICurveToDataModeMapper _dataModeMapper;

      public CurveBinderFactory(ICurveToDataModeMapper dataModeMapper)
      {
         _dataModeMapper = dataModeMapper;
      }

      public ICurveBinder CreateFor(Curve curve, object chartControl, CurveChart chart, IAxisBinder yAxisBinder)
      {
         var control = chartControl.DowncastTo<ChartControl>();
         var yAxis = yAxisBinder.AxisView.DowncastTo<AxisYBase>();
         var mode = _dataModeMapper.MapFrom(curve);

         switch (mode)
         {
            case DataMode.SingleValue:
               return new SingleValueCurveBinder(curve, control, chart, yAxis);

            case DataMode.ArithmeticStdDev:
               return new ArithmeticStdCurveBinder(curve, control, chart, yAxis);
            
            case DataMode.GeometricStdDev:
               return new GeometricStdCurveBinder(curve, control, chart, yAxis);

            case DataMode.ArithmeticMeanArea:
               return new ArithmeticMeanAreaCurveBinder(curve, control, chart, yAxis);

            case DataMode.GeometricMeanArea:
               return new GeometricMeanAreaCurveBinder(curve, control, chart, yAxis);

            default:
               throw new ArgumentException($"Don't know how to build curve binder for '{mode}'");
         }
      }
   }
}