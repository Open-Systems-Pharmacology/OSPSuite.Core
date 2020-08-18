using System.Data;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Importer.Core.DataSourceFileReaders;
using System.Windows.Forms;

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

         /*
          *         //     tempDataSourceFile.Path = "C:/Users/GeorgiosDaskalakis/Documents/GitHub/OSPSuite.Core/tests/OSPSuite.Presentation.Tests/Data/sample1.xlsx";
         // tempDataSourceFile.Path = "C:/Users/GeorgiosDaskalakis/Downloads/Midazolam.xlsx";
         tempDataSourceFile.Path = "C:/Users/GeorgiosDaskalakis/Downloads/Book1 (1).xlsx";
         DataTable temp = tempDataSourceFile.DataSheets["Sheet1"].RawData.AsDataTable();
         gridControl1.DataSource = temp;
         // gridView1.DataSource =temp();

          *
          */

         var tempDataSourceFile = new ExcelDataSourceFile(null);

         //tempDataSourceFile.Path = "C:/Users/GeorgiosDaskalakis/Documents/GitHub/OSPSuite.Core/tests/OSPSuite.Presentation.Tests/Data/sample1.xlsx";
         //tempDataSourceFile.Path = "C:/Users/GeorgiosDaskalakis/Downloads/Midazolam.xlsx";  iv, 0.15 mgkg(Heizmann 1983)
         var file = new OpenFileDialog();
         if (file.ShowDialog() == DialogResult.OK)
         {
            tempDataSourceFile.Path = file.FileName;
         }

         //tempDataSourceFile.Path = "C:/Users/GeorgiosDaskalakis/Downloads/Book1 (1).xlsx";
         DataTable temp = tempDataSourceFile.DataSheets["Sheet1"].RawData.AsDataTable();
         gridControl1.DataSource = temp;
         // gridView1.DataSource =temp();
      }
         public void AttachPresenter(IDataViewingPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}