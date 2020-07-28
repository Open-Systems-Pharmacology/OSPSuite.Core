using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Presentation.Importer.Core.DataFormat;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IImporterTask
   {
      int GetImageIndex(IDataFormatParameter parameter, IEnumerable<IDataFormatParameter> mappings);
   }

   public class ImporterTask : IImporterTask
   {
      public int GetImageIndex(IDataFormatParameter parameter, IEnumerable<IDataFormatParameter> mappings)
      {
         switch (parameter.Type)
         {
            case ParameterConfiguration.DataFormatParameterType.MetaData:
               return ApplicationIcons.IconIndex(
                  mappings
                     .Any(m => (m.Type == ParameterConfiguration.DataFormatParameterType.MetaData) && m.ColumnName == parameter.ColumnName)
                     ? ApplicationIcons.MetaData
                     : ApplicationIcons.MissingMetaData);
            case ParameterConfiguration.DataFormatParameterType.Mapping:
               return ApplicationIcons.IconIndex(
                  mappings
                     .Any(m => (m.Type == ParameterConfiguration.DataFormatParameterType.Mapping) &&
                               (m as MappingDataFormatParameter)?.MappedColumn.Name.ToString() == parameter.ColumnName)
                     ? ApplicationIcons.UnitInformation
                     : ApplicationIcons.MissingUnitInformation);
            case ParameterConfiguration.DataFormatParameterType.GroupBy:
               return ApplicationIcons.IconIndex(ApplicationIcons.GroupBy);
            default:
               throw new Exception($"{parameter.Type} is not currently been handled");
         }
      }
   }
}