using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Starter.Views
{
   public partial class DataRepositoryTestView : BaseUserControl, IDataRepositoryTestView
   {
      private IDataRepositoryTestPresenter _presenter;

      public DataRepositoryTestView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IDataRepositoryTestPresenter presenter) => _presenter = presenter;

      public void AddChartView(IView baseView) => chartPanel.FillWith(baseView);

      public void AddDataView(IView baseView) => dataPanel.FillWith(baseView);

      public void AddMetaDataView(IView baseView) => metaDataPanel.FillWith(baseView);

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnExportToCSV.Click += (o, e) => OnEvent(()=>_presenter.ExportToCSV());
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnExportToCSV.Text = "Export to CSV";
      }
   }


}
