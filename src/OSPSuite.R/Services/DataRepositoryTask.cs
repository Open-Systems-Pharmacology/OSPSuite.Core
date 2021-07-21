using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Xml;
using System;

namespace OSPSuite.R.Services
{
   public interface IDataRepositoryTask
   {
      DataRepository LoadDataRepository(string fileName);

      void SaveDataRepository(DataRepository dataRepository, string fileName);

      DataColumn GetErrorColumn(DataColumn column);

      DataColumn AddErrorColumn(DataColumn column, string name = "yError", string errorType = null);
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

      public DataColumn AddErrorColumn(DataColumn column, string name = "yError", string errorType = null)
      {
         var errorColumn = new DataColumn(name, column.Dimension, column.BaseGrid);

         if (string.IsNullOrEmpty(errorType))
            errorType = AuxiliaryType.ArithmeticStdDev.ToString();

         AuxiliaryType auxiliaryType;
         if (!Enum.TryParse(errorType, out auxiliaryType))
            throw new Exception(Error.InvalidAuxiliaryType);
            
         errorColumn.DataInfo.AuxiliaryType = auxiliaryType;
         errorColumn.DisplayUnit = column.DisplayUnit;
         errorColumn.DataInfo.Origin = ColumnOrigins.ObservationAuxiliary;
         column.AddRelatedColumn(errorColumn);
	      return errorColumn;
      }
   }
}