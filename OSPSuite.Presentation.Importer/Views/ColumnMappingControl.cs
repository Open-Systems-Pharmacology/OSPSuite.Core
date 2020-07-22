using System;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;
using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Linq;
using System.Windows.Forms;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ColumnMappingControl : BaseUserControl, IColumnMappingControl
   {
      private IEnumerable<DataFormatParameter> mappings;
      public ColumnMappingControl()
      {
         InitializeComponent();
         uxGridView.OptionsView.ShowGroupPanel = false;
         uxGridView.OptionsMenu.EnableColumnMenu = false;
         uxGridView.CellValueChanged += (s, e) => ValidateMapping();
         uxGridView.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
         uxGridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;

         //uxGridView.CustomRowCellEdit += onCustomRowCellEdit;
         //uxGridView.CustomRowCellEditForEditing += onCustomRowCellEditForEditing;
         //uxGridView.MouseDown += onMouseDown;
         uxGrid.ToolTipController = new ToolTipController();
         uxGrid.ToolTipController.GetActiveObjectInfo += onGetActiveObjectInfo;
      }

      public void AttachPresenter(IColumnMappingPresenter presenter) { }

      public void SetMappingSource(IEnumerable<DataFormatParameter> mappings)
      {
         uxGrid.DataSource = mappings;
         this.mappings = mappings;

         configureColumn("ColumnName", true, false);
         configureColumn("Type", true, false);
      }

      private void ValidateMapping()
      {
         
      }

      private void configureColumn(string columnName, bool visible, bool allowEdit = false)
      {
         var col = uxGridView.Columns.ColumnByFieldName(columnName);
         if (visible)
         {
            col.OptionsColumn.AllowEdit = allowEdit;
            col.OptionsColumn.AllowGroup = DefaultBoolean.False;
            col.OptionsColumn.AllowShowHide = false;
            col.OptionsColumn.AllowSort = DefaultBoolean.False;
            col.OptionsFilter.AllowFilter = false;
         }
         else
            col.Visible = false;
      }

      private void onGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (sender != uxGrid.ToolTipController) return;
         var view = uxGrid.GetViewAt(e.ControlMousePosition) as GridView;
         if (view == null) return;
         GridHitInfo hitInfo = view.CalcHitInfo(e.ControlMousePosition);

         if (!hitInfo.InRow) return;
         var mappingRow = mappings.ElementAt(hitInfo.RowHandle);
         e.Info = new ToolTipControlInfo(mappingRow, String.Empty);

         e.Info.SuperTip = GenerateToolTipControlInfo(mappingRow);
         e.Info.ToolTipType = ToolTipType.SuperTip;
         uxGrid.ToolTipController.ShowHint(e.Info);
      }

      private SuperToolTip GenerateToolTipControlInfo(DataFormatParameter parameter)
      {
         var superToolTip = new SuperToolTip();
         switch (parameter.Type)
         {
            case DataFormatParameterType.MAPPING:
               var mapping = parameter as MappingDataFormatParameter;
               superToolTip.Items.AddTitle("Mapping:");
               superToolTip.Items.Add($"The column {parameter.ColumnName} will be mapped into {mapping.MappedColumn.Name} with units as {mapping.MappedColumn.Unit}");
               break;
            case DataFormatParameterType.GROUP_BY:
               superToolTip.Items.AddTitle("Group by:");
               superToolTip.Items.Add($"The column {parameter.ColumnName} will be used for grouping by");
               break;
            case DataFormatParameterType.META_DATA:
               superToolTip.Items.AddTitle("Meta data:");
               superToolTip.Items.Add($"The column {parameter.ColumnName} will be used as meta data");
               break;
         }
         return superToolTip;
      }
   }
}
