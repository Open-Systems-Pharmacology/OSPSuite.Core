using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   public class DataRepository : ObjectBase, IEnumerable<DataColumn>
   {
      protected readonly ICache<string, DataColumn> _allColumns = new Cache<string, DataColumn>(col => col.Id);

      /// <summary>
      ///    All Extended properties
      /// </summary>
      public virtual ExtendedProperties ExtendedProperties { get; }

      public DataRepository() : this(Guid.NewGuid().ToString())
      {
      }

      public DataRepository(string id)
      {
         ExtendedProperties = new ExtendedProperties();
         Id = id;
         Icon = IconNames.OBSERVED_DATA;
      }

      /// <summary>
      ///    Returns all columns defiend in the repository
      /// </summary>
      public virtual IEnumerable<DataColumn> Columns => this;

      /// <summary>
      ///    Adds a column to the repository and adds BaseGrid, if not already available
      /// </summary>
      /// <param name="column">Column to add</param>
      public virtual void Add(DataColumn column)
      {
         var myBaseGrid = BaseGrid;
         if (column.BaseGrid != null && myBaseGrid != null && column.BaseGrid != myBaseGrid)
            throw new InvalidOperationException($"Column {column.Name} does not have the same base grid as other columns in this repository.");

         addColumnIfRequired(column.BaseGrid);
         column.RelatedColumns.Each(addColumnIfRequired);

         addColumnIfRequired(column);
      }

      public virtual BaseGrid BaseGrid
      {
         get { return _allColumns.Select(x => x.BaseGrid).FirstOrDefault(col => col != null); }
      }

      /// <summary>
      ///    Removes a column from the repository
      /// </summary>
      public virtual void Remove(DataColumn column)
      {
         if (!_allColumns.Contains(column.Id))
            return;

         _allColumns.Remove(column.Id);
         column.Repository = null;
      }

      /// <summary>
      ///    Returns a column with the given id
      /// </summary>
      /// <param name="columnId">id of column to return</param>
      /// <exception cref="KeyNotFoundException">is thrown if a column with the given key does not exist</exception>
      public virtual DataColumn GetColumn(string columnId)
      {
         return _allColumns[columnId];
      }

      /// <summary>
      ///    Returns true if the repository contains a column with the given id otherwise false
      /// </summary>
      public virtual bool Contains(string columnId)
      {
         return _allColumns.Contains(columnId);
      }

      /// <summary>
      ///    Returns a column with the given id
      /// </summary>
      /// <param name="columnId">id of column to return</param>
      /// <exception cref="KeyNotFoundException">is thrown if a column with the given key does not exist</exception>
      public virtual DataColumn this[string columnId] => GetColumn(columnId);

      /// <summary>
      ///    Removes all columns defined in the repository
      /// </summary>
      public virtual void Clear()
      {
         _allColumns.Each(column => column.Repository = null);
         _allColumns.Clear();
      }

      public IEnumerator<DataColumn> GetEnumerator()
      {
         return _allColumns.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      private void addColumnIfRequired(DataColumn column)
      {
         if (column == null)
            return;

         if (_allColumns.Contains(column.Id))
            return;

         if (column.IsInRepository() && !Equals(column.Repository, this))
            throw new InvalidOperationException($"Column {column.Name} belongs to another Repository.");

         _allColumns.Add(column);
         column.Repository = this;
      }

      public override string ToString()
      {
         return Name;
      }

      private bool hasExtendedPropertyFor(string propertyName)
      {
         return ExtendedProperties.Contains(propertyName);
      }

      /// <summary>
      ///    Gets the value for the named property from the underlying DataRepository
      /// </summary>
      /// <param name="propertyName">The name of the property to be retrieved</param>
      /// <returns>
      ///    null if no property matching <paramref name="propertyName" /> can be found.
      ///    Otherwise it returns the value of the property
      /// </returns>
      public string ExtendedPropertyValueFor(string propertyName)
      {
         return hasExtendedPropertyFor(propertyName) ? ExtendedProperties[propertyName].ValueAsObject.ConvertedTo<string>() : null;
      }

      /// <summary>
      ///    Inserts the values defined in <paramref name="columnValues" /> at the index found in the base column for the value
      ///    <paramref name="baseValue" />.
      ///    Values will be either replaced (there is already an entry with the same time exactly) or inserted at the right index
      ///    to prevent the required base grid monotonie.
      ///    In the case of an insertion, a <c>float.Nan</c> values will be inserted for columns using the base grid and for
      ///    which a value was not provided in <paramref name="columnValues" />
      /// </summary>
      /// <param name="baseValue">Value (e.g. time value) in base unit for which column values should be updated/inserted</param>
      /// <param name="columnValues">Cache containg the id of the columns to update as key and the corresponding value.</param>
      public void InsertValues(float baseValue, Cache<string, float> columnValues)
      {
         var baseGrid = BaseGrid;
         var index = baseGrid.InsertOrReplace(baseValue, out bool replaced);

         foreach (var column in columnValues.KeyValues.Where(x => Contains(x.Key)).Select(x => GetColumn(x.Key)))
         {
            var value = columnValues[column.Id];
            if (replaced)
               column[index] = value;
            else
               column.InsertValueAt(index, columnValues[column.Id]);
         }

         if (replaced) return;

         //we need to add a dummy values for all other columns not in column values using the basegrid
         foreach (var column in columnsThatWereNotUpdated(columnValues))
         {
            column.InsertValueAt(index, float.NaN);
         }
      }

      /// <summary>
      ///    Swaps out  all values defined in the <see cref="DataRepository" /> at the index for the baseGrid
      ///    <paramref name="oldBaseGridValue" /> and insert them back at the index for <paramref name="newBaseGridValue" />
      /// </summary>
      /// <param name="oldBaseGridValue">BaseGrid value that will be used to retrieve the index of values to swap out </param>
      /// <param name="newBaseGridValue">BaseGrid value that will be used to retrieve the index where the values will be inserted</param>
      public virtual void SwapValues(float oldBaseGridValue, float newBaseGridValue)
      {
         var index = BaseGrid.IndexOf(oldBaseGridValue);
         if (index < 0) return;

         var columnValues = new Cache<string, float>();
         AllButBaseGrid().Each(c => columnValues.Add(c.Id, c[index]));
         RemoveValuesAt(index);
         InsertValues(newBaseGridValue, columnValues);
      }

      public virtual void RemoveValuesAt(int index)
      {
         _allColumns.Each(c => c.RemoveValueAt(index));
      }

      private IEnumerable<DataColumn> columnsThatWereNotUpdated(Cache<string, float> columnValues)
      {
         return AllButBaseGrid()
            .Where(x => !columnValues.Contains(x.Id));
      }

      public IEnumerable<DataColumn> AllButBaseGrid()
      {
         return _allColumns.Where(x => !x.IsBaseGrid());
      }

      public float ConvertBaseValueForColumn(string columnId, float valueInDisplayUnit)
      {
         return Convert.ToSingle(GetColumn(columnId).ConvertToBaseUnit(valueInDisplayUnit));
      }

      public bool HasObservationBelowLLOQ(int rowIndex)
      {
         return ColumnWithValueBelowLLOQ(rowIndex) != null;
      }

      public DataColumn ColumnWithValueBelowLLOQ(int rowIndex)
      {
         return ObservationColumns().FirstOrDefault(observationColumn => observationColumn.ColumnValueIsBelowLLOQ(rowIndex));
      }

      public IEnumerable<DataColumn> ObservationColumns()
      {
         return AllButBaseGrid().Where(column => column.DataInfo.Origin == ColumnOrigins.Observation);
      }
   }
}