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
      public string _configurationId;

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
      ///    Returns all columns defined in the repository
      /// </summary>
      public virtual IEnumerable<DataColumn> Columns => this;

      /// <summary>
      ///    Returns all columns defined in the repository as array (for R)
      /// </summary>
      public virtual DataColumn[] ColumnsAsArray => _allColumns.ToArray();

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

      public override string ToString() => Name;

      private bool hasExtendedPropertyFor(string propertyName) => ExtendedProperties.Contains(propertyName);

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

      public virtual void RemoveValuesAt(int index) => _allColumns.Each(c => c.RemoveValueAt(index));

      public IEnumerable<DataColumn> AllButBaseGrid() => _allColumns.Where(x => !x.IsBaseGrid());

      /// <summary>
      ///    Returns all columns except the base grid in the repository as array (for R)
      /// </summary>
      public DataColumn[] AllButBaseGridAsArray => AllButBaseGrid().ToArray();

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

      public string ConfigurationId
      {
         get => _configurationId;
         set => SetProperty(ref _configurationId, value);
      }
   }
}