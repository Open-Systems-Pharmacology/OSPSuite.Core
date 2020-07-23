using System.Data;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Importer.Core.DataSourceFileReaders;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class DataViewingControl : BaseUserControl, IDataViewingControl
   {
      private IDataViewingPresenter _presenter;
      public DataViewingControl()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeResources();

         var tempDataSourceFile = new ExcelDataSourceFile(null);
         tempDataSourceFile.Path = "C:/Users/GeorgiosDaskalakis/Documents/GitHub/OSPSuite.Core/tests/OSPSuite.Presentation.Tests/Data/sample1.xlsx";
         DataTable temp = tempDataSourceFile.DataSheets["Sheet1"].RawData.GetSheetAsDataTable();
         gridControl1.DataSource = temp;
         // gridView1.DataSource =temp();
      }
         public void AttachPresenter(IDataViewingPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}