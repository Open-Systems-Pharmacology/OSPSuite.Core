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
//      MappingDataFormatParameter AddTime(ImporterConfiguration configuration);
      MappingDataFormatParameter GetMeasurement(ImporterConfiguration configuration);
//      MappingDataFormatParameter AddMeasurement(ImporterConfiguration configuration);
      MappingDataFormatParameter GetError(ImporterConfiguration configuration);
      //     MappingDataFormatParameter AddError(ImporterConfiguration configuration);

      void SetIsUnitFromColumn(MappingDataFormatParameter parameter, bool isUnitFromColumn);
      DataFormatParameter[] GetAllGroupingColumns(ImporterConfiguration configuration);
      void AddGroupingColumn(ImporterConfiguration configuration, string columnName);
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

      public void AddGroupingColumn(ImporterConfiguration configuration, string columnName)
      {
         DataFormatParameter parameter = new GroupByDataFormatParameter(columnName);
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

      //public MappingDataFormatParameter AddError(ImporterConfiguration configuration)
      //{
      //   //TODO
      //}

      //public MappingDataFormatParameter AddMeasurement(ImporterConfiguration configuration)
      //{
      //   //TODO
      //}

      //public MappingDataFormatParameter AddTime(ImporterConfiguration configuration)
      //{
      //   //TODO
      //}

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
