using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public interface IDataRepositoryTask
   {
      /// <summary>
      ///    Loads the data repository located at <paramref name="fileName" />
      /// </summary>
      /// <param name="fileName">Full path of the pkml file containing the data repository to load</param>
      /// <returns>The data repository</returns>
      DataRepository LoadDataRepository(string fileName);

      /// <summary>
      ///    Saves the data repository to file
      /// </summary>
      /// <param name="dataRepository">Data repository to save</param>
      /// <param name="fileName">Full path where the <paramref name="dataRepository" /> will be saved</param>
      void SaveDataRepository(DataRepository dataRepository, string fileName);

      /// <summary>
      ///    Returns the Error column associated with the <paramref name="column" /> or <c>null</c> if none is defined
      /// </summary>
      DataColumn GetErrorColumn(DataColumn column);

      /// <summary>
      ///    Adds an error column associated to the <paramref name="column" />. The error type is one of ArithmeticStdDev or
      ///    GeometricStdDev as string.
      ///    The column will also be added to the data repository where <paramref name="column" /> is defined
      /// </summary>
      /// <param name="column">Column for which an error column should be created</param>
      /// <param name="name">Name of the error column</param>
      /// <param name="errorType">Error type for error column</param>
      /// <returns></returns>
      DataColumn AddErrorColumn(DataColumn column, string name, string errorType);

      /// <summary>
      ///    Removes the <paramref name="column" /> from the <paramref name="dataRepository" />. if the column is a related
      ///    column, the association will also be removed. If the column is a base grid column, it can only be removed if the
      ///    data repository has no other column
      /// </summary>
      /// <param name="dataRepository">DataRepository containing the column to remove</param>
      /// <param name="column">Column to remove</param>
      void RemoveColumn(DataRepository dataRepository, DataColumn column);

      /// <summary>
      ///    Adds a new meta data by key. If the meta already exists, it will be overwritten
      /// </summary>
      /// <param name="dataRepository">DataRepository for which the new meta data should be added/updated</param>
      /// <param name="key">Key identifying the meta data</param>
      /// <param name="value">Value</param>
      void AddMetaData(DataRepository dataRepository, string key, string value);

      /// <summary>
      ///    Removes the meta data by key if it exists
      /// </summary>
      /// <param name="dataRepository">DataRepository for which the meta data should be removed</param>
      /// <param name="key">Key of the meta data to remove</param>
      void RemoveMetaData(DataRepository dataRepository, string key);
   }

   public class DataRepositoryTask : IDataRepositoryTask
   {
      private readonly IPKMLPersistor _pkmlPersistor;

      public DataRepositoryTask(IPKMLPersistor pkmlPersistor)
      {
         _pkmlPersistor = pkmlPersistor;
      }

      public DataRepository LoadDataRepository(string fileName)
      {
         return _pkmlPersistor.Load<DataRepository>(fileName);
      }

      public void SaveDataRepository(DataRepository dataRepository, string fileName)
      {
         _pkmlPersistor.SaveToPKML(dataRepository, fileName);
      }

      public DataColumn GetErrorColumn(DataColumn column)
      {
         if (column.ContainsRelatedColumn(AuxiliaryType.ArithmeticStdDev))
            return column.GetRelatedColumn(AuxiliaryType.ArithmeticStdDev);

         if (column.ContainsRelatedColumn(AuxiliaryType.GeometricStdDev))
            return column.GetRelatedColumn(AuxiliaryType.GeometricStdDev);

         return null;
      }

      public DataColumn AddErrorColumn(DataColumn column, string name, string errorType)
      {
         if (!Enum.TryParse(errorType, out AuxiliaryType auxiliaryType))
            throw new OSPSuiteException(Error.InvalidAuxiliaryType);

         DataColumn errorColumn;
         switch (auxiliaryType)
         {
            case AuxiliaryType.ArithmeticStdDev:
               errorColumn = new DataColumn(name, column.Dimension, column.BaseGrid) {DisplayUnit = column.DisplayUnit};
               break;
            case AuxiliaryType.GeometricStdDev:
               errorColumn = new DataColumn(name, Constants.Dimension.NO_DIMENSION, column.BaseGrid);
               break;
            default:
               throw new OSPSuiteException(Error.InvalidAuxiliaryType);
         }

         errorColumn.DataInfo.AuxiliaryType = auxiliaryType;
         errorColumn.DataInfo.Origin = ColumnOrigins.ObservationAuxiliary;
         column.AddRelatedColumn(errorColumn);
         //May be null if the method is called for an orphan column... Does not make sense
         column.Repository?.Add(errorColumn);
         return errorColumn;
      }

      public void RemoveColumn(DataRepository dataRepository, DataColumn column)
      {
         var baseGrid = dataRepository.BaseGrid;
         //We remove the base grid and there is another column depending on it...error 
         if (Equals(baseGrid, column) && dataRepository.Count() > 1)
            throw new OSPSuiteException(Error.CannotRemoveBaseGridColumnStillInUse);

         //we remove the column and all its related column
         dataRepository.Remove(column);
         column.RelatedColumns.Each(dataRepository.Remove);

         //now remove the column from related column if it is, for instance, an error column
         var allColumnsReferencingTheColumnToRemove = dataRepository.Columns.Where(x => x.RelatedColumns.Contains(column)).ToList();
         allColumnsReferencingTheColumnToRemove.Each(x => x.RelatedColumnsCache.Remove(column.DataInfo.AuxiliaryType));
      }

      public void AddMetaData(DataRepository dataRepository, string key, string value)
      {
         dataRepository.ExtendedProperties[key] = new ExtendedProperty<string> {Name = key, Value = value};
      }

      public void RemoveMetaData(DataRepository dataRepository, string key)
      {
         dataRepository.ExtendedProperties.Remove(key);
      }
   }
}