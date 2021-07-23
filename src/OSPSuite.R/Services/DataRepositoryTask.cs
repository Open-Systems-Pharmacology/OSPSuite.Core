using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility.Exceptions;
using System;

namespace OSPSuite.R.Services
{
   public interface IDataRepositoryTask
   {
      DataRepository LoadDataRepository(string fileName);

      void SaveDataRepository(DataRepository dataRepository, string fileName);

      DataColumn GetErrorColumn(DataColumn column);

      DataColumn AddErrorColumn(DataColumn column, string name, string errorType);
   }

   public class DataRepositoryTask : IDataRepositoryTask
   {
      private readonly IPKMLPersistor _pkmlPersistor;
      private readonly IDimensionTask _dimensionTask;

      public DataRepositoryTask(IPKMLPersistor pkmlPersistor, IDimensionTask dimensionTask)
      {
         _pkmlPersistor = pkmlPersistor;
         _dimensionTask = dimensionTask;
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
         if (string.IsNullOrEmpty(errorType))
            errorType = AuxiliaryType.ArithmeticStdDev.ToString();

         AuxiliaryType auxiliaryType;
         if (!Enum.TryParse(errorType, out auxiliaryType))
            throw new OSPSuiteException(Error.InvalidAuxiliaryType);

         DataColumn errorColumn = null;
         switch (auxiliaryType)
         {
            case AuxiliaryType.ArithmeticStdDev:
               errorColumn = new DataColumn(name, column.Dimension, column.BaseGrid);
               errorColumn.DisplayUnit = column.DisplayUnit;
               break;
            case AuxiliaryType.GeometricStdDev:
               errorColumn = new DataColumn(name, _dimensionTask.DimensionByName("Fraction"), column.BaseGrid);
               break;
            default:
               throw new OSPSuiteException(Error.InvalidAuxiliaryType);
         }
         
         errorColumn.DataInfo.AuxiliaryType = auxiliaryType;
         errorColumn.DataInfo.Origin = ColumnOrigins.ObservationAuxiliary;
         column.AddRelatedColumn(errorColumn);
	      return errorColumn;
      }
   }
}