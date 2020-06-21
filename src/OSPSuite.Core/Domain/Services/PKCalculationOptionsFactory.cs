using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IPKCalculationOptionsFactory
   {
      PKCalculationOptions CreateFor(IModelCoreSimulation simulation, string moleculeName);

      void UpdateTotalDrugMassPerBodyWeight(IModelCoreSimulation simulation, string moleculeName, PKCalculationOptions options,
         IReadOnlyList<ApplicationParameters> allApplicationParametersOrderedByStartTime);
   }

   public class PKCalculationOptionsFactory : IPKCalculationOptionsFactory
   {
      public PKCalculationOptions CreateFor(IModelCoreSimulation simulation, string moleculeName)
      {
         var options = new PKCalculationOptions();
         var endTime = (simulation.EndTime ?? 0).ToFloat();

         var allApplicationParameters = simulation.AllApplicationParametersOrderedByStartTimeFor(moleculeName);

         // all application start times starting before the end of the simulation
         var applicationStartTimes = allApplicationParameters.Select(x => x.StartTime.Value).ToFloatArray();
         var applicationEndTimes = new List<float>(applicationStartTimes.Skip(1)) {endTime};


         for (int i = 0; i < applicationStartTimes.Length; i++)
         {
            var dosingInterval = new DosingInterval
            {
               StartValue = applicationStartTimes[i],
               EndValue = applicationEndTimes[i]
            };

            options.AddInterval(dosingInterval);
         }

         // single dosing
         if (applicationStartTimes.Length <= 1)
         {
            options.InfusionTime = allApplicationParameters.FirstOrDefault()?.InfusionTime?.Value;
         }

         // Once all dosing are defined, update total drug mass
         UpdateTotalDrugMassPerBodyWeight(simulation, moleculeName, options, allApplicationParameters);

         return options;
      }

      public virtual void UpdateTotalDrugMassPerBodyWeight(IModelCoreSimulation simulation, string moleculeName, PKCalculationOptions options,
         IReadOnlyList<ApplicationParameters> allApplicationParametersOrderedByStartTime)
      {
         var bodyWeight = simulation.BodyWeight?.Value;
         var totalDrugMass = simulation.TotalDrugMassFor(moleculeName);

         options.TotalDrugMassPerBodyWeight = drugMassPerBodyWeightFor(totalDrugMass, bodyWeight);

         options.DosingIntervals.Each((x, i) =>
         {
            x.DrugMassPerBodyWeight = drugMassPerBodyWeightFor(allApplicationParametersOrderedByStartTime[i].DrugMass, bodyWeight);
         });
      }

      private double? drugMassPerBodyWeightFor(IParameter drugMass, double? bodyWeight)
      {
         if (drugMass == null || bodyWeight == null || double.IsNaN(bodyWeight.Value))
            return null;

         return drugMass.Value / bodyWeight.Value;
      }
   }
}