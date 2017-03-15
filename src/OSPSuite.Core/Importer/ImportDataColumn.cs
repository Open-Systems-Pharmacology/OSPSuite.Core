using System;
using System.Collections.Generic;
using System.Data;

namespace OSPSuite.Core.Importer
{
   public class ImportDataColumn : DataColumn
   {

      /// <summary>
      /// This property must be overloaded to give a <see cref="ImportDataTable"></see> instead of a <see cref="DataTable"></see>.
      /// </summary>
      public new ImportDataTable Table
      {
         get { return (ImportDataTable)base.Table; }
      }

      /// <summary>
      /// This property shows the display name of the column.
      /// </summary>
       /// <remarks>It is equal to <see cref="DataColumn.Caption"/>.</remarks>
      public string DisplayName
      {
         get { return Caption; }
         set { Caption = value; }
      }

      public string Description { get; set; }

      /// <summary>
      /// This properties is used to determine how to treat null values.
      /// </summary>      
      /// <remarks><para>If the property is set to <c>true</c> all rows should be skipped where the value for this column is empty.</para>
      /// <para>Remember that this is just an information for the implementer of the import.</para></remarks>
      public bool SkipNullValueRows { get; set; }


      /// <summary>
      /// This property informs about whether the column value is optional or required.
      /// </summary>
      /// <remarks><para>The property is just the negation of <see cref="DataColumn.AllowDBNull"/>.</para></remarks>
      public bool Required
      {
         get { return !AllowDBNull; }
         set { AllowDBNull = !value; }
      }

      /// <summary>
      /// This property is special data table which represents meta data. 
      /// </summary>
      public MetaDataTable MetaData { get; set; }

      public string Source { get; set; }

      private IList<Dimension> _dimensions;

      public IList<Dimension> Dimensions
      {
         get { return _dimensions; }
         set
         {
            if (!DimensionHelper.Check(value)) return;
            _dimensions = value;
            ActiveDimension = DimensionHelper.GetDefaultDimension(value);
         }
      }

      private Dimension _activeDimension;

      public Dimension ActiveDimension
      {
         get { return _activeDimension; }
         set
         {
            if (_dimensions == null) return;
            if (!_dimensions.Contains(value)) return;
            _activeDimension = value;
            ActiveUnit = value.GetDefaultUnit();
            IsUnitExplicitlySet = false;

            if (Table?.Columns == null) return;

            //Set dimension on related column accordingly
            if (hasRelatedColumn())
            {
               if (!Table.Columns.ContainsName(ColumnNameOfRelatedColumn))
                  throw new ColumnNotFoundException(ColumnNameOfRelatedColumn);

               var relatedColumn = Table.Columns.ItemByName(ColumnNameOfRelatedColumn);
               if (relatedColumnDimensionDoesNotMatch(relatedColumn))
               {
                  try
                  {
                     setDimensionOfRelatedColumn(relatedColumn);
                  }
                  catch (DimensionNotFound){}
               }
            }
            //set dimension on those current column is related to
            foreach (ImportDataColumn col in Table.Columns)
            {
               if (col.ColumnNameOfRelatedColumn != ColumnName) continue;
               if (col.ActiveDimension.Name == _activeDimension.Name) continue;

               try
               {
                  var relatedDim = DimensionHelper.FindDimension(col.CurrentlySupportedDimensions, _activeDimension.Name);
                  if (relatedDim != null)
                  {
                     col.ActiveDimension = relatedDim;
                     col.IsUnitExplicitlySet = false;
                     DimensionHelper.TakeOverInputParameters(_activeDimension, relatedDim);
                  }
               }
               catch (DimensionNotFound) {}
            }
         }
      }

      private bool hasRelatedColumn()
      {
         return !string.IsNullOrEmpty(ColumnNameOfRelatedColumn);
      }

      private bool relatedColumnDimensionDoesNotMatch(ImportDataColumn relatedColumn)
      {
         return relatedColumn.ActiveDimension.Name != _activeDimension.Name;
      }

      private void setDimensionOfRelatedColumn(ImportDataColumn relatedColumn)
      {
         var newRelatedColumnDimension = bestDimensionSupportedByRelatedColumn(relatedColumn);

         if (newRelatedColumnDimension == null) return;

         if (!shouldChangeRelatedColumnDimensionTo(newRelatedColumnDimension)) return;

         relatedColumn.ActiveDimension = newRelatedColumnDimension;
         relatedColumn.IsUnitExplicitlySet = false;
         DimensionHelper.TakeOverInputParameters(_activeDimension, newRelatedColumnDimension);
      }

      private Dimension bestDimensionSupportedByRelatedColumn(ImportDataColumn relatedColumn)
      {
         return DimensionHelper.FindDimension(relatedColumn.CurrentlySupportedDimensions, _activeDimension.Name);
      }

      private static bool shouldChangeRelatedColumnDimensionTo(Dimension newRelatedColumnDimension)
      {
         return !newRelatedColumnDimension.IsDimensionless();
      }

      /// <summary>
      /// A related column must use the same dimension if possible.
      /// </summary>
      public string ColumnNameOfRelatedColumn { get; set; }

      private Unit _activeUnit;

      public Unit ActiveUnit
      {
         get { return _activeUnit; }
         set
         {
            if (!_activeDimension.Units.Contains(value)) return;
            _activeUnit = value;
         }
      }

      /// <summary>
      /// This method checks whether a given unit is supported by that column.
      /// </summary>
      /// <param name="unit">Name of the unit to check against.</param>
      public bool SupportsUnit(string unit)
      {
         if (unit == null || (Dimensions == null && unit == string.Empty)) return false;
         if (Dimensions == null) return false;
         if (Dimensions.Count == 0) return false;
         foreach (var dim in Dimensions)
            foreach (var u in dim.Units)
               if (u.IsEqual(unit))
                  return true;
         return false;
      }

      /// <summary>
      /// This property retrieves a list of currently supported dimensions.
      /// </summary>
      /// <remarks>On dimensions there can be conditions defined for metadata values.</remarks>
      /// <returns>List of filtered dimensions.</returns>
      public IList<Dimension> CurrentlySupportedDimensions 
      {
         get
         {
            if (MetaData == null || MetaData.Rows.Count == 0) return Dimensions;
            var retVal = new List<Dimension>();
            foreach (var dim in Dimensions)
            {
               //Check metadata conditions
               if (dim.MetaDataConditions != null)
                  if (!MetaData.Rows.ItemByIndex(0).CheckConditions(dim.MetaDataConditions))
                     continue;
               retVal.Add(dim);
            } 
            return retVal;
         }
      }

      /// <summary>
      /// This method sets the active dimension and unit to a valid one depending on meta data settings.
      /// </summary>
      public void SetColumnUnitDependingOnMetaData()
      {
         if (MetaData == null || MetaData.Rows.Count == 0) return;
         if (Dimensions == null) return;
         if (CurrentlySupportedDimensions.Count == 0) return;
         if (CurrentlySupportedDimensions.Contains(ActiveDimension)) return;
         if (!string.IsNullOrEmpty(ColumnNameOfRelatedColumn) && Table != null)
         {
            var relatedColumn = Table.Columns.ItemByName(ColumnNameOfRelatedColumn);
            if (relatedColumn.IsUnitExplicitlySet)
            {
               try
               {
                  var relatedDim = DimensionHelper.FindDimension(CurrentlySupportedDimensions,
                                                                 relatedColumn.ActiveDimension.Name);
                  if (relatedDim != null)
                  {
                     ActiveDimension = relatedDim;
                     DimensionHelper.TakeOverInputParameters(relatedDim, _activeDimension);
                     var unitName = relatedColumn.ActiveUnit.Name;
                     ActiveUnit = relatedDim.HasUnit(unitName) ? relatedDim.FindUnit(unitName) : ActiveDimension.GetDefaultUnit();

                     IsUnitExplicitlySet = false;
                     return;
                  }
               } catch (DimensionNotFound) {}
            }
         }

         ActiveDimension = CurrentlySupportedDimensions.Contains(
                              DimensionHelper.GetDefaultDimension(Dimensions))
                              ? DimensionHelper.GetDefaultDimension(Dimensions)
                              : CurrentlySupportedDimensions[0];
         ActiveUnit = ActiveDimension.GetDefaultUnit();
         IsUnitExplicitlySet = false;
      }

      /// <summary>
      /// Property indicating if the column has missing input parameters.
      /// </summary>
      public bool HasMissingInputParameters
      {
         get
         {
            return ActiveDimension != null && ActiveDimension.AreInputParametersMissing();
         }
      }

      private bool _isUnitExplicitlySet;

      /// <summary>
      /// Property indicating whether the unit of the column has been set explicitly or not.
      /// </summary>
      public bool IsUnitExplicitlySet
      {
         get { return _isUnitExplicitlySet; }
         set 
         { 
            _isUnitExplicitlySet = value;

            if (!_isUnitExplicitlySet) return;
            //maybe we can set the meta data by condition constraint.
            if (MetaData == null) return; 
            if (_activeDimension.MetaDataConditions  == null) return; 
            if (_activeDimension.MetaDataConditions.Count <= 0) return;

            MetaDataRow row = MetaData.Rows.Count == 0
                                 ?
                                    MetaData.NewRow() as MetaDataRow
                                 :
                                    MetaData.Rows.ItemByIndex(0);
            if (row == null) return;

            foreach(var condition in _activeDimension.MetaDataConditions)
            {
               if (!MetaData.Columns.ContainsName(condition.Key)) continue;
               var column = MetaData.Columns.ItemByName(condition.Key);
               if (row[column].ToString() != condition.Value)
                  row[column] = condition.Value;
            }
            if (MetaData.Rows.Count == 0)
            {
               //check that all required columns are set
               foreach (MetaDataColumn col in MetaData.Columns)
                  if (col.Required && string.IsNullOrEmpty(row[col].ToString()))
                     return;
               MetaData.Rows.Add(row);
            }
            foreach (MetaDataColumn col in MetaData.Columns)
               if (col.Required && string.IsNullOrEmpty(row[col].ToString()))
               {
                  row.RejectChanges();
                  return;
               }

            MetaData.AcceptChanges();
         }
      }

      /// <summary>
      /// Property indicating if the column has required input parameters.
      /// </summary>
      public bool HasRequiredInputParameters
      {
         get
         {
            return Dimensions != null && ActiveDimension.AreInputParametersRequired();
         }
      }

      /// <summary>
      /// This readonly property informs whether there are required metadata information which are missing.
      /// </summary>
      public bool HasMissingRequiredMetaData
      {
         get
         {           
            var retVal = false;
            if (MetaData != null)
               foreach (MetaDataColumn col in MetaData.Columns)
                  retVal |= (col.Required && (MetaData.Rows.Count == 0));

            return retVal;
         }
      }

      public ImportDataColumn Clone()
      {
         var retValue = new ImportDataColumn
                           {
                              ColumnName = ColumnName,
                              Required = Required,
                              Description = Description,
                              DisplayName = DisplayName,
                              SkipNullValueRows = SkipNullValueRows,
                              Source = Source
                           };
         if (Dimensions != null)
         {
            retValue.Dimensions = DimensionHelper.Clone(Dimensions);
            if (ActiveDimension != null)
            {
               retValue.ActiveDimension = DimensionHelper.FindDimension(retValue.Dimensions, ActiveDimension.Name);
               retValue.ActiveUnit = retValue.ActiveDimension.FindUnit(ActiveUnit.Name);
               retValue.IsUnitExplicitlySet = IsUnitExplicitlySet;
            }
         }
         if (MetaData != null)
            retValue.MetaData = MetaData.Clone();

         retValue.ColumnNameOfRelatedColumn = ColumnNameOfRelatedColumn;
         return retValue;
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