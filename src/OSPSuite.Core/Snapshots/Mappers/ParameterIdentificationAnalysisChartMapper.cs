using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class ParameterIdentificationAnalysisChartMapper : CurveChartMapper<AnalysisChart>
   {
      public ParameterIdentificationAnalysisChartMapper(ChartMapper chartMapper, AxisMapper axisMapper, CurveMapper curveMapper, IIdGenerator idGenerator) : base(chartMapper, axisMapper, curveMapper, idGenerator)
      {
      }
   }
}