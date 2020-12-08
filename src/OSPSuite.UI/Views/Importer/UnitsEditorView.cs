using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Importer
{
   public partial class UnitsEditorView : BaseUserControl, IUnitsEditorView
   {
      private IUnitsEditorPresenter _presenter;
      private bool _useDimensionSelector;
      private ImageComboBoxEdit _columnComboBox;

      public UnitsEditorView()
      {
         InitializeComponent();
         Text = Captions.Importer.PleaseEnterDimensionAndUnitInformation;
         _dimensionsLayoutControlItem.Text = Captions.Importer.Dimension;
         _dimensionsComboBox.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
         _unitLayoutControlItem.Text = Captions.Importer.Unit;
         _unitComboBox.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
         _columnsToogleLayoutControlItem.Text = Captions.Importer.AssignColumn;
         _unitComboBox.EditValueChanged += (s, a) => OnEvent(onUnitComboBoxTextChanged);
         _columnComboBox.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
         _columnLayoutControlItem.Text = Captions.Importer.Column;
         _columnsToggleSwitch.IsOnChanged += onIsOnChanged;
         _dimensionsComboBox.EditValueChanged += (s, a) => OnEvent(onDimensionComboBoxTextChanged);
         _columnComboBox.EditValueChanged += (s, a) => OnEvent(onColumnComboBoxTextChanged);
      }

      public sealed override string Text
      {
         get => base.Text;
         set => base.Text = value;
      }

      public void SetParams(bool columnMapping, bool useDimensionSelector)
      {
         if (columnMapping)
         {
            _columnComboBox = createComboBox("Column", onColumnComboBoxTextChanged);
            _unitComboBox.EditValueChanged += (s, e) => { };
            _unitComboBox.Visible = false;
            _columnComboBox.Visible = true;
         }
         else
         {
            _unitComboBox.EditValueChanged += onUnitComboBoxTextChanged;
            _columnComboBox = createComboBox("Column", (s, e) => { });
            if (useDimensionSelector)
            {
               //_dimensionComboBox.Visible = true;
            }
            else
            {
               //_dimensionComboBox.Visible = false;
            }
            _unitComboBox.Visible = true;
            _columnComboBox.Visible = false;
         }
         _columnsToggleSwitch.IsOn = columnMapping;
         _useDimensionSelector = useDimensionSelector;
         onIsOnChanged(null, null);
      }

      private void onIsOnChanged(object sender, EventArgs e)
      {
         if (_columnsToggleSwitch.IsOn)
         {
            _unitLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            _dimensionsLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            _columnLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            onColumnComboBoxTextChanged();
         }
         else
         {
            if (_useDimensionSelector)
            {
               _dimensionsLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
               _dimensionsLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            _unitLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            _columnLayoutControlItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            onUnitComboBoxTextChanged();
         }
      }

      private ImageComboBoxEdit createComboBox(string name, EventHandler textChangedHandler)
      {
         var comboBox = new ImageComboBoxEdit { Name = name };
         comboBox.Properties.AllowNullInput = DefaultBoolean.False;
         comboBox.TextChanged += (s, a) => OnEvent(() => textChangedHandler(s, a));
         comboBox.Enabled = true;
         Controls.Add(comboBox);
         return comboBox;
      }

      private void onDimensionComboBoxTextChanged(object sender, EventArgs e)
      {
         //_presenter.SelectDimension(_dimensionComboBox.EditValue as string);
      }

      private void onUnitComboBoxTextChanged(object sender, EventArgs e)
      {
         _presenter.SelectUnit(_unitComboBox.EditValue as string);
      }

      private void onColumnComboBoxTextChanged()
      {
         if (_columnComboBox.EditValue != null)
            _presenter.SelectColumn(_columnComboBox.EditValue as string);
      }

      private void addControlItem(LayoutControl lc, string name, Control control)
      {
         var colItem = lc.Root.AddItem();
         colItem.Name = name;
         colItem.Control = control;
      }

      public void FillColumnComboBox(IEnumerable<string> cols)
      {
         var columns = cols.ToList();
         _columnComboBox.Properties.Items.Clear();
         foreach (var column in columns)
            _columnComboBox.Properties.Items.Add(new ImageComboBoxItem
            {
               Description = column,
               Value = column
            });
         _columnComboBox.EditValue = columns.First();
      }

      public void FillDimensionComboBox(IEnumerable<IDimension> dimensions, string defaultValue)
      {
         var dimensionList = dimensions as IDimension[] ?? dimensions.ToArray();
         if (!dimensionList.Any())
         {
            return;
         }
         //_dimensionComboBox.Properties.Items.Clear();
         foreach (var dimension in dimensionList)
         {
            var newItem = new ImageComboBoxItem
            {
               Description = dimension.DisplayName,
               Value = dimension.Name
            };
            //_dimensionComboBox.Properties.Items.Add(newItem);
         }
         //_dimensionComboBox.EditValue = defaultValue;
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

      private bool isEnabled() //TODO Resharper
      {
         return true;// (_dimensionComboBox == null || !String.IsNullOrEmpty(_dimensionComboBox.Text)) && _unitComboBox.Text != null;
      }

      private void onOkClick()
      {
         _presenter.SetUnit();
         Parent.Hide();
      }

      private void onCopyClick() //TODO Resharper
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
      public event CopyUnitInfoHandler OnCopyUnitInfo = delegate { }; //TODO Resharper

      public void AttachPresenter(IUnitsEditorPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
