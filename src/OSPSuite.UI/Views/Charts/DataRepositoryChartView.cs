using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class DataRepositoryChartView : BaseUserControl, IDataRepositoryChartView
   {
      public DataRepositoryChartView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IDataRepositoryChartPresenter presenter)
      {
      }

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.DefaultIcon;

      public void AddChart(IView view)
      {
         panelChart.FillWith(view);
      }
   }
}