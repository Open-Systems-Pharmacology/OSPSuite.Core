using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationWeightedObservedDataView : BaseUserControl, IParameterIdentificationWeightedObservedDataView
   {
      private IParameterIdentificationWeightedObservedDataPresenter _presenter;
      public override ApplicationIcon ApplicationIcon => ApplicationIcons.ObservedData;

      public ParameterIdentificationWeightedObservedDataView()
      {
         InitializeComponent();

         layoutControl.Resize += (o, e) => OnEvent(layoutControlResized);
         VisibleChanged+= (o, e) =>OnEvent(ResizeView);
      }

      public void AttachPresenter(IParameterIdentificationWeightedObservedDataPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddDataView(IView view)
      {
         panelData.FillWith(view);
      }

      public void AddChartView(IView view)
      {
         panelChart.FillWith(view);
      }

      public void ResizeView()
      {
         layoutControl.Invalidate();
         layoutControl.Refresh();
      }

      private void layoutControlResized()
      {
         ResizeView();
      }
   }
}