using System.Globalization;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   ///    Formats molecular weight values the way the observed data importer expects them as display value of the
   ///    Molecule metadata category (<c>MetaDataCategory.ListOfValues</c>): the value is converted from the base unit of the
   ///    molecular weight dimension (kg/µmol) to its default unit (g/mol) and formatted with the current culture, which is
   ///    how the importer parses it back before converting it to the base unit again.
   ///    Both applications must use this helper when listing project molecules in the importer so that the importer's
   ///    consistency check between the molecular weight column and the selected molecule works in both.
   /// </summary>
   public static class MolWeightMetaDataFormatter
   {
      /// <summary>
      ///    Returns the value of <paramref name="molWeightParameter" /> in the default unit of its dimension (g/mol) as string,
      ///    or an empty string when the parameter is <c>null</c> or has no value
      /// </summary>
      public static string MolWeightInDefaultUnitFor(IParameter molWeightParameter)
      {
         if (molWeightParameter == null)
            return string.Empty;

         return MolWeightInDefaultUnitFor(molWeightParameter.Value, molWeightParameter.Dimension);
      }

      /// <summary>
      ///    Returns <paramref name="molWeightInBaseUnit" /> converted to the default unit of
      ///    <paramref name="molWeightDimension" /> (g/mol) as string, or an empty string when the value or the dimension is
      ///    undefined
      /// </summary>
      public static string MolWeightInDefaultUnitFor(double? molWeightInBaseUnit, IDimension molWeightDimension)
      {
         if (molWeightInBaseUnit == null || double.IsNaN(molWeightInBaseUnit.Value) || molWeightDimension == null)
            return string.Empty;

         var molWeightInDefaultUnit = molWeightDimension.BaseUnitValueToUnitValue(molWeightDimension.DefaultUnit, molWeightInBaseUnit.Value);
         return molWeightInDefaultUnit.ToString(CultureInfo.CurrentCulture);
      }
   }
}
