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
         switch (parameter.Configuration.Type)
         {
            case ParameterConfiguration.DataFormatParameterType.MetaData:
               return ApplicationIcons.IconIndex(
                  mappings
                     .Any(m => (m.Configuration.Type == ParameterConfiguration.DataFormatParameterType.MetaData) && m.ColumnName == parameter.ColumnName)
                     ? ApplicationIcons.MetaData
                     : ApplicationIcons.MissingMetaData);
            case ParameterConfiguration.DataFormatParameterType.Mapping:
               return ApplicationIcons.IconIndex(
                  mappings
                     .Any(m => (m.Configuration.Type == ParameterConfiguration.DataFormatParameterType.Mapping) &&
                               (m as MappingDataFormatParameter)?.MappedColumn.Name.ToString() == parameter.ColumnName)
                     ? ApplicationIcons.UnitInformation
                     : ApplicationIcons.MissingUnitInformation);
            case ParameterConfiguration.DataFormatParameterType.GroupBy:
               return ApplicationIcons.IconIndex(ApplicationIcons.GroupBy);
            default:
               throw new Exception($"{parameter.Configuration.Type} is not currently been handled");
         }
      }
   }
}