using System.Data;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationResidualHistogramView : 
      IView, 
      ICanCopyToClipboardWithWatermark
   {
      void BindTo(DataTable gaussData, ContinuousDistributionData distributionData, DistributionSettings settings);
      ICanCopyToClipboard CopyToClipboardManager { get; set; }
   }
}