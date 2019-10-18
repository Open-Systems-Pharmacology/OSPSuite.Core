using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;

namespace OSPSuite.UI.Importer
{
   public partial class InputParametersControl : XtraUserControl
   {
      private readonly IList<BaseEdit> _inputParameterEditors;
      private readonly IList<InputParameter> _inputParameters;
      private readonly string _numberFormat = String.Concat("#".PadLeft(24, '#'), "0.00", "#".PadRight(12, '#'));
      private const string INFINITY_SIGN = "∞";

      public InputParametersControl(IList<InputParameter> inputParameters)
      {
         InitializeComponent();

         _inputParameterEditors = new List<BaseEdit>();
         _inputParameters = inputParameters;
         foreach (var inputParameter in inputParameters)
         {
            LayoutControlItem colItem = layoutControl.Root.AddItem();
            colItem.Name = inputParameter.DisplayName;

            var inputParameterEditor = new CalcEdit();
            _inputParameterEditors.Add(inputParameterEditor);
            inputParameterEditor.Name = inputParameter.Name;
            inputParameterEditor.Validating += onEditorValidating;
            inputParameterEditor.EditValueChanged += onInputParameterEditorEditValueChanged;
            if (inputParameter.Value != null)
               inputParameterEditor.EditValue = inputParameter.Value;

            inputParameterEditor.Properties.AllowNullInput = DefaultBoolean.False;

            if (!String.IsNullOrEmpty(inputParameter.Unit.Name))
               inputParameterEditor.Properties.EditMask = String.Format("{0} [{1}];-{0} [{1}]", _numberFormat,
                                                                        inputParameter.Unit.Name);
            else
               inputParameterEditor.Properties.EditMask = String.Format("{0};-{0}", _numberFormat);
            inputParameterEditor.Properties.DisplayFormat.FormatString = inputParameterEditor.Properties.EditMask;
            inputParameterEditor.Properties.DisplayFormat.FormatType = FormatType.Custom;

            inputParameterEditor.Properties.NullValuePrompt = Captions.Importer.PleaseEnterData;
            inputParameterEditor.Properties.CloseUpKey = new KeyShortcut(Keys.Enter);

            inputParameterEditor.ToolTipController = new ToolTipController();
            inputParameterEditor.SuperTip = new SuperToolTip();
            inputParameterEditor.SuperTip.Items.AddTitle(inputParameter.DisplayName);
            if (!String.IsNullOrEmpty(inputParameter.Unit.Name))
               inputParameterEditor.SuperTip.Items.Add(String.Format("Please enter a value in {0} [{1}].",
                                                                     inputParameter.Unit.DisplayName,
                                                                     inputParameter.Unit.Name));
            else
               inputParameterEditor.SuperTip.Items.Add("Please enter a value.");

            if (inputParameter.MinValue != null || inputParameter.MaxValue != null)
            {
               var lowerBound = (inputParameter.MinValue == null) ? INFINITY_SIGN : (inputParameter.MinValueAllowed) ? "[" : "]";
               var lowerValue = (inputParameter.MinValue == null) ? String.Empty : inputParameter.MinValue.ToString();
               var upperValue = (inputParameter.MaxValue == null) ? String.Empty : inputParameter.MaxValue.ToString();
               var upperBound = (inputParameter.MaxValue == null) ? INFINITY_SIGN : (inputParameter.MaxValueAllowed) ? "]" : "[";
               var text = String.Format("Valid values must be within range {0}{1};{2}{3}.", lowerBound, lowerValue, upperValue, upperBound);
               inputParameterEditor.SuperTip.Items.Add(text);
            }
            inputParameterEditor.ToolTipController.ToolTipType = ToolTipType.SuperTip;
            inputParameterEditor.PerformLayout();
            colItem.Control = inputParameterEditor;
            checkEditorInput(inputParameterEditor);
         }
      }

      private InputParameter getInputParameter(String name)
      {
         foreach (var param in _inputParameters)
         {
            if (param.Name != name) continue;
            return param;
         }
         return new InputParameter();
      }

      /// <summary>
      /// Method reacting on editor validating event.
      /// </summary>
      void onEditorValidating(object sender, CancelEventArgs e)
      {
         var editor = sender as BaseEdit;
         if (editor == null) return;

         checkEditorInput(editor);
      }

      /// <summary>
      /// Method for checking the input of a given editor.
      /// </summary>
      private bool checkEditorInput(BaseEdit editor)
      {
         if (editor == null) return false;
         editor.ErrorText = String.Empty;
         var inputParameter = getInputParameter(editor.Name);

         if (editor.EditValue == null) return false;
         var editorValue = getEditorValue(editor);
         if (editorValue != null && inputParameter.IsValueValid((double)editorValue)) return true;

         editor.ErrorText = String.Format("The input parameter {0} is out of range!", inputParameter.DisplayName);
         return false;
      }

      /// <summary>
      /// This property determines whether a values has been set for each input parameter.
      /// </summary>
      public bool AreAllValuesEntered
      {
         get
         {
            foreach (var inputParameterEditor in _inputParameterEditors)
               if (!checkEditorInput(inputParameterEditor)) return false;
            return true;
         }
      }

      /// <summary>
      /// This method retrieves a list of filled input parameters.
      /// </summary>
      /// <returns>A list of filled input parameters.</returns>
      public IList<InputParameter> GetEnteredValues()
      {
         for (var i = 0; i < _inputParameterEditors.Count; i++ )
         {
            var inputParameter = _inputParameters[i];
            inputParameter.Value = _inputParameterEditors[i].EditValue == null
                                      ? null
                                      : getEditorValue(_inputParameterEditors[i]);
            _inputParameters[i] = inputParameter;
         }
         return _inputParameters;
      }

      /// <summary>
      /// This method retrieves the value of the used editor as a double value.
      /// </summary>
      private static double? getEditorValue(BaseEdit editor)
      {
         var propInfo = editor.GetType().GetProperty("Value");
         if (propInfo == null) return null;

         return Decimal.ToDouble((decimal)propInfo.GetValue(editor, null));
      }

      /// <summary>
      /// Handler for event AllValuesEntered.
      /// </summary>
      public delegate void AllValuesEnteredHandler(object sender, EventArgs e);

      /// <summary>
      /// Event raised when all values are entered.
      /// </summary>
      public event AllValuesEnteredHandler AllValuesEntered;

      /// <summary>
      /// Handler for event ValuesMissing.
      /// </summary>
      public delegate void ValuesMissingHandler(object sender, EventArgs e);

      /// <summary>
      /// Event raised when there are missing values.
      /// </summary>
      public event ValuesMissingHandler ValuesMissing;

      /// <summary>
      /// This is the event handler raised if an input parameter value has been changed.
      /// </summary>
      private void onInputParameterEditorEditValueChanged(object sender, EventArgs e)
      {
         if (AreAllValuesEntered)
         {
            if (AllValuesEntered != null)
               AllValuesEntered(this, new EventArgs());
         } else
         {
            if (ValuesMissing != null)
               ValuesMissing(this, new EventArgs());
         }

      }

      private void cleanMemory()
      {
         foreach (var edit in _inputParameterEditors)
         {
            CleanUpHelper.ReleaseEvents(edit);
            edit.Dispose();
         }
         _inputParameterEditors.Clear();

         CleanUpHelper.ReleaseEvents(this);
         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }

   }
}
