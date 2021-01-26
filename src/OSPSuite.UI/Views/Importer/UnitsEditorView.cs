using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
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

      public UnitsEditorView()
      {
         InitializeComponent();
         Text = Captions.Importer.PleaseEnterDimensionAndUnitInformation;
         _dimensionsLayoutControlItem.Text = Captions.Importer.Dimension;
         _dimensionsComboBox.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
         _unitLayoutControlItem.Text = Captions.Importer.Unit;
         _unitComboBox.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
         _columnsToogleLayoutControlItem.Text = Captions.Importer.ImportUnitFromColumn;
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

      private void onDimensionComboBoxTextChanged()
      {
         _presenter.SelectDimension(_dimensionsComboBox.EditValue as string);
      }

      private void onUnitComboBoxTextChanged()
      {
         _presenter.SelectUnit(_unitComboBox.EditValue as string);
      }

      private void onColumnComboBoxTextChanged()
      {
         if (_columnComboBox.EditValue != null)
            _presenter.SelectColumn(_columnComboBox.EditValue as string);
      }

      public void FillColumnComboBox(IEnumerable<string> cols)
      {
         var columns = cols.ToList();
         _columnComboBox.Properties.Items.Clear();
         foreach (var column in columns.Where(c => c != null))
            _columnComboBox.Properties.Items.Add(new ImageComboBoxItem
            {
               Description = column,
               Value = column
            });
         _columnComboBox.EditValue = columns.First();
      }

      public void SetUnitColumnSelection()
      {
         _columnsToogleLayoutControlItem.Visibility = LayoutVisibility.Never;
         _dimensionsLayoutControlItem.Visibility = LayoutVisibility.Never;
         _unitLayoutControlItem.Visibility = LayoutVisibility.Never;

      }

      public void ShowAll()
      {
         _columnsToogleLayoutControlItem.Visibility = LayoutVisibility.Always;
        // _dimensionsLayoutControlItem.Visibility = LayoutVisibility.Always;
         //_unitLayoutControlItem.Visibility = LayoutVisibility.Always;
         //_columnLayoutControlItem.Visibility = LayoutVisibility.Always;
      }

      public void SetUnitsManualSelection()
      {
         _columnsToogleLayoutControlItem.Visibility = LayoutVisibility.Never;
         _dimensionsLayoutControlItem.Visibility = LayoutVisibility.Never;
         _columnLayoutControlItem.Visibility = LayoutVisibility.Never;
      }

      public void FillDimensionComboBox(IEnumerable<IDimension> dimensions, string defaultValue)
      {
         var dimensionList = dimensions as IDimension[] ?? dimensions.ToArray();
         if (!dimensionList.Any())
         {
            return;
         }
         _dimensionsComboBox.Properties.Items.Clear();
         foreach (var dimension in dimensionList)
         {
            var newItem = new ImageComboBoxItem
            {
               Description = dimension.DisplayName,
               Value = dimension.Name
            };
            _dimensionsComboBox.Properties.Items.Add(newItem);
         }
         _dimensionsComboBox.EditValue = defaultValue;
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

      public void AttachPresenter(IUnitsEditorPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
