using System;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.R.Services
{
   public interface IDataRepositoryTask
   {
      DataRepository LoadDataRepository(string fileName);

      void SaveDataRepository(DataRepository dataRepository, string fileName);

      DataColumn GetErrorColumn(DataColumn column);

      DataColumn AddErrorColumn(DataColumn column, string name, string errorType);

      void AddMetaData(DataRepository dataRepository, string key, string value);
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
         return errorColumn;
      }

      public void AddMetaData(DataRepository dataRepository, string key, string value)
      {
         dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = key, Value = value});
      }
   }
}