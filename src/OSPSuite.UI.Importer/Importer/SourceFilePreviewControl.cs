using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using OSPSuite.Utility.Collections;

namespace OSPSuite.UI.Importer
{
   public partial class SourceFilePreviewControl : XtraUserControl
   {
      private DataSetControl _dataSetControl;
      private readonly string _fileName;
      private DataSet _data;
      private readonly Cache<string, Rectangle> _rangesCache;
      private readonly Presentation.Services.Importer _importer;

      public SourceFilePreviewControl(string fileName, Cache<string, Rectangle> rangeCache, Presentation.Services.Importer importer)
      {
         InitializeComponent();
         _rangesCache = rangeCache;
         _importer = importer;
         _fileName = fileName;
         showFilePreview(_fileName);
      }

      private void showFilePreview(string fileName)
      {
         if (fileName.Length == 0) return;
         if (!File.Exists(fileName))
         {
            XtraMessageBox.Show("The specified file does not exist!", "An error occurred:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         var cursor = Cursor.Current;
         try
         {
            Cursor.Current = Cursors.WaitCursor;

            _data = _importer.GetPreview(fileName, _rangesCache);

            _dataSetControl = new DataSetControl(_data);
            Controls.Add(_dataSetControl);
            _dataSetControl.Dock = DockStyle.Fill;
            _dataSetControl.TableSelected += onDataSetControlTableSelected;
            _dataSetControl.TableDeleted += onDataSetControlTableDeleted;
         }
         finally
         {
            Cursor.Current = cursor;
         }
      }

      public DataSet PreviewData
      {
         get { return _data; }
      }

      public string SelectedSheetName
      {
         get { return _dataSetControl.SelectedTable.TableName; }
      }

      public DataTable SelectedSheetData
      {
         get { return _dataSetControl.SelectedTable; }
      }

      /// <summary>
      ///    Event arguments for event SheetSelected.
      /// </summary>
      public class SheetSelectedEventArgs : EventArgs
      {
         /// <summary>
         ///    Name of the selected sheet.
         /// </summary>
         public string SheetName { get; set; }

         /// <summary>
         ///    Table data of the selected sheet.
         /// </summary>
         public DataTable SheetData { get; set; }
      }

      /// <summary>
      ///    Handler for event SheetSelected.
      /// </summary>
      public delegate void SheetSelectedHandler(object sender, SheetSelectedEventArgs e);

      /// <summary>
      ///    Event raised when sheet selection has been changed.
      /// </summary>
      public event SheetSelectedHandler SheetSelected;

      /// <summary>
      ///    Method reacting when user is changing the table selection.
      /// </summary>
      private void onDataSetControlTableSelected(object sender, DataSetControl.TableSelectedEventArgs e)
      {
         if (SheetSelected != null) SheetSelected(this, new SheetSelectedEventArgs {SheetName = e.TableName, SheetData = e.Table});
      }

      /// <summary>
      ///    Method reacting when user is deleting a table.
      /// </summary>
      private void onDataSetControlTableDeleted(object sender, DataSetControl.TableDeletedEventArgs e)
      {
         if (e.TableCount != 0) return;
         Controls.Clear();
         showFilePreview(_fileName);
         if (SheetSelected != null)
            SheetSelected(this,
               new SheetSelectedEventArgs
               {
                  SheetName = _dataSetControl.SelectedTable.TableName,
                  SheetData = _dataSetControl.SelectedTable
               });
      }

      private void cleanMemory()
      {
         if (_data != null)
         {
            foreach (DataTable table in _data.Tables)
               table.Dispose();
            _data.Dispose();
         }

         CleanUpHelper.ReleaseEvents(_dataSetControl);
         if (_dataSetControl != null) _dataSetControl.Dispose();

         CleanUpHelper.ReleaseEvents(this);
         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }

      public void UpdateTable(string tableName, DataTable table)
      {
         _dataSetControl.UpdateTable(tableName, table);
      }
   }
}