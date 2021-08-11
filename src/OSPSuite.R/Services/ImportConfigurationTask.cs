using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using System.Collections.Generic;
using System.Linq;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

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
      private readonly IReadOnlyList<ColumnInfo> _columnInfos;
      public ImportConfigurationTask(IDataImporter dataImporter)
      {
         _columnInfos = ((DataImporter)dataImporter).DefaultPKSimImportConfiguration();
      }

      public void AddGroupingColumn(ImporterConfiguration configuration, DataFormatParameter parameter)
      {
         configuration.AddParameter(parameter);
      }

      public void AddLoadedSheet(ImporterConfiguration configuration, string sheet)
      {
         configuration.AddToLoadedSheets(sheet);
      }

      public DataFormatParameter[] GetAllGroupingColumns(ImporterConfiguration configuration)
      {
         return configuration.Parameters.Where(p => (p is MetaDataFormatParameter) || (p is GroupByDataFormatParameter)).ToArray();
      }

      public string[] GetAllLoadedSheets(ImporterConfiguration configuration)
      {
         return configuration.LoadedSheets.ToArray();
      }

      public MappingDataFormatParameter GetError(ImporterConfiguration configuration)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p => _columnInfos.First(ci => ci.DisplayName == p.MappedColumn.Name).IsAuxiliary());
      }

      public MappingDataFormatParameter GetMeasurement(ImporterConfiguration configuration)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p =>
         {
            var columnInfo = _columnInfos.First(ci => ci.DisplayName == p.MappedColumn.Name);
            return !(columnInfo.IsAuxiliary() || columnInfo.IsBase());
         });
      }

      public MappingDataFormatParameter GetTime(ImporterConfiguration configuration)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p => _columnInfos.First(ci => ci.DisplayName == p.MappedColumn.Name).IsBase());
      }

      public void RemoveGroupingColumn(ImporterConfiguration configuration, DataFormatParameter parameter)
      {
         configuration.Parameters.Remove(parameter);
      }

      public void RemoveLoadedSheet(ImporterConfiguration configuration, string sheet)
      {
         configuration.RemoveFromLoadedSheets(sheet);
      }

      public void SetError(ImporterConfiguration configuration, MappingDataFormatParameter error)
      {
         var errorParameter = GetError(configuration);
         if (errorParameter != null)
            configuration.Parameters.Remove(errorParameter);

         if (error == null)
            return;

         error.MappedColumn.Name = _columnInfos.First(ci => ci.IsAuxiliary()).DisplayName;
         configuration.AddParameter(error);
      }

      public void SetMeasurement(ImporterConfiguration configuration, MappingDataFormatParameter measurement)
      {
         var measurementParameter = GetMeasurement(configuration);
         if (measurementParameter != null)
            configuration.Parameters.Remove(measurementParameter);

         if (measurement == null)
            return;

         measurement.MappedColumn.Name = _columnInfos.First(ci => !(ci.IsAuxiliary() || ci.IsBase())).DisplayName;
         configuration.AddParameter(measurement);
      }

      public void SetTime(ImporterConfiguration configuration, MappingDataFormatParameter time)
      {
         var timeParameter = GetError(configuration);
         if (timeParameter != null)
            configuration.Parameters.Remove(timeParameter);

         if (time == null)
            return;

         time.MappedColumn.Name = _columnInfos.First(ci => ci.IsBase()).DisplayName;
         configuration.AddParameter(time);
      }
   }
}
