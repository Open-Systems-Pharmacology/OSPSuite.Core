using System;
using System.Data;
using System.Linq;

namespace OSPSuite.Core.Importer
{
   public class ImportDataTable : DataTable, System.Collections.IEnumerable 
   {
      private readonly ImportDataColumnCollection _columns;
      private readonly ImportDataRowCollection _rows;
      
      public ImportDataTable()
      {
         _rows = new ImportDataRowCollection(base.Rows);
         _columns = new ImportDataColumnCollection(base.Columns);
      }

      public new ImportDataColumnCollection Columns
      {
         get {return (_columns);}
      } 

      public new ImportDataRowCollection Rows {
         get {return _rows;}
      }

      public void RemapMetaDataListOfValueSelection()
      {
         if (MetaData != null)
            MetaData.RemapListOfValueSelection();
         foreach (ImportDataColumn col in Columns)
            if (col.MetaData != null)
               col.MetaData.RemapListOfValueSelection();
      }

      public new ImportDataTable Copy()
      {
         var retValue = Clone();
         retValue.BeginLoadData();
         foreach (ImportDataRow row in Rows)
            retValue.ImportRow(row);
         retValue.EndLoadData();
         if (MetaData != null) retValue.MetaData = MetaData.Copy();
         foreach (ImportDataColumn col in Columns)
         {
            if (col.MetaData == null) continue;
            retValue.Columns.ItemByName(col.ColumnName).MetaData = col.MetaData.Copy();
         }
         return retValue;
      }

      public new ImportDataTable Clone()
      {
         var retValue = (ImportDataTable) base.Clone();
         retValue.File = File;
         retValue.Sheet = Sheet;
         foreach (ImportDataColumn col in Columns)
         {
            var newCol = retValue.Columns.ItemByName(col.ColumnName);
            newCol.DisplayName = col.DisplayName;
            newCol.Description = col.Description;
            newCol.Source = col.Source;
            newCol.SkipNullValueRows = col.SkipNullValueRows;
            newCol.ColumnNameOfRelatedColumn = col.ColumnNameOfRelatedColumn;
            if (col.MetaData != null) newCol.MetaData = col.MetaData.Clone();
            if (col.Dimensions == null) continue;
            newCol.Dimensions = DimensionHelper.Clone(col.Dimensions);
         }
         if (MetaData != null) retValue.MetaData = MetaData.Clone();

         //this second run overt the columns is necessary to avoid side effects on unit setting by setting the dimensions.
         foreach (ImportDataColumn col in Columns)
         {
            if (col.ActiveDimension == null) continue;
            var newCol = retValue.Columns.ItemByName(col.ColumnName);
            newCol.ActiveDimension = DimensionHelper.FindDimension(newCol.Dimensions, col.ActiveDimension.Name);
            newCol.ActiveUnit = newCol.ActiveDimension.FindUnit(col.ActiveUnit.Name);
            newCol.IsUnitExplicitlySet = col.IsUnitExplicitlySet;
         }
         return retValue;
      }

      protected override DataTable CreateInstance()
      {
         return new ImportDataTable();
      }

      protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
         return new ImportDataRow(builder);
      }

      public System.Collections.IEnumerator GetEnumerator()
      {
         return Rows.GetEnumerator();
      }

      /// <summary>
      /// This property is special data table which represents meta data. 
      /// </summary>
      public MetaDataTable MetaData {get; set;}

      public string Source 
      {
         get { return string.Format("{0}.{1}", File, Sheet); } 
      }

      public string File { set; get; }

      public string Sheet { set; get; }

      public bool HasMissingRequiredMetaData
      {
         get
         {
            var retVal = Columns.Cast<ImportDataColumn>().Aggregate(false, (current, col) => current | col.HasMissingRequiredMetaData);

            if (MetaData != null)
               retVal = MetaData.Columns.Cast<MetaDataColumn>().Aggregate(retVal, (current, col) => current | (col.Required && (MetaData.Rows.Count == 0)));

            return retVal;
         }
      }

      public bool HasMissingData
      {
         get
         {
            var retVal = HasMissingRequiredMetaData;
            foreach (ImportDataColumn column in Columns)
            {
               if (!column.Required && string.IsNullOrEmpty(column.Source)) continue;
               retVal |= (column.HasMissingInputParameters || column.HasMissingRequiredMetaData ||
                          !column.IsUnitExplicitlySet);
               if (retVal) break;
            }

            return retVal;
         }
      }

      private bool _disposed;

      protected override void Dispose(bool disposing)
      {
         if (!_disposed)
         {
            try
            {
               if (disposing)
               {
                  if (MetaData != null) MetaData.Dispose();
                  foreach(ImportDataColumn col in _columns)
                     col.Dispose();
               }
               _disposed = true;
            }
            finally
            {
               base.Dispose(disposing);
            }
         }
      }
   }
}
