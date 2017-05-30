using OSPSuite.Core.Chart;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace OSPSuite.Starter.Views
{
   public partial class HistogramTestView : BaseUserControl, IHistogramTestView
   {
      private IHistogramTestPresenter _presenter;
      private ScreenBinder<IHistogramTestPresenter> _screenBinder;

      public HistogramTestView(IImageListRetriever imageListRetriever, IToolTipCreator toolTipManager)
      {
         InitializeComponent();

         uxHistogramControl.Initialize(imageListRetriever, toolTipManager);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<IHistogramTestPresenter>();

         _screenBinder.Bind(x => x.Minimum).To(minTextEdit);
         _screenBinder.Bind(x => x.Maximum).To(maxTextEdit);
         _screenBinder.Bind(x => x.Bins).To(binsTextEdit);
         _screenBinder.Bind(x => x.ValueCount).To(valuesTextEdit);
         _screenBinder.BindToSource(_presenter);
         plotButton.Click += (o,e) => OnEvent(() => _presenter.Plot());
      }

      public void PlotPopulationData(ContinuousDistributionData distributionData, DistributionSettings distributionSettings)
      {
         uxHistogramControl.Plot(distributionData, distributionSettings);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         plotButton.Text = "Plot";
      }

      public void AttachPresenter(IHistogramTestPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}