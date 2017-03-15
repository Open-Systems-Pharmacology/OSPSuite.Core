using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using DevExpress.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Services.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public delegate void CopyMetaDataEventHandler(MetaDataTable metaDataTable, string columnName);

   public delegate void MetaDataChangedEventHandler();

   public interface IImportDataTableGridPresenter : IPresenter<IImportDataTableGridView>
   {
      void Edit(ImportDataTable table);
      void ReflectMetaDataChangesForColumn(ImportDataColumn column);
      void SetColumnImage(string gridColumn);
      void Clear();
      void SetUnitForColumn(ImportDataTable table);
      bool SetColumnMetaData(MetaDataTable targetMetaDataTable, MetaDataTable sourceMetaDataTable);
      void SetMetaDataForColumn(MetaDataTable metaDataTable, string fieldName);
      void SetMetaDataForTable(MetaDataTable metaDataTable);
      void SetInputParametersForColumn(IList<InputParameter> inputParameters, Dimension dimension, string columnName);
      void SetUnitInformationForColumn(Dimension dimension, Unit unit, string columnName);
      MetaDataTable GetMetaDataTable();

      event CopyMetaDataEventHandler CopyMetaDataColumnControlEvent;
      event EventHandler SetUnitEvent;
      event MetaDataChangedEventHandler MetaDataChangedEvent;

      void RaiseCopyMetaDataColumnControlEvent(MetaDataTable metaDataTable, string columnName);
      void RaiseSetUnitEvent(object sender, EventArgs eventArgs);

      /// <summary>
      ///    Gets the background color for the data row indicated by <paramref name="sourceRow" />
      /// </summary>
      /// <returns>
      ///    The color for lower limit of quantification if the table has a column with a lower limit and the value in the
      ///    row/column is below that value
      /// </returns>
      Color GetBackgroundColorForRow(int sourceRow);

      /// <summary>
      ///    The default background color for the table cells.
      /// </summary>
      Color DefaultBackgroundColorForRow { get; }

      ImportDataColumn TableColumnByName(string gridColumnName);
      bool SetUnitInTableForColumnName(Dimension dimension, Unit unit, string columnName);
      bool SetParametersInTableForColumn(IList<InputParameter> inputParameters, Dimension dimension, string columnName);
      string GetCaptionForColumnByName(string columnName);
      int GetImageIndexForColumnName(string columnName);

      string GetLowerLimitOfQuantificationToolTipTextForRow(int dataTableRow);
      SuperToolTip GetToolTipForImportDataColumn(ImportDataColumn importDataColumn);
   }

   public class ImportDataTableGridPresenter : AbstractPresenter<IImportDataTableGridView, IImportDataTableGridPresenter>, IImportDataTableGridPresenter
   {
      private ImportDataTable _table;
      private readonly IImporterTask _importerTask;
      public event CopyMetaDataEventHandler CopyMetaDataColumnControlEvent = delegate { };
      public event EventHandler SetUnitEvent = delegate { };
      public event MetaDataChangedEventHandler MetaDataChangedEvent = delegate { };

      public ImportDataTableGridPresenter(IImportDataTableGridView view, IImporterTask importerTask) : base(view)
      {
         _importerTask = importerTask;
      }

      public void RaiseMetaDataChangedEvent()
      {
         MetaDataChangedEvent();
      }

      public Color DefaultBackgroundColorForRow => Colors.DefaultRowColor;

      public ImportDataColumn TableColumnByName(string gridColumnName)
      {
         return _table.Columns.ItemByName(gridColumnName);
      }

      public bool SetUnitInTableForColumnName(Dimension dimension, Unit unit, string columnName)
      {
         if (!_table.Columns.ContainsName(columnName)) return false;
         var column = _table.Columns.ItemByName(columnName);

         column.ActiveDimension = DimensionHelper.FindDimension(column.Dimensions, dimension.Name);
         DimensionHelper.TakeOverInputParameters(dimension, column.ActiveDimension);
         column.ActiveUnit = column.ActiveDimension.FindUnit(unit.Name);
         column.IsUnitExplicitlySet = true;
         return true;
      }

      public bool SetParametersInTableForColumn(IList<InputParameter> inputParameters, Dimension dimension, string columnName)
      {
         if (!_table.Columns.ContainsName(columnName)) return false;

         var column = _table.Columns.ItemByName(columnName);
         if (column.ActiveDimension.Name != dimension.Name || !column.HasMissingInputParameters) return true;
         for (var i = 0; i < inputParameters.Count; i++)
            column.ActiveDimension.InputParameters[i] = inputParameters[i];
         return true;
      }

      public string GetCaptionForColumnByName(string columnName)
      {
         return TableColumnByName(columnName).GetCaptionForColumn();
      }

      public int GetImageIndexForColumnName(string columnName)
      {
         var importDataColumn = _table.Columns.ItemByName(columnName);
         var image = _importerTask.GetImageIndex(importDataColumn);
         return image;
      }

      public string GetLowerLimitOfQuantificationToolTipTextForRow(int dataTableRow)
      {
         if (!sourceRowHasValueBelowLowerLimitOfQuantification(dataTableRow))
            return string.Empty;

         ImportDataColumn lloqColumn = null;
         foreach (ImportDataColumn column in _table.Columns)
         {
            double columnValue;
            if (!double.TryParse(getValueForRowAndColumn(dataTableRow, column), out columnValue)) continue;
            if (hasValueBelowLimitOfQuantification(column, columnValue))
            {
               lloqColumn = column;
            }
         }

         if (lloqColumn == null)
            return string.Empty;

         return ToolTips.LLOQTooltip(lloqColumn.ColumnName, getValueForRowAndColumn(dataTableRow, lloqColumn), lloqColumn.ActiveUnit.DisplayName, getLowerLimitFromColumnExtendedProperties(lloqColumn));
      }

      public SuperToolTip GetToolTipForImportDataColumn(ImportDataColumn importDataColumn)
      {
         return _importerTask.GetToolTipForImportDataColumn(importDataColumn);
      }

      private string getValueForRowAndColumn(int sourceRow, DataColumn column)
      {
         return _table.Rows.ItemByIndex(sourceRow)[column].ToString();
      }

      public void RaiseCopyMetaDataColumnControlEvent(MetaDataTable metaDataTable, string columnName)
      {
         CopyMetaDataColumnControlEvent(metaDataTable, columnName);
      }

      public void RaiseSetUnitEvent(object sender, EventArgs eventArgs)
      {
         SetUnitEvent(sender, eventArgs);
      }

      private bool hasValueBelowLimitOfQuantification(DataColumn column, double? columnValue)
      {
         if (!columnValue.HasValue)
            return false;

         if (!column.ExtendedProperties.ContainsKey(Constants.LLOQ))
            return false;

         var lowerLimitOfQuantification = getLowerLimitFromColumnExtendedProperties(column);
         if (!lowerLimitOfQuantification.HasValue) return false;
         if (double.IsNaN(lowerLimitOfQuantification.Value)) return false;

         return columnValue.Value < lowerLimitOfQuantification;
      }

      private static double? getLowerLimitFromColumnExtendedProperties(DataColumn column)
      {
         double lowerLimitOfQuantification;
         if (double.TryParse(column.ExtendedProperties[Constants.LLOQ].ToString(), out lowerLimitOfQuantification))
            return lowerLimitOfQuantification;
         return null;
      }

      public Color GetBackgroundColorForRow(int sourceRow)
      {
         if (_table.Rows.Count <= sourceRow || sourceRow < 0) return DefaultBackgroundColorForRow;

         return sourceRowHasValueBelowLowerLimitOfQuantification(sourceRow) ? Colors.BelowLLOQ : DefaultBackgroundColorForRow;
      }

      private bool sourceRowHasValueBelowLowerLimitOfQuantification(int sourceRow)
      {
         return _table.Columns.Cast<DataColumn>().Any(column =>
         {
            var valueForRowAndColumn = getValueForRowAndColumn(sourceRow, column);
            double value;
            return double.TryParse(valueForRowAndColumn, out value) && hasValueBelowLimitOfQuantification(column, value);
         });
      }

      public void Edit(ImportDataTable table)
      {
         _table = table;
         _view.BindTo(table);
      }

      public void ReflectMetaDataChangesForColumn(ImportDataColumn column)
      {
         //maybe the unit must be changed according to new metadata entered
         column.SetColumnUnitDependingOnMetaData();

         _view.ReflectMetaDataChangesForColumn(column);

         RaiseMetaDataChangedEvent();
      }

      public void SetColumnImage(string column)
      {
         _view.SetColumnImage(column);
      }

      public void Clear()
      {
         _view.Clear();
      }

      public void SetUnitForColumn(ImportDataTable table)
      {
         _view.SetUnitForColumn(table);
      }

      public bool SetColumnMetaData(MetaDataTable targetMetaDataTable, MetaDataTable sourceMetaDataTable)
      {
         return setMetaData(targetMetaDataTable, sourceMetaDataTable);
      }

      public void SetMetaDataForColumn(MetaDataTable metaDataTable, string columnName)
      {
         if (!_table.Columns.ContainsName(columnName)) return;
         var column = _table.Columns.ItemByName(columnName);
         setMetaData(column.MetaData, metaDataTable);

         ReflectMetaDataChangesForColumn(column);
      }

      public void SetMetaDataForTable(MetaDataTable metaDataTable)
      {
         setMetaData(_table.MetaData, metaDataTable);
      }

      public void SetInputParametersForColumn(IList<InputParameter> inputParameters, Dimension dimension, string columnName)
      {
         _view.SetInputParametersForColumn(inputParameters, dimension, columnName);
      }

      public void SetUnitInformationForColumn(Dimension dimension, Unit unit, string columnName)
      {
         _view.SetUnitInformationForColumn(dimension, unit, columnName);
      }

      public MetaDataTable GetMetaDataTable()
      {
         return _table.MetaData;
      }

      private bool setMetaData(MetaDataTable target, MetaDataTable source)
      {
         if (target == null) return true;

         MetaDataRow newRow = (MetaDataRow) target.NewRow();

         for (var i = 0; i < newRow.ItemArray.Length; i++)
         {
            if (!source.Columns.ItemByIndex(i).IsColumnUsedForGrouping)
               newRow[i] = source.Rows.ItemByIndex(0)[i];
         }

         if (target.Rows.Count == 0)
            target.Rows.Add(newRow);
         else
         {
            target.Rows.ItemByIndex(0).ItemArray = newRow.ItemArray;
         }
         target.AcceptChanges();
         return false;
      }
   }
}