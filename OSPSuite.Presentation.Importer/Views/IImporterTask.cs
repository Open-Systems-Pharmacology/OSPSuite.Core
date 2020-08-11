using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Presentation.Importer.Core.DataFormat;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IImporterTask
   {
      int GetImageIndex(DataFormatParameter parameter, IEnumerable<DataFormatParameter> mappings = null);
   }

   public class ImporterTask : IImporterTask
   {
      public int GetImageIndex(DataFormatParameter parameter, IEnumerable<DataFormatParameter> mappings = null)
      {
         switch (parameter)
         {
            case MetaDataFormatParameter mp:
               if (mappings == null)
                  return ApplicationIcons.IconIndex(ApplicationIcons.MetaData);
               return ApplicationIcons.IconIndex(
                  mappings
                     .Any(m => (m is MetaDataFormatParameter) && (m as MetaDataFormatParameter).MetaDataId == mp.MetaDataId)
                     ? ApplicationIcons.MetaData
                     : ApplicationIcons.MissingMetaData);
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
   }
}