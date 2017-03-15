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
      public int HelpId { get; set; } = OSPSuite.Presentation.HelpId.Tool_Parameter_Identification_Analyses;

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

      protected override int TopicId => HelpId;
   }
}