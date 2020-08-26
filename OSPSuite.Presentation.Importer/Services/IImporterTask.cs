using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core.DataFormat;

namespace OSPSuite.Presentation.Importer.Services
{
   public interface IImporterTask
   {
      int GetImageIndex(DataFormatParameter parameter, IEnumerable<DataFormatParameter> mappings = null);

      string CheckWhetherAllDataColumnsAreMapped(IReadOnlyList<ColumnInfo> dataColumns, IEnumerable<DataFormatParameter> mappings);
   }

   public class ImporterTask : IImporterTask
   {
      public int GetImageIndex(DataFormatParameter parameter, IEnumerable<DataFormatParameter> mappings = null)
      {
         switch (parameter)
         {
            case MetaDataFormatParameter mp:
               return ApplicationIcons.IconIndex(ApplicationIcons.MetaData);
            case MappingDataFormatParameter mp:
               if (mappings == null)
                  return ApplicationIcons.IconIndex(ApplicationIcons.UnitInformation);
               return ApplicationIcons.IconIndex(
                  mappings
                     .Any(m => (m is MappingDataFormatParameter) &&
                               (m as MappingDataFormatParameter)?.MappedColumn.Name == mp.MappedColumn.Name)
                     ? ApplicationIcons.UnitInformation
                     : ApplicationIcons.MissingUnitInformation);
            case GroupByDataFormatParameter gp:
               return ApplicationIcons.IconIndex(ApplicationIcons.GroupBy);
            default:
               throw new Exception($"{parameter.GetType()} is not currently been handled");
         }
      }

      public string CheckWhetherAllDataColumnsAreMapped(IReadOnlyList<ColumnInfo> dataColumns, IEnumerable<DataFormatParameter> mappings)
      {
         var subset = mappings.OfType<MappingDataFormatParameter>().ToList();
         foreach (var col in dataColumns)
         {
            if (!col.IsMandatory) continue;
            if
            (
               subset
                  .Where
                  (
                     cm =>
                     cm.MappedColumn.Name.ToString() == col.Name
                  ).FirstOrDefault() == null
            )
               return col.Name;
         }
         return null;
      }
   }
}