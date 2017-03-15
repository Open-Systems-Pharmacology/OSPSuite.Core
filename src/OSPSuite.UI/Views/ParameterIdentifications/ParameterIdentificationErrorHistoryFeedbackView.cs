using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationErrorHistoryFeedbackView : BaseUserControl, IParameterIdentificationErrorHistoryFeedbackView
   {
      private IParameterIdentificationErrorHistoryFeedbackPresenter _presenter;

      public ParameterIdentificationErrorHistoryFeedbackView()
      {
         InitializeComponent();
      }

      public void AddChartView(IView view)
      {
         panelChart.FillWith(view);
      }

      public void AttachPresenter(IParameterIdentificationErrorHistoryFeedbackPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}