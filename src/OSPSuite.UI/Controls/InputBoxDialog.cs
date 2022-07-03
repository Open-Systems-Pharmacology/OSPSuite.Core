using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace OSPSuite.UI.Controls
{
   public partial class InputBoxDialog : BaseModalView
   {
      internal IEnumerable<string> NotAllowedValues { get; set; }
      internal string FormPrompt { get; set; }
      internal string DefaultValue { get; set; }
      internal IEnumerable<string> Values { get; set; }
      internal string FormCaption { get; set; }

      public InputBoxDialog()
      {
         InitializeComponent();
         InitializeResources();
         Load += onLoad;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         lblPrompt.AsDescription();
      }

      internal string InputResponse => cbInput.Text;

      public static string Show(
         string prompt,
         string title,
         string defaultValue = null,
         IEnumerable<string> forbiddenValues = null,
         IEnumerable<string> predefinedValues = null,
         string iconName = null)
      {
         var inputBoxDialog = new InputBoxDialog
         {
            FormPrompt = prompt,
            FormCaption = title,
            DefaultValue = defaultValue ?? string.Empty,
            Values = predefinedValues ?? Enumerable.Empty<string>(),
            NotAllowedValues = forbiddenValues ?? Enumerable.Empty<string>()
         };

         inputBoxDialog.ApplicationIcon = ApplicationIcons.IconByNameOrDefault(iconName, ApplicationIcons.DefaultIcon);
         inputBoxDialog.Display();

         if (inputBoxDialog.Canceled)
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
            OnValidationError(cbInput, "Please enter a value");
         else if (NotAllowedValues.Contains(value))
            OnValidationError(cbInput, $"{e.NewValue} is not allowed");
         else
            OnClearError(cbInput);
      }
   }
}