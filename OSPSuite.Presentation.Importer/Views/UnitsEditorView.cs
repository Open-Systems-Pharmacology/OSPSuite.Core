using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Presentation.Importer.Presenters;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using DevExpress.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors.Controls;
using System.Collections.Generic;
using OSPSuite.UI.Views;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class UnitsEditorView : BaseModalView, IUnitsEditorView
   {
      private ImageComboBoxEdit _unitComboBox;
      private ImageComboBoxEdit _dimensionComboBox;
      private ImageComboBoxEdit _columnComboBox;
      private IUnitsEditorPresenter _presenter;

      public UnitsEditorView()
      {
         InitializeComponent();
         Text = Captions.Importer.PleaseEnterDimensionAndUnitInformation;
      }

      public sealed override string Text
      {
         get => base.Text;
         set => base.Text = value;
      }

      public void SetParams(bool columnMapping, bool useDimensionSelector)
      {
         var lc = new LayoutControl { Name = "LayoutControl" };
         unitPanel.Controls.Add(lc);
         lc.Dock = DockStyle.Fill;
         lc.AllowCustomization = false;

         if (columnMapping)
         {
            _columnComboBox = createComboBox("Column", onColumnComboBoxTextChanged);
            _dimensionComboBox = createComboBox("Dimension", (s, e) => { });
            _unitComboBox = createComboBox("Unit", (s, e) => { });
            unitPanel.Size = new Size(unitPanel.Size.Width, 50);
            addControlItem(lc, "Column", _columnComboBox);
            _unitComboBox.Visible = false;
            _dimensionComboBox.Visible = false;
            _columnComboBox.Visible = true;
         }
         else
         {
            _dimensionComboBox = createComboBox("Dimension", onDimensionComboBoxTextChanged);
            _unitComboBox = createComboBox("Unit", onUnitComboBoxTextChanged);
            _columnComboBox = createComboBox("Column", (s, e) => { });
            if (useDimensionSelector)
            {
               unitPanel.Size = new Size(unitPanel.Size.Width, 85);
               addControlItem(lc, "Dimension", _dimensionComboBox);
               _dimensionComboBox.Visible = true;
            }
            else
            {
               unitPanel.Size = new Size(unitPanel.Size.Width, 50);
               _dimensionComboBox.Visible = false;
            }
            _unitComboBox.Visible = true;
            _columnComboBox.Visible = false;

            addControlItem(lc, "Unit", _unitComboBox);
         }
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

      private void onDimensionComboBoxTextChanged(object sender, EventArgs e)
      {
         _presenter.SelectDimension(_dimensionComboBox.EditValue as string);
      }

      private void onUnitComboBoxTextChanged(object sender, EventArgs e)
      {
         _presenter.SelectUnit(_unitComboBox.EditValue as string);
      }

      private void onColumnComboBoxTextChanged(object sender, EventArgs e)
      {
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
         _dimensionComboBox.Properties.Items.Clear();
         foreach (var dimension in dimensionList)
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

      private bool isEnabled() //TODO Resharper
      {
         return (_dimensionComboBox == null || !_dimensionComboBox.Properties.Items.Any() || !String.IsNullOrEmpty(_dimensionComboBox.Text)) && _unitComboBox.Text != null;
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
