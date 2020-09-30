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
      int GetImageIndex(DataFormatParameter parameter);
      string CheckWhetherAllDataColumnsAreMapped(IReadOnlyList<ColumnInfo> dataColumns, IEnumerable<DataFormatParameter> mappings);
   }

   public class ImporterTask : IImporterTask
   {
      public int GetImageIndex(DataFormatParameter parameter)
      {
         switch (parameter)
         {
            case MetaDataFormatParameter mp:
               return ApplicationIcons.IconIndex(ApplicationIcons.MetaData);
            case MappingDataFormatParameter mp:
               return ApplicationIcons.IconIndex(ApplicationIcons.UnitInformation);
            case GroupByDataFormatParameter gp:
               return ApplicationIcons.IconIndex(ApplicationIcons.GroupBy);
            default:
               throw new Exception($"{parameter.GetType()} is not currently been handled");
         }
      }

      public string CheckWhetherAllDataColumnsAreMapped(IReadOnlyList<ColumnInfo> dataColumns, IEnumerable<DataFormatParameter> mappings)
      {
         var subset = mappings.OfType<MappingDataFormatParameter>().ToList();
         return dataColumns.Where(col => col.IsMandatory && subset.All(cm => cm.MappedColumn.Name != col.Name)).Select(col => col.Name).FirstOrDefault();
      }
   }
}