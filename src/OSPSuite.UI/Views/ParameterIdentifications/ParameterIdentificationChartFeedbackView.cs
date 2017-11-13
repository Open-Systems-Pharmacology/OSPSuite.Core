using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationChartFeedbackView : BaseUserControl, IParameterIdentificationChartFeedbackView
   {
      private IParameterIdentificationChartFeedbackPresenter _presenter;
      private readonly ScreenBinder<IParameterIdentificationChartFeedbackPresenter> _screenBinder;

      public void AttachPresenter(IParameterIdentificationChartFeedbackPresenter presenter)
      {
         _presenter = presenter;
      }

      public ParameterIdentificationChartFeedbackView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<IParameterIdentificationChartFeedbackPresenter>();
      }

      public void AddChartView(IView view)
      {
         panelChart.FillWith(view);
      }

      public void BindToSelecteOutput()
      {
         _screenBinder.BindToSource(_presenter);
         OutputSelectionEnabled = true;
      }

      public void ClearBinding()
      {
         _screenBinder.DeleteBinding();
         OutputSelectionEnabled = false;
      }

      public bool OutputSelectionEnabled
      {
         get => cbOutputSelection.Enabled;
         set => cbOutputSelection.Enabled = value;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(x => x.SelectedOutput)
            .To(cbOutputSelection)
            .WithValues(x => _presenter.AllOutputs)
            .AndDisplays(x => x.FullOutputPath);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemChart.TextVisible = false;
         layoutItemOutputSelection.TextVisible = false;
      }
   }
}