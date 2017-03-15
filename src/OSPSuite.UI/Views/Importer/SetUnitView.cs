using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Services.Importer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class SetUnitView : XtraForm
   {
      private readonly ImportDataColumn _importDataColumn;
      private readonly ImageComboBoxEdit _unitComboBox;
      private readonly ImageComboBoxEdit _dimensionComboBox;
      private InputParametersControl _inputParametersControl;
      private readonly Dictionary<string, string> _units = new Dictionary<string, string>();

      public SetUnitView(ImportDataColumn importDataColumn, IImporterTask importerTask)
      {
         _importDataColumn = importDataColumn;
         getUnits();

         InitializeComponent();
         Shown += (s, e) => enableButtons();

         Text = Captions.Importer.PleaseEnterDimensionAndUnitInformation;
         btnOK.Click += (o, e) => this.DoWithinExceptionHandler(onOkClick);
         btnCopy.Click += (o, e) => this.DoWithinExceptionHandler(onCopyClick);

         if (useDimensionSelector)
            _dimensionComboBox = createComboBox("Dimension", onDimensionComboBoxTextChanged);

         _unitComboBox = createComboBox("Unit", onUnitComboBoxTextChanged);

         if (useDimensionSelector)
            fillDimensionComboBox();

         fillUnitComboBox();

         if (useDimensionSelector)
         {
            var dimItem = _dimensionComboBox.Properties.Items.GetItem(_importDataColumn.ActiveDimension.Name);
            if (_dimensionComboBox.Properties.Items.Contains(dimItem))
               _dimensionComboBox.EditValue = _importDataColumn.ActiveDimension.Name;
            else
               importerTask.SetColumnUnit(_importDataColumn, selectedUnit.Name, true);
         }

         var unitItem = _unitComboBox.Properties.Items.GetItem(_importDataColumn.ActiveUnit.Name);
         if (_unitComboBox.Properties.Items.Contains(unitItem))
            _unitComboBox.EditValue = _importDataColumn.ActiveUnit.Name;

         arrangeControls();

         enableButtons();
         if (ParentForm != null)
            Icon = ParentForm.Icon;
      }

      private ImageComboBoxEdit createComboBox(string name, EventHandler textChangedHandler)
      {
         var comboBox = new ImageComboBoxEdit {Name = name};
         comboBox.Properties.AllowNullInput = DefaultBoolean.False;
         comboBox.TextChanged += textChangedHandler;
         comboBox.Enabled = true;
         Controls.Add(comboBox);
         return comboBox;
      }

      private void getUnits()
      {
         _units.Clear();

         try
         {
            foreach (var dimension in _importDataColumn.Dimensions)
               foreach (var unit in dimension.Units)
                  _units.Add(unit.DisplayName, dimension.Name);
         }
         catch (ArgumentException)
         {
            _units.Clear();
         } 

      }

      private bool useDimensionSelector
      {
         get { return _units.Count == 0; }
      }

      private void onDimensionComboBoxTextChanged(object sender, EventArgs e)
      {
         fillUnitComboBox();
         showInputParametersControl();
         enableButtons();
      }

      private void onUnitComboBoxTextChanged(object sender, EventArgs e)
      {
         showInputParametersControl();
         enableButtons();
      }

      private void arrangeControls()
      {
         var lc = new LayoutControl {Name = "LayoutControl"};
         splitContainerControl.Panel1.Controls.Add(lc);
         splitContainerControl.PanelVisibility = SplitPanelVisibility.Panel1;
         lc.Dock = DockStyle.Fill;
         lc.AllowCustomization = false;

         if (useDimensionSelector)
            addControlItem(lc, "Dimension", _dimensionComboBox);

         addControlItem(lc, "Unit", _unitComboBox);

         showInputParametersControl();
      }

      private void addControlItem(LayoutControl lc, string name, Control control)
      {
         var colItem = lc.Root.AddItem();
         colItem.Name = name;
         colItem.Control = control;
      }

      private Dimension selectedDimension
      {
         get
         {
            if (useDimensionSelector)
            {
               return DimensionHelper.FindDimension(_importDataColumn.Dimensions,
                                                    _dimensionComboBox.EditValue.ToString());
            }
            return getDimensionFromUnitSelection();
         }
      }

      private Unit selectedUnit
      {
         get
         {
            return selectedDimension.FindUnit(_unitComboBox.EditValue.ToString());
         }
      }

      private Dimension getDimensionFromUnitSelection()
      {
         if (!_units.ContainsKey(_unitComboBox.Text))
            throw new Exception("An unexpected error occured");
         var dimensionName = _units[_unitComboBox.Text];
         var dimension = DimensionHelper.FindDimension(_importDataColumn.Dimensions, dimensionName);
         return dimension;
      }

      private void showInputParametersControl()
      {
         if (selectedDimension.AreInputParametersRequired())
         {
            splitContainerControl.Panel2.Controls.Clear();
            if (_inputParametersControl == null)
            {
               _inputParametersControl = new InputParametersControl(selectedDimension.InputParameters);
               Controls.Add(_inputParametersControl);
            }
            _inputParametersControl.AllValuesEntered += (sender, e) => enableButtons();
            _inputParametersControl.ValuesMissing += (sender, e) => enableButtons();
            splitContainerControl.Panel2.Controls.Add(_inputParametersControl);
            _inputParametersControl.Dock = DockStyle.Fill;
            splitContainerControl.PanelVisibility = SplitPanelVisibility.Both;
         }
         else
         {
            if (_inputParametersControl != null) _inputParametersControl.Dispose();
            _inputParametersControl = null;
            splitContainerControl.Panel2.Controls.Clear();
            splitContainerControl.PanelVisibility = SplitPanelVisibility.Panel1;
         }
      }

      private void fillDimensionComboBox()
      {
         ImageComboBoxItem defaultItem = null;
         _dimensionComboBox.Properties.Items.Clear();
         foreach (var dimension in _importDataColumn.CurrentlySupportedDimensions)
         {
            var newItem = new ImageComboBoxItem
            {
               Description = dimension.DisplayName,
               Value = dimension.Name
            };
            if (dimension.IsDefault) defaultItem = newItem;
            _dimensionComboBox.Properties.Items.Add(newItem);
         }
         if (_dimensionComboBox.Properties.Items.Contains(defaultItem))
            _dimensionComboBox.EditValue = DimensionHelper.GetDefaultDimension(_importDataColumn.Dimensions).Name;
         else if (_dimensionComboBox.Properties.Items.Count > 0)
            _dimensionComboBox.EditValue = _dimensionComboBox.Properties.Items[0];
      }

      private void fillUnitComboBox()
      {
         _unitComboBox.Properties.Items.Clear();
         if (useDimensionSelector)
         {
            foreach (var unit in selectedDimension.Units)
               addUnit(unit);
            _unitComboBox.EditValue = selectedDimension.GetDefaultUnit().Name;
            return;
         }

         if (_importDataColumn.Dimensions == null || !_importDataColumn.Dimensions.Any()) 
            return;

         foreach (var dimension in _importDataColumn.Dimensions)
            foreach (var unit in dimension.Units)
               addUnit(unit);
         _unitComboBox.EditValue = _importDataColumn.Dimensions.First().GetDefaultUnit().Name;
      }

      private void addUnit(Unit unit)
      {
         _unitComboBox.Properties.Items.Add(new ImageComboBoxItem
                                               {
                                                  Description = unit.DisplayName,
                                                  Value = unit.Name
                                               });
      }

      private void enableButtons()
      {
         btnOK.Enabled = isEnabled();
         btnCopy.Enabled = btnOK.Enabled;
         btnCopy.Visible = (OnCopyUnitInfo != null);
      }

      private bool isEnabled()
      {
         return (_dimensionComboBox == null || !String.IsNullOrEmpty(_dimensionComboBox.Text)) && _unitComboBox.Text != null && (_inputParametersControl == null || _inputParametersControl.AreAllValuesEntered);
      }

      private void onOkClick()
      {
         if (_inputParametersControl != null)
            if (selectedDimension.InputParameters != null)
               selectedDimension.InputParameters = _inputParametersControl.GetEnteredValues();

         _importDataColumn.ActiveDimension = selectedDimension;
         _importDataColumn.ActiveUnit = selectedUnit;
         _importDataColumn.IsUnitExplicitlySet = true;
      }

      private void onCopyClick()
      {
         if (_inputParametersControl != null)
            if (selectedDimension.InputParameters != null)
               selectedDimension.InputParameters = _inputParametersControl.GetEnteredValues();

         _importDataColumn.ActiveDimension = selectedDimension;
         _importDataColumn.ActiveUnit = selectedUnit;
         _importDataColumn.IsUnitExplicitlySet = true;

         OnCopyUnitInfo(this, new CopyUnitInfoEventArgs
         {
            ColumnName = _importDataColumn.ColumnName,
            Dimension = selectedDimension,
            Unit = selectedUnit
         });
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

    

      private void cleanMemory()
      {
         CleanUpHelper.ReleaseEvents(_inputParametersControl);
         if (_inputParametersControl != null) _inputParametersControl.Dispose();
         CleanUpHelper.ReleaseEvents(_dimensionComboBox);
         if (_dimensionComboBox != null) _dimensionComboBox.Dispose();
         CleanUpHelper.ReleaseEvents(_unitComboBox);
         if (_unitComboBox != null) _unitComboBox.Dispose();
         CleanUpHelper.ReleaseEvents(_importDataColumn);
         if (_importDataColumn != null) _importDataColumn.Dispose();
         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }
   }
}