using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Extensions;
using System.Linq;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.R.Services
{
   public interface IImportConfigurationTask
   {
      MappingDataFormatParameter GetTime(ImporterConfiguration configuration, ColumnInfo[] columnInfos);
      void SetTime(ImporterConfiguration configuration, MappingDataFormatParameter time, ColumnInfo[] columnInfos);
      MappingDataFormatParameter GetMeasurement(ImporterConfiguration configuration, ColumnInfo[] columnInfos);
      void SetMeasurement(ImporterConfiguration configuration, MappingDataFormatParameter measurement, ColumnInfo[] columnInfos);
      MappingDataFormatParameter GetError(ImporterConfiguration configuration, ColumnInfo[] columnInfos);
      void SetError(ImporterConfiguration configuration, MappingDataFormatParameter error, ColumnInfo[] columnInfos);
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

      public MappingDataFormatParameter GetError(ImporterConfiguration configuration, ColumnInfo[] columnInfos)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p => columnInfos.First(ci => ci.DisplayName == p.MappedColumn.Name).IsAuxiliary());
      }

      public MappingDataFormatParameter GetMeasurement(ImporterConfiguration configuration, ColumnInfo[] columnInfos)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p =>
         {
            var columnInfo = columnInfos.First(ci => ci.DisplayName == p.MappedColumn.Name);
            return !(columnInfo.IsAuxiliary() || columnInfo.IsBase());
         });
      }

      public MappingDataFormatParameter GetTime(ImporterConfiguration configuration, ColumnInfo[] columnInfos)
      {
         return configuration.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p => columnInfos.First(ci => ci.DisplayName == p.MappedColumn.Name).IsBase());
      }

      public void RemoveGroupingColumn(ImporterConfiguration configuration, DataFormatParameter parameter)
      {
         configuration.Parameters.Remove(parameter);
      }

      public void RemoveLoadedSheet(ImporterConfiguration configuration, string sheet)
      {
         configuration.RemoveFromLoadedSheets(sheet);
      }

      public void SetError(ImporterConfiguration configuration, MappingDataFormatParameter error, ColumnInfo[] columnInfos)
      {
         var errorParameter = GetError(configuration, columnInfos);
         if (errorParameter != null)
            configuration.Parameters.Remove(errorParameter);

         if (error == null)
            return;

         error.MappedColumn.Name = columnInfos.First(ci => ci.IsAuxiliary()).DisplayName;
         configuration.AddParameter(error);
      }

      public void SetMeasurement(ImporterConfiguration configuration, MappingDataFormatParameter measurement, ColumnInfo[] columnInfos)
      {
         var measurementParameter = GetMeasurement(configuration, columnInfos);
         if (measurementParameter != null)
            configuration.Parameters.Remove(measurementParameter);

         if (measurement == null)
            return;

         measurement.MappedColumn.Name = columnInfos.First(ci => !(ci.IsAuxiliary() || ci.IsBase())).DisplayName;
         configuration.AddParameter(measurement);
      }

      public void SetTime(ImporterConfiguration configuration, MappingDataFormatParameter time, ColumnInfo[] columnInfos)
      {
         var timeParameter = GetError(configuration, columnInfos);
         if (timeParameter != null)
            configuration.Parameters.Remove(timeParameter);

         if (time == null)
            return;

         time.MappedColumn.Name = columnInfos.First(ci => ci.IsBase()).DisplayName;
         configuration.AddParameter(time);
      }
   }
}
