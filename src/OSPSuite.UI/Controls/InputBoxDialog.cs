using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Controls
{
   public partial class InputBoxDialog : XtraForm
   {
      internal IEnumerable<string> NotAllowedValues { get; set; }
      internal string FormPrompt { get; set; }
      internal string DefaultValue { get; set; }
      internal IEnumerable<string> Values { get; set; }
      internal string FormCaption { get; set; }

      public InputBoxDialog()
      {
         InitializeComponent();
         initializeResources();
         Load += onLoad;
         btnOk.Manager = new BarManager {Form = this};
      }

      private void initializeResources()
      {
         lblPrompt.AsDescription();

         btnOk.Shortcut = Keys.Control | Keys.Enter;
         btnCancel.Text = Captions.CancelButton;

         btnOk.InitWithImage(ApplicationIcons.OK, Captions.OKButton, ImageLocation.MiddleRight);
         btnCancel.InitWithImage(ApplicationIcons.Cancel, Captions.CancelButton, ImageLocation.MiddleRight);

         MaximizeBox = false;
         MinimizeBox = false;
         layoutItemOk.AdjustButtonSize();
         layoutItemCancel.AdjustButtonSize();
      }

      internal string InputResponse => cbInput.Text;

      public static string Show(string prompt, string title, string defaultValue = null, IEnumerable<string> forbiddenValues = null, IEnumerable<string> predefinedValues = null)
      {
         var inputBoxDialog = new InputBoxDialog
         {
            FormPrompt = prompt,
            FormCaption = title,
            DefaultValue = defaultValue ?? string.Empty,
            Values = predefinedValues ?? Enumerable.Empty<string>(),
            NotAllowedValues = forbiddenValues ?? Enumerable.Empty<string>()
         };

         if (inputBoxDialog.ShowDialog() == DialogResult.Cancel)
            return string.Empty;

         return inputBoxDialog.InputResponse;
      }

      private void onLoad(object sender, EventArgs e)
      {
         //use reverse because cbInput shows last item first
         var listOfValues = Values.Reverse().Cast<object>().ToList();
         cbInput.Properties.Items.AddRange(listOfValues);
         cbInput.Properties.AllowRemoveMRUItems = false;
         lblPrompt.Text = FormPrompt;
         Text = FormCaption;
         cbInput.SelectionStart = 0;
         cbInput.SelectionLength = cbInput.Text.Length;
         cbInput.EditValueChanging += validateInput;
         cbInput.EditValue = DefaultValue;
         cbInput.Focus();
      }

      private void validateInput(object sender, ChangingEventArgs e)
      {
         var value = e.NewValue.ToString().Trim();
         if (string.IsNullOrEmpty(value))
            errorProvider.SetError(cbInput, "Please enter a value");
         else if (NotAllowedValues.Contains(value))
            errorProvider.SetError(cbInput, $"{e.NewValue} is not allowed");
         else
            errorProvider.SetError(cbInput, string.Empty);

         btnOk.Enabled = !errorProvider.HasErrors;
      }
   }
}