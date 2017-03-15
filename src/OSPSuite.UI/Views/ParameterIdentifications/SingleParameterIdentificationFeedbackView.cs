using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class SingleParameterIdentificationFeedbackView : BaseUserControl, ISingleParameterIdentificationFeedbackView
   {
      private ISingleParameterIdentificationFeedbackPresenter _presenter;

      public SingleParameterIdentificationFeedbackView()
      {
         InitializeComponent();
      }

      public void AddParameterView(IView view)
      {
         panelParameters.FillWith(view);
      }

      public void AddTimeProfileView(IView view)
      {
         panelTimeProfile.FillWith(view);
      }

      public void AddErrorHistoryView(IView view)
      {
         panelErrorHistory.FillWith(view);
      }

      public void AddPredictedVsObservedView(IView view)
      {
         panelPredictedVsObserved.FillWith(view);
      }

      public void AttachPresenter(ISingleParameterIdentificationFeedbackPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
