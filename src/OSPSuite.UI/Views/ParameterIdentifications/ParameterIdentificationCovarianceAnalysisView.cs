using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationCovarianceAnalysisView : BaseUserControl, IParameterIdentificationCovarianceAnalysisView
   {
      public ParameterIdentificationCovarianceAnalysisView()
      {
         InitializeComponent();
      }

      public void SetMatrixView(IView view)
      {
         panelMatrix.FillWith(view);
      }

      public void SetConfidenceIntevalView(IView view)
      {
         panelConfidenceInterval.FillWith(view);
      }
   }
}