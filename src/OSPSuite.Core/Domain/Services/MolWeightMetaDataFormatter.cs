using System.Globalization;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   //the importer parses the mol weight listed next to a molecule as g/mol in the current culture
   public static class MolWeightMetaDataFormatter
   {
      public static string MolWeightInDefaultUnitFor(IParameter molWeightParameter)
      {
         if (molWeightParameter == null)
            return string.Empty;

         return MolWeightInDefaultUnitFor(molWeightParameter.Value, molWeightParameter.Dimension);
      }

      public static string MolWeightInDefaultUnitFor(double? molWeightInBaseUnit, IDimension molWeightDimension)
      {
         if (molWeightInBaseUnit == null || double.IsNaN(molWeightInBaseUnit.Value) || molWeightDimension == null)
            return string.Empty;

         var molWeightInDefaultUnit = molWeightDimension.BaseUnitValueToUnitValue(molWeightDimension.DefaultUnit, molWeightInBaseUnit.Value);
         return molWeightInDefaultUnit.ToString(CultureInfo.CurrentCulture);
      }
   }
}
