using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Services.Importer;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImportDataTableControl : XtraUserControl
   {
      private readonly IImportDataTableGridPresenter _gridPresenter;

      private MetaDataEditControl _metaDataControl;
      private readonly ISimpleChartPresenter _presenter;
      private readonly IImportDataTableToDataRepositoryMapper _mapper;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private readonly IImporterTask _importerTask;

      private const int PANEL_MIN_SIZE = 100;
      private const int PANEL_ROW_HEIGHT = 270;
      private const int PANEL_MIN_WIDTH = 300;


      public ImportDataTableControl(ISimpleChartPresenter presenter, IImportDataTableToDataRepositoryMapper mapper, IImportDataTableGridPresenter gridPresenter, IImporterTask importerTask)
      {
         _presenter = presenter;
         _presenter.LogLinSelectionEnabled = true;
         _mapper = mapper;
         _gridPresenter = gridPresenter;
         _importerTask = importerTask;
         _gridPresenter.CopyMetaDataColumnControlEvent += onCopyMetaDataColumnControl;
         _gridPresenter.SetUnitEvent += onSetUnitClick;
      }

      /// <summary>
      /// Builds the control
      /// </summary>
      /// <param name="table">The Import datatable which is used to configure the table</param>
      /// <param name="columnInfos">The columns used to configure the data repository</param>
      public void Build(ImportDataTable table, IReadOnlyList<ColumnInfo> columnInfos)
      {
         InitializeComponent();
         Table = table;
         _columnInfos = columnInfos;
         buildControl();
      }

      public ImportDataTable Table { get; private set; }

      private void buildControl()
      {
         var cursor = Cursor.Current;
         Cursor.Current = Cursors.WaitCursor;
         try
         {
            Controls.Clear();
            _gridPresenter.Edit(Table);
            if (Table.MetaData != null)
            {
               var splitter = new SplitContainerControl();
               Controls.Add(splitter);
               splitter.Dock = DockStyle.Fill;
               splitter.AlwaysScrollActiveControlIntoView = true;

               var panel = splitter.Panel1;
               SplitContainerControl newSplitter = null;
               for (var i = Table.Columns.Count - 1; i >= 0; i--)
               {
                  var col = Table.Columns.ItemByIndex(i);
                  if (col.MetaData == null) continue;
                  newSplitter = createColumnMetaDataControl(col, false);
                  if (newSplitter == null) continue;
                  panel.Controls.Add(newSplitter);
                  newSplitter.Dock = DockStyle.Fill;
                  panel = newSplitter.Panel1;
               }

               if (newSplitter != null)
                  panel = newSplitter.Panel1;

               _metaDataControl = new MetaDataEditControl(Table.MetaData) { AutoAcceptChanges = true };
               Controls.Add(_metaDataControl);
               panel.Name = "Meta Data";
               panel.Text = Captions.Importer.MetaData;
               panel.ShowCaption = false;
               panel.MinSize = PANEL_MIN_SIZE;
               panel.Width = PANEL_MIN_WIDTH;
               panel.Height = (Table.MetaData.Columns.Count + 2) * PANEL_ROW_HEIGHT;
               panel.Controls.Add(_metaDataControl);
               _metaDataControl.Dock = DockStyle.Fill;
               _metaDataControl.OnCopyMetaData += onCopyMetaDataTableControl;
               _metaDataControl.OnBroadCastMetaData += onBroadCastMetaDataTableControl;
               _metaDataControl.OnIsDataValidChanged += metaDataControlOnIsDataValidChanged;

               splitter.SplitterPosition = PANEL_MIN_WIDTH;

               splitter.Panel2.Name = "Data";
               splitter.Panel2.Text = Captions.Importer.Data;
               splitter.Panel2.MinSize = PANEL_MIN_SIZE;

               var datasplitter = new SplitContainerControl { Dock = DockStyle.Fill, Horizontal = false };
               datasplitter.Panel1.FillWith(_gridPresenter.View);
               datasplitter.Panel1.Padding = new Padding(10, 12, 10, 10);
               datasplitter.Panel2.FillWith(_presenter.View);
               datasplitter.Panel2.Padding = new Padding(10);
               datasplitter.CollapsePanel = SplitCollapsePanel.Panel2;
               rePlot();

               UnitChanged += (sender, args) => rePlot();
               _gridPresenter.MetaDataChangedEvent += rePlot;
               splitter.Panel2.Controls.Add(datasplitter);

            }
            else
            {
               var metaDataExists = false;
               foreach (ImportDataColumn col in Table.Columns)
               {
                  metaDataExists = (col.MetaData != null && !string.IsNullOrEmpty(col.Source));
                  if (metaDataExists) break;
               }
               if (metaDataExists)
               {
                  var splitter = new SplitContainerControl();
                  Controls.Add(splitter);
                  splitter.Dock = DockStyle.Fill;
                  splitter.Horizontal = true;
                  splitter.CollapsePanel = SplitCollapsePanel.Panel1;
                  splitter.Panel2.FillWith(_gridPresenter.View);
                  splitter.Panel2.MinSize = PANEL_MIN_SIZE;

                  var panel = splitter.Panel1;
                  panel.Width = PANEL_MIN_WIDTH;
                  splitter.SplitterPosition = panel.Width;
                  SplitContainerControl newSplitter = null;
                  foreach (ImportDataColumn col in Table.Columns)
                  {
                     if (col.MetaData == null) continue;
                     newSplitter = createColumnMetaDataControl(col, true);
                     if (newSplitter == null) continue;
                     panel.Controls.Add(newSplitter);
                     newSplitter.Dock = DockStyle.Fill;
                     panel = newSplitter.Panel2;
                  }
                  if (newSplitter != null) newSplitter.Collapsed = true;
               }
               else
               {
                  this.FillWith(_gridPresenter.View);
               }
            }
            PerformLayout();
         }
         finally
         {
            Cursor.Current = cursor;
         }
      }

      private void rePlot()
      {
         bool error;
         _presenter.PlotObservedData(_mapper.ConvertImportDataTable(Table, _columnInfos, out error));
      }

      private SplitContainerControl createColumnMetaDataControl(ImportDataColumn col, bool swapPanels)
      {
         if (col.MetaData == null) return null;
         if (string.IsNullOrEmpty(col.Source)) return null;
         var colMetaDataControl = new MetaDataEditControl(col.MetaData) { Tag = col, AutoAcceptChanges = true };
         Controls.Add(colMetaDataControl);
         colMetaDataControl.OnCopyMetaData += onCopyMetaDataColumnControlPanel;
         colMetaDataControl.OnIsDataValidChanged += onIsValidChangedColumnControlPanel;
         colMetaDataControl.OnMetaDataChanged += onMetaDataChangedColumnControlPanel;
         if (col.MetaData.Rows.Count > 0)
         {
            col.SetColumnUnitDependingOnMetaData();
            reflectMetaDataChangesForColumn(col);
         }

         var newSplitter = new SplitContainerControl();
         Controls.Add(newSplitter);

         newSplitter.BeginInit();
         newSplitter.Horizontal = false;
         newSplitter.FixedPanel = SplitFixedPanel.Panel1;
         newSplitter.AlwaysScrollActiveControlIntoView = true;
         newSplitter.Panel2.Controls.Add(colMetaDataControl);
         colMetaDataControl.Dock = DockStyle.Fill;
         newSplitter.Panel2.Name = col.ColumnName;
         newSplitter.Panel2.Text = col.GetCaptionForColumn();
         newSplitter.Panel2.ShowCaption = true;
         newSplitter.Panel2.BorderStyle = BorderStyles.Simple;
         newSplitter.Panel2.MinSize = PANEL_MIN_SIZE;
         newSplitter.Panel2.Height = (col.MetaData.Columns.Count + 2) * PANEL_ROW_HEIGHT;
         newSplitter.CollapsePanel = SplitCollapsePanel.Panel2;
         if (swapPanels)
            newSplitter.SwapPanels();
         newSplitter.SplitterPosition = PANEL_ROW_HEIGHT;
         newSplitter.EndInit();

         return newSplitter;
      }

      void onMetaDataChangedColumnControlPanel(object sender, EventArgs e)
      {
         var control = sender as MetaDataEditControl;
         var column = control?.Tag as ImportDataColumn;
         if (column == null) return;

         reflectMetaDataChangesForColumn(column);
      }

      private void reflectMetaDataChangesForColumn(ImportDataColumn column)
      {
         _gridPresenter.ReflectMetaDataChangesForColumn(column);


         UnitChanged?.Invoke(this, new EventArgs());
      }

      /// <summary>
      /// Method reacting on data validity changes of the meta data of the table.
      /// </summary>
      void metaDataControlOnIsDataValidChanged(object sender, EventArgs e)
      {
         var metaDataControl = sender as MetaDataEditControl;
         if (metaDataControl == null) return;
         metaDataControl.AcceptDataChanges();
         MetaDataChanged?.Invoke(this, new EventArgs());
      }

      #region event CopyMetaDataOnTable
      /// <summary>
      /// Event arguments for event OnCopyMetaDataOnTable.
      /// </summary>
      public class CopyMetaDataOnTableEventArgs : EventArgs
      {
         public MetaDataTable MetaData { get; set; }
      }

      /// <summary>
      /// Handler for event OnCopyMetaDataOnTable.
      /// </summary>
      public delegate void CopyMetaDataOnTableHandler(object sender, CopyMetaDataOnTableEventArgs e);

      /// <summary>
      /// Event raised when the user clicks on copy button of table meta data.
      /// </summary>
      public event CopyMetaDataOnTableHandler OnCopyMetaDataOnTable;

      public delegate void BroadcastMetaDataOnTableHandler(object sender, BroadcastMetaDataOnTableEventArgs e);

      public event BroadcastMetaDataOnTableHandler OnBroadcastMetaDataOnTable;

      /// <summary>
      /// Method raising OnCopyMetaDataOnTable event if the click on the copy button of the table meta data control.
      /// </summary>
      private void onCopyMetaDataTableControl(object sender, EventArgs e)
      {
         MetaDataChanged?.Invoke(this, new EventArgs());
         OnCopyMetaDataOnTable?.Invoke(this, new CopyMetaDataOnTableEventArgs { MetaData = _gridPresenter.GetMetaDataTable() });
      }

      private void onBroadCastMetaDataTableControl(object sender, BroadcastMetaDataEventArgs e)
      {
         OnBroadcastMetaDataOnTable?.Invoke(this, new BroadcastMetaDataOnTableEventArgs { Name = e.Name, Value = e.Value });
      }

      #endregion

      #region event CopyMetaDataOnColumn
      /// <summary>
      /// Event arguments for event OnCopyMetaDataOnColumn.
      /// </summary>
      public class CopyMetaDataOnColumnEventArgs : EventArgs
      {
         /// <summary>
         /// Meta data of current column.
         /// </summary>
         public MetaDataTable MetaData { get; set; }
         /// <summary>
         /// Name of current column.
         /// </summary>
         public string ColumnName { get; set; }
      }

      /// <summary>
      /// Handler of event OnCopyMetaDataOnColumn.
      /// </summary>
      public delegate void CopyMetaDataOnColumnHandler(object sender, CopyMetaDataOnColumnEventArgs e);

      /// <summary>
      /// Event raised when user clicks on copy meta data button for a column.
      /// </summary>
      public event CopyMetaDataOnColumnHandler OnCopyMetaDataOnColumn = delegate { };

      /// <summary>
      /// Method raising OnCopyMetaDataOnColumn if a user initiated the copy of meta data of a column.
      /// </summary>
      private void onCopyMetaDataColumnControl(MetaDataTable metaData, string columnName)
      {
         OnCopyMetaDataOnColumn(this, new CopyMetaDataOnColumnEventArgs { MetaData = metaData, ColumnName = columnName });
      }

      private void onCopyMetaDataColumnControlPanel(object sender, EventArgs e)
      {
         var control = sender as MetaDataEditControl;
         var column = control?.Tag as ImportDataColumn;
         if (column == null) return;

         OnCopyMetaDataOnColumn?.Invoke(this,
            new CopyMetaDataOnColumnEventArgs { MetaData = column.MetaData, ColumnName = column.ColumnName });
      }
      private void onIsValidChangedColumnControlPanel(object sender, EventArgs e)
      {
         var control = sender as MetaDataEditControl;
         if (control == null) return;
         control.AcceptDataChanges();
         MetaDataChanged?.Invoke(this, new EventArgs());

         var column = control.Tag as ImportDataColumn;
         if (column == null) return;

         _gridPresenter.SetColumnImage(column.ColumnName);

      }
      #endregion

      /// <summary>
      /// Handler for event MetaDataChanged.
      /// </summary>
      public delegate void MetaDataChangedHandler(object sender, EventArgs e);

      /// <summary>
      /// Event raised when meta data has been changed.
      /// </summary>
      public event MetaDataChangedHandler MetaDataChanged;

      /// <summary>
      /// Handler for event UnitChanged.
      /// </summary>
      public delegate void UnitChangedHandler(object sender, EventArgs e);

      /// <summary>
      /// Event raised when unit information has been changed.
      /// </summary>
      public event UnitChangedHandler UnitChanged;

      #region event CopyUnitInputParametersOnColumn
      /// <summary>
      /// Event arguments for event OnCopyUnitInputParametersOnColumn.
      /// </summary>
      public class CopyUnitInputParametersOnColumnEventArgs : EventArgs
      {
         /// <summary>
         /// Input parameters to get copied.
         /// </summary>
         public IList<InputParameter> InputParameters { get; set; }
         /// <summary>
         /// Dimension to get copied.
         /// </summary>
         public Dimension Dimension { get; set; }
         /// <summary>
         /// Name of the column to copy from/to.
         /// </summary>
         public string ColumnName { get; set; }
      }

      /// <summary>
      /// Handler for event OnCopyUnitInputParametersOnColumn.
      /// </summary>
      public delegate void CopyUnitInputParametersOnColumnHandler(object sender, CopyUnitInputParametersOnColumnEventArgs e);

      /// <summary>
      /// Event raised when unit input parameters of a column should be copied to the columns of the other tables.
      /// </summary>
      public event CopyUnitInputParametersOnColumnHandler OnCopyUnitInputParametersOnColumn;
      #endregion



      public void SetMetaDataForTable(string metaDataColumn, object value)
      {
         //set metadata
         _metaDataControl?.SetEditorValue(metaDataColumn, value);
      }

      /// <summary>
      /// This method sets the unit and dimension information to given column.
      /// </summary>
      public void SetUnitInformationForColumn(Dimension dimension, Unit unit, string columnName)
      {
         _gridPresenter.SetUnitInformationForColumn(dimension, unit, columnName);
      }

      /// <summary>
      /// This method sets the given input parameters to given column.
      /// </summary>
      public void SetInputParametersForColumn(IList<InputParameter> inputParameters, Dimension dimension, string columnName)
      {
         _gridPresenter.SetInputParametersForColumn(inputParameters, dimension, columnName);
         UnitChanged?.Invoke(this, new EventArgs());
      }

      internal void SetMetaDataForTable(MetaDataTable metaData)
      {
         if (_metaDataControl == null) return;

         _gridPresenter.SetMetaDataForTable(metaData);
         MetaDataChanged?.Invoke(this, new EventArgs());

         _metaDataControl.RefreshData(0);
      }

      /// <summary>
      /// This event handler is used to react on menu item set unit clicks.
      /// </summary>
      private void onSetUnitClick(object sender, EventArgs e)
      {
         var item = sender as DXMenuItem;

         var col = item?.Tag as GridColumn;

         var table = col?.View.GridControl.DataSource as ImportDataTable;
         if (table == null) return;
         if (!table.Columns.ContainsName(col.FieldName)) return;

         var tableColumn = table.Columns.ItemByName(col.FieldName);
         var icon = ParentForm == null ? ApplicationIcons.EmptyIcon : ParentForm.Icon;
         var frm = new SetUnitView(tableColumn, _importerTask) { StartPosition = FormStartPosition.CenterParent, Icon = icon };
         frm.OnCopyUnitInfo += onCopyUnitInfo;
         if (frm.ShowDialog() != DialogResult.OK) return;
         UnitChanged?.Invoke(this, new EventArgs());
         OnCopyUnitInputParametersOnColumn?.Invoke(this,
            new CopyUnitInputParametersOnColumnEventArgs
            {
               ColumnName = tableColumn.ColumnName,
               Dimension = tableColumn.ActiveDimension,
               InputParameters = tableColumn.ActiveDimension.InputParameters
            });

         _gridPresenter.SetUnitForColumn(table);
      }

      /// <summary>
      /// Handler for event OnCopyUnitInfo.
      /// </summary>
      public delegate void CopyUnitInfoOnColumnHandler(object sender, SetUnitView.CopyUnitInfoEventArgs e);

      /// <summary>
      /// Event raised when user clicks on copy button.
      /// </summary>
      public event CopyUnitInfoOnColumnHandler OnCopyUnitInfoOnColumn;

      /// <summary>
      /// Method reacting on Copy (Apply to others) button click in SetUnitView.
      /// </summary>
      void onCopyUnitInfo(object sender, SetUnitView.CopyUnitInfoEventArgs e)
      {
         OnCopyUnitInfoOnColumn?.Invoke(this, e);
      }

      private void cleanMemory()
      {
         _gridPresenter.CopyMetaDataColumnControlEvent -= onCopyMetaDataColumnControl;
         _gridPresenter.SetUnitEvent -= onSetUnitClick;

         if (_metaDataControl != null)
         {
            _metaDataControl.OnCopyMetaData -= onCopyMetaDataTableControl;
            _metaDataControl.OnBroadCastMetaData -= onBroadCastMetaDataTableControl;
            _metaDataControl.OnIsDataValidChanged -= metaDataControlOnIsDataValidChanged;
         }
         _gridPresenter.MetaDataChangedEvent -= rePlot;
         _gridPresenter?.Clear();
         _metaDataControl?.Dispose();
         _metaDataControl = null;
         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }

      public void SetMetaDataForColumn(MetaDataTable metaData, string columnName)
      {
         _gridPresenter.SetMetaDataForColumn(metaData, columnName);
      }
   }

   public class BroadcastMetaDataOnTableEventArgs : EventArgs
   {
      public string Name { get; set; }
      public object Value { get; set; }
   }
}
