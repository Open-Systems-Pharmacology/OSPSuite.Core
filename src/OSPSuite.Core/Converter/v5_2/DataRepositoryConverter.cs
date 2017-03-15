using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Converter.v5_2
{
   public interface IDataRepositoryConverter
   {
      void Convert(DataRepository dataRepository);
   }

   internal class DataRepositoryConverter : IDataRepositoryConverter
   {
      private readonly IUsingDimensionConverter _usingDimensionConverter;
      private readonly IDimensionMapper _dimensionMapper;

      public DataRepositoryConverter(IUsingDimensionConverter usingDimensionConverter, IDimensionMapper dimensionMapper)
      {
         _usingDimensionConverter = usingDimensionConverter;
         _dimensionMapper = dimensionMapper;
      }

      public void Convert(DataRepository dataRepository)
      {

         var conversionFactor = _dimensionMapper.ConversionFactor("MolecularWeight");

         if (dataRepository.ExtendedProperties.Contains(Constants.MOL_WEIGHT_EXTENDED_PROPERTY))
         {
            var molWeight = dataRepository.ExtendedProperties[Constants.MOL_WEIGHT_EXTENDED_PROPERTY].DowncastTo<IExtendedProperty<double>>();
            molWeight.Value *= conversionFactor;
         }

         foreach (var column in dataRepository.Columns)
         {
            convert(column);
            var dataInfo = column.DataInfo;
            if (dataInfo == null || dataInfo.MolWeight == null) continue;
            
            dataInfo.MolWeight *= conversionFactor;
         }
      }

      private void convert(DataColumn column)
      {
         var dimension = column.Dimension;
         if (dimension == null) return;

         var conversionFactor = (float) _dimensionMapper.ConversionFactor(column);
         _usingDimensionConverter.Convert(column);
         if (conversionFactor == 1) return;

         var values = column.InternalValues;
         for (int i = 0; i < values.Count; i++)
         {
            values[i] *= conversionFactor;
         }
         column.Values = values;
      }
   }
}