using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationSingleRunAnalysisView : BaseUserControl, IParameterIdentificationSingleRunAnalysisView
   {
      private IParameterIdentificationSingleRunAnalysisPresenter _presenter;
      private readonly ScreenBinder<IParameterIdentificationSingleRunAnalysisPresenter> _screenBinder;

      public ParameterIdentificationSingleRunAnalysisView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<IParameterIdentificationSingleRunAnalysisPresenter>();
      }

      public void AttachPresenter(IParameterIdentificationSingleRunAnalysisPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetAnalysisView(IView view)
      {
         panelAnalysis.FillWith(view);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.SelectedRunResults)
            .To(cbRunSelection)
            .WithValues(x => x.AllRunResults)
            .AndDisplays(run => run.SingleLineDescription);
      }

      public bool CanSelectRunResults
      {
         set => layoutItemRunSelection.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
      }

      public void BindToSelectedRunResult()
      {
         _screenBinder.BindToSource(_presenter);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemChartControl.TextVisible = false;
         layoutItemRunSelection.TextVisible = false;
      }

   }
}