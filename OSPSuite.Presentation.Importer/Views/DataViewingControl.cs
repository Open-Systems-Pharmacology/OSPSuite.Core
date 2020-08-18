using System.Data;
using System.Linq;
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

         var tempDataSourceFile = new ExcelDataSourceFile(null);

         var file = new OpenFileDialog();
         if (file.ShowDialog() == DialogResult.OK)
         {
            tempDataSourceFile.Path = file.FileName;
         }

         var keys = tempDataSourceFile.DataSheets.Keys;
         DataTable temp = tempDataSourceFile.DataSheets[keys.ElementAt(0)].RawData.AsDataTable();
         gridControl1.DataSource = temp;
         // gridView1.DataSource =temp();
      }
         public void AttachPresenter(IDataViewingPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}