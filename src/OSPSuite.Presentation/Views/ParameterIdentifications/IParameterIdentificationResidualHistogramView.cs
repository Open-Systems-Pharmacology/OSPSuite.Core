using System.Data;
using OSPSuite.Core.Chart;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationResidualHistogramView : IView
   {
      void BindTo(DataTable gaussData, ContinuousDistributionData distributionData, DistributionSettings settings);
   }
}