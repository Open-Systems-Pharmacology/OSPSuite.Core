using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Presentation.Importer.Core.DataFormat;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IImporterTask
   {
      int GetImageIndex(DataFormatParameter parameter, IEnumerable<DataFormatParameter> mappings);
   }

   public class ImporterTask : IImporterTask
   {
      public int GetImageIndex(DataFormatParameter parameter, IEnumerable<DataFormatParameter> mappings)
      {
         switch (parameter.Type)
         {
            case DataFormatParameterType.MetaData:
               return ApplicationIcons.IconIndex(
                  mappings
                     .Any(m => (m.Type == DataFormatParameterType.MetaData) && m.ColumnName == parameter.ColumnName)
                     ? ApplicationIcons.MetaData
                     : ApplicationIcons.MissingMetaData);
            case DataFormatParameterType.Mapping:
               return ApplicationIcons.IconIndex(
                  mappings
                     .Any(m => (m.Type == DataFormatParameterType.Mapping) &&
                               (m as MappingDataFormatParameter)?.MappedColumn.Name.ToString() == parameter.ColumnName)
                     ? ApplicationIcons.UnitInformation
                     : ApplicationIcons.MissingUnitInformation);
            case DataFormatParameterType.GroupBy:
               return ApplicationIcons.IconIndex(ApplicationIcons.GroupBy);
            default:
               throw new Exception($"{parameter.Type} is not currently been handled");
         }
      }
   }
}