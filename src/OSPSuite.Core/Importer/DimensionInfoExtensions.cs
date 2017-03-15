using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Importer
{
   public static class DimensionInfoExtensions
   {
      public static Dimension ConvertToDimensions(this DimensionInfo dimensionInfo)
      {
         var dimension = new Dimension
         {
            Name = dimensionInfo.Dimension.Name,
            DisplayName = dimensionInfo.Dimension.DisplayName,
            IsDefault = dimensionInfo.IsMainDimension,
            InputParameters = new List<InputParameter>()
         };

         if (dimensionInfo.Dimension.Units != null)
         {
            dimension.Units = new List<Unit>();
            foreach (var unit in dimensionInfo.Dimension.Units)
            {
               dimension.Units.Add(new Unit
               {
                  Name = unit.Name,
                  DisplayName = unit.Name,
                  IsDefault = (unit == dimensionInfo.Dimension.DefaultUnit)
               });
            }
         }

         if (dimensionInfo.ConversionParameters != null && dimensionInfo.ConversionParameters.Count > 0)
         {
            foreach (var inputParameter in dimensionInfo.ConversionParameters.Select(conversionParameter => new InputParameter
            {
               Name = conversionParameter.Name,
               DisplayName = conversionParameter.Description,
               Unit = new Unit
               {
                  Name = conversionParameter.Dimension.DefaultUnit.Name,
                  DisplayName = conversionParameter.Dimension.DefaultUnit.Name
               }
            }))
            {
               dimension.InputParameters.Add(inputParameter);
            }
         }
         return dimension;
      }
   }
}