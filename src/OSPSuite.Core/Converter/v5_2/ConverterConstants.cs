using System.Collections.Generic;

namespace OSPSuite.Core.Converter.v5_2
{
   public static class ConverterConstants
   {
      internal static IReadOnlyDictionary<string, string> DummyDimensions = new Dictionary<string, string>
         {
            {"Nanolength", "Length"},
            {"MassProteinPerVolume", "Concentration (mass)"},
            {"ConcentrationNorm", "Concentration (mass)"},
            {"AUCNorm", "AUC (mass)"},
            {"Vmax", "Concentration (molar) per time"},
            {"DosePerBodyWeight", "Dose per body weight"},
            {"Age", "Age in years"},
            {"MassAmount", "Mass"},
            {"Rate", "Amount per time"},
            {"DiffusionCoefficient", "Diffusion coefficient"},
            {"Surface", "Area"},
            {"Concentration", "Concentration (mass)"}
         };
   }
}