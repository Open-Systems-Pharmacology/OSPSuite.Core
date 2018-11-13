using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationMultipleRunsAnalysisView : BaseUserControl, IParameterIdentificationMultipleRunsAnalysisView
   {
      private IParameterIdentificationAnalysisPresenter _presenter;
      public ParameterIdentificationMultipleRunsAnalysisView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IParameterIdentificationAnalysisPresenter presenter)
      {
         _presenter = presenter;
      }


      public void SetAnalysisView(IView view)
      {
         panelChart.FillWith(view);
      }
   }
}