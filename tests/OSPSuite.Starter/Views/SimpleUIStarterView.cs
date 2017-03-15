using System;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Utility.Container;
using DevExpress.XtraEditors;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace OSPSuite.Starter.Views
{
   public partial class SimpleUIStarterView : Form
   {
      private DirectoryMapSettings _directoryMap;
      private readonly IDialogCreator _dialogCreator;

      public SimpleUIStarterView()
      {
         InitializeComponent();
         allowChecksOutsideControlAreaCheckBox.CheckStateChanged += toggleCheckAreas;
         _directoryMap = new DirectoryMapSettings();
         _dialogCreator = IoC.Resolve<IDialogCreator>();
      }

      private void toggleCheckAreas(object sender, EventArgs e)
      {
         var checkEdit = sender as CheckEdit;
         if (checkEdit.Checked)
         {
            leftHandCaptionCheckBox.AllowClicksOutsideControlArea = true;
            rightHandCaptionCheckBox.AllowClicksOutsideControlArea = true;
         }
         else
         {
            leftHandCaptionCheckBox.AllowClicksOutsideControlArea = false;
            rightHandCaptionCheckBox.AllowClicksOutsideControlArea = false;
         }
      }

      private void simpleButton1_Click(object sender, EventArgs e)
      {
         var folder = _dialogCreator.AskForFolder("Test", "Import");
         _dialogCreator.MessageBoxInfo(folder);
      }

      private void btnShowInputDialog_Click(object sender, EventArgs e)
      {
         var dialog = InputBoxDialog.Show("Prompt", "Title", "France", Enumerable.Empty<string>(), predefinedValues: new[] {"France", "Germany", "Danemark", "Finland"});
         
         InputBoxDialog.Show("Prompt", "Title", "France");

         InputBoxDialog.Show("Prompt", "Title");

         InputBoxDialog.Show("Prompt", "Title",predefinedValues: new[] { "France", "Germany", "Danemark", "Finland" });

         InputBoxDialog.Show("Prompt", "Title", forbiddenValues: new[] { "France", "Germany", "Danemark", "Finland" });
      }
   }


}
