using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Extensions;

namespace OSPSuite.UI.Importer
{
   public partial class OpenSourceFileControl : XtraUserControl
   {
      private readonly IDialogCreator _dialogCreator;
      
      public OpenSourceFileControl(IDialogCreator dialogCreator, string sourceFile)
      {
         InitializeComponent();
         _dialogCreator = dialogCreator;
         txtExcelFile.Text = sourceFile;
         layoutItemExcelFile.Text = Captions.Importer.ExcelFile.FormatForLabel();
      }

      private bool selectSourceFile()
      {
         var initDir = Path.GetDirectoryName(txtExcelFile.Text);
         var newFile = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, OSPSuite.Core.Domain.Constants.DirectoryKey.OBSERVED_DATA, initDir);

         if (string.IsNullOrEmpty(newFile))
            return false;

         txtExcelFile.Text = newFile;
         return true;
      }

      /// <summary>
      ///    Property retrieving the full path and name of the source file.
      /// </summary>
      public string SourceFile
      {
         get { return txtExcelFile.Text; }
      }

      public class OpenSourceFileEventArgs : EventArgs
      {
         /// <summary>
         ///    Full path and name of the source file.
         /// </summary>
         public string SourceFile { get; set; }
      }

      /// <summary>
      ///    Handler for event OnOpenSourceFile.
      /// </summary>
      public delegate void OpenSourceFileHandler(object sender, OpenSourceFileEventArgs e);

      /// <summary>
      ///    Event raised when the user has opened a new source file.
      /// </summary>
      public event OpenSourceFileHandler OnOpenSourceFile = delegate { };

      /// <summary>
      ///    Method for raising OnOpenSourceFile event when new selected has taken place.
      /// </summary>
      private void openSourceFile()
      {
         if (txtExcelFile.Text.Length == 0) return;
         if (File.Exists(txtExcelFile.Text))
         {
            OnOpenSourceFile(this, new OpenSourceFileEventArgs {SourceFile = txtExcelFile.Text});
         }
         else XtraMessageBox.Show("The specified file does not exist!", "An error occurred:", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      /// <summary>
      ///    Method reacting when user clicks on select excel file button.
      /// </summary>
      private void btnSelectExcelFileClick(object sender, EventArgs e)
      {
         if (selectSourceFile())
            openSourceFile();
      }

      private void cleanMemory()
      {
         CleanUpHelper.ReleaseEvents(this);
      }
   }
}