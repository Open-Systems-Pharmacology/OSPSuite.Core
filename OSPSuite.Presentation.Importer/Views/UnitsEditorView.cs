using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Presenters;
using DevExpress.XtraEditors;
using OSPSuite.UI.Importer;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using DevExpress.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors.Controls;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class UnitsEditorView : BaseUserControl, IUnitsEditorView
   {
      private Column _importDataColumn;
      private IEnumerable<IDimension> _dimensions;
      private ImageComboBoxEdit _unitComboBox;
      private ImageComboBoxEdit _dimensionComboBox;
      private InputParametersControl _inputParametersControl;
      private string _selectedUnit;

      public UnitsEditorView()
      {
         InitializeComponent();
      }

      public void SetParams(Column importDataColumn, IEnumerable<IDimension> dimensions)
      {
         _selectedUnit = importDataColumn.Unit;
         _importDataColumn = importDataColumn;
         _dimensions = dimensions;

         Text = Captions.Importer.PleaseEnterDimensionAndUnitInformation;
         btnOK.Click += (o, e) => this.DoWithinExceptionHandler(onOkClick);
         btnAllOK.Click += (o, e) => this.DoWithinExceptionHandler(onCopyClick);

         if (useDimensionSelector)
            _dimensionComboBox = createComboBox("Dimension", onDimensionComboBoxTextChanged);

         _unitComboBox = createComboBox("Unit", onUnitComboBoxTextChanged);

         if (useDimensionSelector)
            fillDimensionComboBox();

         fillUnitComboBox();

         if (useDimensionSelector)
         {
            var dimItem = _dimensionComboBox.Properties.Items.GetItem(_importDataColumn.Name.ToString());
            if (_dimensionComboBox.Properties.Items.Contains(dimItem))
               _dimensionComboBox.EditValue = _importDataColumn.Name.ToString();
         }

         var unitItem = _unitComboBox.Properties.Items.GetItem(_importDataColumn.Unit);
         if (_unitComboBox.Properties.Items.Contains(unitItem))
            _unitComboBox.EditValue = _selectedUnit;

         arrangeControls();

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

      private bool useDimensionSelector
      {
         get { return _dimensions.Count() > 1; }
      }

      private void onDimensionComboBoxTextChanged(object sender, EventArgs e)
      {
         _selectedUnit = _dimensions.First(d => d.Name == _dimensionComboBox.EditValue as string).DefaultUnit.Name;
         fillUnitComboBox();
         showInputParametersControl();
         enableButtons();
      }

      private void onUnitComboBoxTextChanged(object sender, EventArgs e)
      {
         _selectedUnit = _unitComboBox.EditValue as string;
         showInputParametersControl();
         enableButtons();
      }

      private void arrangeControls()
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

      private IDimension FindDimension()
      {
         foreach (var dimension in _dimensions)
         {
            if (dimension.Units.FirstOrDefault(u => u.Name == _selectedUnit) != null) return dimension;
         }
         return _dimensions.First();
      }

      private IDimension selectedDimension
      {
         get
         {
            if (useDimensionSelector)
            {
               return FindDimension();
            }
            return _dimensions.ElementAt(0);
         }
      }

      private void showInputParametersControl()
      {
         if (_inputParametersControl != null) _inputParametersControl.Dispose();
         _inputParametersControl = null;
      }

      private void fillDimensionComboBox()
      {
         ImageComboBoxItem defaultItem = null;
         _dimensionComboBox.Properties.Items.Clear();
         foreach (var dimension in _dimensions)
         {
            var newItem = new ImageComboBoxItem
            {
               Description = dimension.DisplayName,
               Value = dimension.Name
            };
            if (selectedDimension == dimension) defaultItem = newItem;
            _dimensionComboBox.Properties.Items.Add(newItem);
         }
         if (_dimensionComboBox.Properties.Items.Contains(defaultItem))
            _dimensionComboBox.EditValue = defaultItem.Value;
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
            _unitComboBox.EditValue = selectedDimension.DefaultUnit.Name;
            return;
         }

         if (_dimensions == null || !_dimensions.Any())
            return;

         foreach (var dimension in _dimensions)
            foreach (var unit in dimension.Units)
               addUnit(unit);
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
         return (_dimensionComboBox == null || !String.IsNullOrEmpty(_dimensionComboBox.Text)) && _unitComboBox.Text != null && (_inputParametersControl == null || _inputParametersControl.AreAllValuesEntered);
      }

      private void onOkClick()
      {
         OnOK(_selectedUnit);
         _importDataColumn.Unit = _selectedUnit;
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

      public event OKHandler OnOK = delegate { };



      private void cleanMemory()
      {
         CleanUpHelper.ReleaseEvents(_inputParametersControl);
         if (_inputParametersControl != null) _inputParametersControl.Dispose();
         CleanUpHelper.ReleaseEvents(_dimensionComboBox);
         if (_dimensionComboBox != null) _dimensionComboBox.Dispose();
         CleanUpHelper.ReleaseEvents(_unitComboBox);
         if (_unitComboBox != null) _unitComboBox.Dispose();
         CleanUpHelper.ReleaseEvents(_importDataColumn);
         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }

      public void AttachPresenter(IUnitsEditorPresenter presenter)
      {
      }
   }
}
