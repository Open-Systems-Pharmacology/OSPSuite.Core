using OSPSuite.Core.Import;

namespace OSPSuite.R.Services
{
   public interface IImportConfigurationTask
   {
      MappingDataFormatParameter GetTime(ImporterConfiguration configuration);
      void SetTime(ImporterConfiguration configuration, MappingDataFormatParameter time);
      MappingDataFormatParameter GetMeasurement(ImporterConfiguration configuration);
      void SetMeasurement(ImporterConfiguration configuration, MappingDataFormatParameter measurement);
      MappingDataFormatParameter GetError(ImporterConfiguration configuration);
      void SetError(ImporterConfiguration configuration, MappingDataFormatParameter error);
      DataFormatParameter[] GetAllGroupingColumns(ImporterConfiguration configuration);
      void AddGroupingColumn(ImporterConfiguration configuration, DataFormatParameter parameter);
      void RemoveGroupingColumn(ImporterConfiguration configuration, DataFormatParameter parameter);
      string[] GetAllLoadedSheets(ImporterConfiguration configuration);
      void AddLoadedSheet(ImporterConfiguration configuration, string sheet);
      void RemoveLoadedSheet(ImporterConfiguration configuration, string sheet);
   }

   public class ImportConfigurationTask : IImportConfigurationTask
   {
      public void AddGroupingColumn(ImporterConfiguration configuration, DataFormatParameter parameter)
      {
         throw new System.NotImplementedException();
      }

      public void AddLoadedSheet(ImporterConfiguration configuration, string sheet)
      {
         throw new System.NotImplementedException();
      }

      public DataFormatParameter[] GetAllGroupingColumns(ImporterConfiguration configuration)
      {
         throw new System.NotImplementedException();
      }

      public string[] GetAllLoadedSheets(ImporterConfiguration configuration)
      {
         throw new System.NotImplementedException();
      }

      public MappingDataFormatParameter GetError(ImporterConfiguration configuration)
      {
         throw new System.NotImplementedException();
      }

      public MappingDataFormatParameter GetMeasurement(ImporterConfiguration configuration)
      {
         throw new System.NotImplementedException();
      }

      public MappingDataFormatParameter GetTime(ImporterConfiguration configuration)
      {
         throw new System.NotImplementedException();
      }

      public void RemoveGroupingColumn(ImporterConfiguration configuration, DataFormatParameter parameter)
      {
         throw new System.NotImplementedException();
      }

      public void RemoveLoadedSheet(ImporterConfiguration configuration, string sheet)
      {
         throw new System.NotImplementedException();
      }

      public void SetError(ImporterConfiguration configuration, MappingDataFormatParameter error)
      {
         throw new System.NotImplementedException();
      }

      public void SetMeasurement(ImporterConfiguration configuration, MappingDataFormatParameter measurement)
      {
         throw new System.NotImplementedException();
      }

      public void SetTime(ImporterConfiguration configuration, MappingDataFormatParameter time)
      {
         throw new System.NotImplementedException();
      }
   }
}
