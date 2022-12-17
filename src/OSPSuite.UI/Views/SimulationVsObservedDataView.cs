using DevExpress.XtraLayout.Utils;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Format;
using static OSPSuite.Assets.Captions.ParameterIdentification;

namespace OSPSuite.UI.Views
{
   public partial class SimulationVsObservedDataView : BaseUserControl, ISimulationVsObservedDataView
   {
      private ISimulationVsObservedDataPresenter _presenter;
      private readonly IFormatter<double> _doubleFormatter = new DoubleFormatter();

      public SimulationVsObservedDataView()
      {
         InitializeComponent();
         totalErrorLayoutControlItem.TextVisible = true;
         totalErrorLayoutControlItem.Visibility = LayoutVisibility.Never;
         totalErrorTextEdit.ReadOnly = true;
      }

      public void SetAnalysisView(IView view)
      {
         panelChart.FillWith(view);
      }

      public void SetTotalError(double totalError)
      {
         totalErrorLayoutControlItem.Visibility = LayoutVisibility.Always;
         totalErrorTextEdit.Text = _doubleFormatter.Format(totalError);
      }

      public void AttachPresenter(ISimulationVsObservedDataPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         totalErrorLayoutControlItem.Text = TotalError.FormatForLabel();
      }
   }
}