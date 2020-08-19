using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using DevExpress.XtraEditors;
using OSPSuite.UI.Importer;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using DevExpress.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors.Controls;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class UnitsEditorView : BaseUserControl, IUnitsEditorView
   {
      private ImageComboBoxEdit _unitComboBox;
      private ImageComboBoxEdit _dimensionComboBox;
      private InputParametersControl _inputParametersControl;
      
      public UnitsEditorView()
      {
         InitializeComponent();
         Text = Captions.Importer.PleaseEnterDimensionAndUnitInformation;
         btnOK.Click += (o, e) => this.DoWithinExceptionHandler(onOkClick);
         btnAllOK.Click += (o, e) => this.DoWithinExceptionHandler(onCopyClick);

         _dimensionComboBox = createComboBox("Dimension", onDimensionComboBoxTextChanged);
         _unitComboBox = createComboBox("Unit", onUnitComboBoxTextChanged);
      }

      public void SetParams(bool useDimensionSelector)
      {
         arrangeControls(useDimensionSelector);
         enableButtons();
      }

      private ImageComboBoxEdit createComboBox(string name, EventHandler textChangedHandler)
      {
         var comboBox = new ImageComboBoxEdit { Name = name };
         comboBox.Properties.AllowNullInput = DefaultBoolean.False;
         comboBox.TextChanged += textChangedHandler;
         comboBox.Enabled = true;
         Controls.Add(comboBox);
         return comboBox;
      }

      public event UnitChangeHandler OnUnitChange;

      public event DimensionChangeHandler OnDimensionChange;

      private void onDimensionComboBoxTextChanged(object sender, EventArgs e)
      {
         OnDimensionChange?.Invoke(_dimensionComboBox.EditValue as string);
         showInputParametersControl();
         enableButtons();
      }

      private void onUnitComboBoxTextChanged(object sender, EventArgs e)
      {
         OnUnitChange?.Invoke(_unitComboBox.EditValue as string);
         showInputParametersControl();
         enableButtons();
      }

      private void arrangeControls(bool useDimensionSelector)
      {
         var lc = new LayoutControl { Name = "LayoutControl" };
         panel1.Controls.Add(lc);
         lc.Dock = DockStyle.Fill;
         lc.AllowCustomization = false;

         if (useDimensionSelector)
         {
            panel1.Size = new Size(panel1.Size.Width, 85);
            addControlItem(lc, "Dimension", _dimensionComboBox);
         }
         else
         {
            panel1.Size = new Size(panel1.Size.Width, 50);
         }

         addControlItem(lc, "Unit", _unitComboBox);

         showInputParametersControl();
      }

      private void addControlItem(LayoutControl lc, string name, Control control)
      {
         var colItem = lc.Root.AddItem();
         colItem.Name = name;
         colItem.Control = control;
      }

      private void showInputParametersControl()
      {
         if (_inputParametersControl != null) _inputParametersControl.Dispose();
         _inputParametersControl = null;
      }

      public void FillDimensionComboBox(IEnumerable<IDimension> dimensions, string defaultValue)
      {
         if (dimensions.Count() == 0)
         {
            _dimensionComboBox.Visible = false;
            return;
         }
         _dimensionComboBox.Visible = true;
         _dimensionComboBox.Properties.Items.Clear();
         foreach (var dimension in dimensions)
         {
            var newItem = new ImageComboBoxItem
            {
               Description = dimension.DisplayName,
               Value = dimension.Name
            };
            _dimensionComboBox.Properties.Items.Add(newItem);
         }
         _dimensionComboBox.EditValue = defaultValue;
      }

      public void FillUnitComboBox(IEnumerable<Unit> units, string defaultValue)
      {
         _unitComboBox.Properties.Items.Clear();
         foreach (var unit in units)
            addUnit(unit);
         _unitComboBox.EditValue = defaultValue;
      }

      private void addUnit(Unit unit)
      {
         _unitComboBox.Properties.Items.Add(new ImageComboBoxItem
         {
            Description = unit.Name,
            Value = unit.Name
         });
      }

      private void enableButtons()
      {
         btnOK.Enabled = isEnabled();
         btnAllOK.Enabled = btnOK.Enabled;
         btnAllOK.Visible = (OnCopyUnitInfo != null);
      }

      private bool isEnabled()
      {
         return (_dimensionComboBox == null || _dimensionComboBox.Properties.Items.Count() == 0 || !String.IsNullOrEmpty(_dimensionComboBox.Text)) && _unitComboBox.Text != null && (_inputParametersControl == null || _inputParametersControl.AreAllValuesEntered);
      }

      public event OKHandler OnOK;

      private void onOkClick()
      {
         OnOK?.Invoke();
         Parent.Hide();
      }

      private void onCopyClick()
      {
         onOkClick();

         /*OnCopyUnitInfo(this, new CopyUnitInfoEventArgs
         {
            ColumnName = _importDataColumn.ColumnName,
            Dimension = selectedDimension,
            Unit = _selectedUnit
         });*/
      }

      public class CopyUnitInfoEventArgs : EventArgs
      {
         /// <summary>
         ///    Name of current column.
         /// </summary>
         public string ColumnName { get; set; }

         /// <summary>
         ///    Dimension of current column.
         /// </summary>
         public Dimension Dimension { get; set; }

         /// <summary>
         ///    Unit of current column.
         /// </summary>
         public Unit Unit { get; set; }
      }

      /// <summary>
      ///    Handler for event OnCopyUnitInfo.
      /// </summary>
      public delegate void CopyUnitInfoHandler(object sender, CopyUnitInfoEventArgs e);

      /// <summary>
      ///    Event raised when user clicks on copy button.
      /// </summary>
      public event CopyUnitInfoHandler OnCopyUnitInfo = delegate { };

      public void AttachPresenter(IUnitsEditorPresenter presenter)
      {
      }
   }
}
