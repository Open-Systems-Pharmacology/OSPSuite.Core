namespace OSPSuite.Core.Domain
{
   public static class ModelCoreSimulationExtensions
   {
      /// <summary>
      ///    Returns the total drug mass defined in the simulation.
      /// </summary>
      public static double? TotalDrugMassFor(this IModelCoreSimulation modelCoreSimulation, string compoundName)
      {
         //total drug mass is a parameter defined under the compound molecule global property
         var totalDrugMassParameter = modelCoreSimulation.Model.Root.EntityAt<IParameter>(compoundName, Constants.Parameters.TOTAL_DRUG_MASS);
         return totalDrugMassParameter?.Value;
      }

      /// <summary>
      ///    returns the total drug mass per body weight in [umol/kg BW]
      /// </summary>
      public static double? TotalDrugMassPerBodyWeightFor(this IModelCoreSimulation modelCoreSimulation, string compoundName)
      {
         var totalDrugMass = TotalDrugMassFor(modelCoreSimulation, compoundName);
         if (totalDrugMass == null)
            return null;

         var bodyWeightParameter = BodyWeight(modelCoreSimulation);

         if (bodyWeightParameter?.Value > 0)
            return totalDrugMass.Value / bodyWeightParameter.Value;

         return null;
      }

      /// <summary>
      ///    Returns the Body weight <see cref="IParameter" /> if available in the simulation otherwise null.
      /// </summary>
      public static IParameter BodyWeight(this IModelCoreSimulation modelCoreSimulation) => modelCoreSimulation.Model.Root.EntityAt<IParameter>(Constants.ORGANISM, Constants.Parameters.WEIGHT);
   }
}