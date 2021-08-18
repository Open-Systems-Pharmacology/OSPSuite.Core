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
      MappingDataFormatParameter GetMeasurement(ImporterConfiguration configuration);
      MappingDataFormatParameter GetError(ImporterConfiguration configuration);
      void AddError(ImporterConfiguration configuration, MappingDataFormatParameter errorColumn);
      void RemoveError(ImporterConfiguration configuration);

      void SetIsUnitFromColumn(MappingDataFormatParameter parameter, bool isUnitFromColumn);
      string[] GetAllGroupingColumns(ImporterConfiguration configuration);
      void AddGroupingColumn(ImporterConfiguration configuration, string columnName);
      void RemoveGroupingColumn(ImporterConfiguration configuration, string columnName);
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

      public void AddError(ImporterConfiguration configuration, MappingDataFormatParameter errorColumn)
      {
         var errorParameter = GetError(configuration);
         if (errorParameter != null)
            configuration.Parameters.Remove(errorParameter);

         if (errorColumn == null)
            return;

         errorColumn.MappedColumn.Name = _columnInfos.First(ci => ci.IsAuxiliary()).DisplayName;
         configuration.AddParameter(errorColumn);
      }

      public void AddGroupingColumn(ImporterConfiguration configuration, string columnName)
      {
         DataFormatParameter parameter = new GroupByDataFormatParameter(columnName);
         configuration.AddParameter(parameter);
      }

      public void AddLoadedSheet(ImporterConfiguration configuration, string sheet)
      {
         configuration.AddToLoadedSheets(sheet);
      }

      public string[] GetAllGroupingColumns(ImporterConfiguration configuration)
      {
         return configuration.Parameters.Where(p => (p is MetaDataFormatParameter) || (p is GroupByDataFormatParameter)).Select(p => p.ColumnName).ToArray();
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

      public void RemoveError(ImporterConfiguration configuration)
      {
         var errorParameter = GetError(configuration);
         if (errorParameter != null)
            configuration.Parameters.Remove(errorParameter);
      }

      public void RemoveGroupingColumn(ImporterConfiguration configuration, string columnName)
      {
         configuration.Parameters.Remove(configuration.Parameters.First(p => p.ColumnName == columnName));
      }

      public void RemoveLoadedSheet(ImporterConfiguration configuration, string sheet)
      {
         configuration.RemoveFromLoadedSheets(sheet);
      }

      public void SetIsUnitFromColumn(MappingDataFormatParameter parameter, bool isUnitFromColumn)
      {
         if (isUnitFromColumn)
         {
            parameter.MappedColumn.Unit.ColumnName = parameter.MappedColumn.Unit.SelectedUnit;
            parameter.MappedColumn.Dimension = null;
         } else
         {
            parameter.MappedColumn.Unit.ColumnName = null;
            // 2DO set dimension from unit
         }
      }
   }
}
