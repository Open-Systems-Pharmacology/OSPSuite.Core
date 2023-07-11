using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace OSPSuite.UI.Controls
{
   public partial class InputBoxDialog : BaseModalView
   {
      private IEnumerable<string> _notAllowedValues;
      private string _formPrompt;
      private string _defaultValue;
      private IEnumerable<string> _values;
      private string _formCaption;
      private bool _hasError;

      public override bool HasError => _hasError;

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
            _formPrompt = prompt,
            _formCaption = title,
            _defaultValue = defaultValue ?? string.Empty,
            _values = predefinedValues ?? Enumerable.Empty<string>(),
            _notAllowedValues = (forbiddenValues ?? Enumerable.Empty<string>()).Select(x => x.ToLower())
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
         var listOfValues = _values.Reverse().Cast<object>().ToList();
         cbInput.Properties.Items.AddRange(listOfValues);
         cbInput.Properties.AllowRemoveMRUItems = false;
         lblPrompt.Text = _formPrompt;
         Text = _formCaption;
         cbInput.SelectionStart = 0;
         cbInput.SelectionLength = cbInput.Text.Length;
         cbInput.EditValueChanging += validateInput;
         cbInput.EditValue = _defaultValue;
         cbInput.Focus();
      }

      private void validateInput(object sender, ChangingEventArgs e)
      {
         var value = e.NewValue.ToString().Trim();
         if (string.IsNullOrEmpty(value))
            OnValidationError(cbInput, "Please enter a value");
         else if (_notAllowedValues.Contains(value.ToLower()))
            OnValidationError(cbInput, $"{e.NewValue} is not allowed");
         else
            OnClearError(cbInput);
      }

      protected override void OnClearError(Control control)
      {
         _hasError = false;
         base.OnClearError(control);
      }

      protected override void OnValidationError(Control control, string error)
      {
         _hasError = true;
         base.OnValidationError(control, error);
      }
   }
}