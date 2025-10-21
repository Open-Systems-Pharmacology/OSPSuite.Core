using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Services
{
   public interface IDataSourceToDimensionSelectionDTOMapper : IMapper<IDataSource, IReadOnlyList<DimensionSelectionDTO>>
   {
      IReadOnlyList<DimensionSelectionDTO> MapFrom(IDataSource dataSource, IReadOnlyList<string> sheetNames);
   }

   public class DataSourceToDimensionSelectionDTOMapper : IDataSourceToDimensionSelectionDTOMapper
   {
      public IReadOnlyList<DimensionSelectionDTO> MapFrom(IDataSource dataSource)
      {
         return dimensionsSupporting(dataSource).ToList();
      }

      public IReadOnlyList<DimensionSelectionDTO> MapFrom(IDataSource dataSource, IReadOnlyList<string> sheetNames)
      {
         return sheetNames.SelectMany(x => dimensionsSupporting(x, dataSource.DataSets[x])).ToList();
      }

      private IEnumerable<DimensionSelectionDTO> dimensionsSupporting(IDataSource dataSource)
      {
         return dataSource.DataSets.KeyValues.SelectMany(x => dimensionsSupporting(x.Key, x.Value));
      }

      private IEnumerable<DimensionSelectionDTO> dimensionsSupporting(string sheetName, IDataSet dataSource)
      {
         return dataSource.Data.SelectMany(x => dimensionsSupporting(sheetName, x));
      }

      private IEnumerable<DimensionSelectionDTO> dimensionsSupporting(string sheetName, ParsedDataSet parsedDataSet)
      {
         return parsedDataSet.Data.Keys.Select(extendedColumn => 
            dimensionsSupporting(parsedDataSet.Data[extendedColumn].Select(point => point.Unit).Distinct().ToList(), extendedColumn, sheetName, parsedDataSet.Description.Select(x =>x.Value).ToList()));
      }

      private DimensionSelectionDTO dimensionsSupporting(IReadOnlyList<string> units, ExtendedColumn extendedColumn, string sheetName, IReadOnlyList<string> description)
      {
         return new DimensionSelectionDTO(sheetName, description, extendedColumn.Column, 
            extendedColumn.Column.Dimension != null ? 
               new List<IDimension> { extendedColumn.Column.Dimension } : 
               extendedColumn.ColumnInfo.SupportedDimensions.Where(dimension => supportsAllUnits(extendedColumn.Column, dimension, units)).ToList());
      }

      private bool supportsAllUnits(Column extendedColumn, IDimension dimension, IReadOnlyList<string> units)
      {
         // For GeoStd, the only supporting dimension is NO_DIMENSION
         if (extendedColumn.ErrorStdDev == Constants.STD_DEV_GEOMETRIC)
            return dimension == Constants.Dimension.NO_DIMENSION;
         
         return units.All(x => dimension.SupportsUnit(x));
      }
   }
}
