using OSPSuite.Assets;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
         var imageIndex = ApplicationIcons.IconIndex(ApplicationIcons.EmptyIcon);
         switch (parameter.Type)
         {
            case DataFormatParameterType.META_DATA:
               imageIndex = ApplicationIcons.IconIndex(
                  mappings
                     .Where(m => (m.Type == DataFormatParameterType.META_DATA) && m.ColumnName == parameter.ColumnName)
                     .Count() > 0 ?
                        ApplicationIcons.MetaData : 
                        ApplicationIcons.MissingMetaData);
               break;
            case DataFormatParameterType.MAPPING:
               imageIndex = ApplicationIcons.IconIndex(
                  mappings
                     .Where(m => (m.Type == DataFormatParameterType.MAPPING) && (m as MappingDataFormatParameter).MappedColumn.Name.ToString() == parameter.ColumnName)
                     .Count() > 0 ?
                        ApplicationIcons.UnitInformation :
                        ApplicationIcons.MissingUnitInformation);
               break;
            case DataFormatParameterType.GROUP_BY:
               imageIndex = ApplicationIcons.IconIndex(ApplicationIcons.GroupBy);
               break;
         }
         return imageIndex;
      }
   }
}
