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
         var applicationStartTimes = applicationsByStartTime(allApplicationParameters).Select(x => x.Key).ToArray();
         var applicationEndTimes = new List<float>(applicationStartTimes.Skip(1)) {endTime};


         for (var i = 0; i < applicationStartTimes.Length; i++)
         {
            var dosingInterval = new DosingInterval
            {
               StartValue = applicationStartTimes[i],
               EndValue = applicationEndTimes[i]
            };

            //as soon as one dosing interval is not valid, it means that the end time might be on exactly the start time of an application.
            //Everything after this is by construction invalid so we do not consider further interval for the calculations
            if (!dosingInterval.IsValid)
               break;

            options.AddInterval(dosingInterval);
         }

         // single dosing
         if (allApplicationParameters.Count <= 1)
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

         var allApplicationsByStartTime = applicationsByStartTime(allApplicationParametersOrderedByStartTime);
         options.DosingIntervals.Each((x, i) => x.DrugMassPerBodyWeight = drugMassPerBodyWeightFor(allApplicationsByStartTime[i], bodyWeight));
      }

      private IReadOnlyList<IGrouping<float, ApplicationParameters>> applicationsByStartTime(IReadOnlyList<ApplicationParameters> allApplicationParametersOrderedByStartTime) =>
         allApplicationParametersOrderedByStartTime.GroupBy(x => x.StartTime.Value.ToFloat()).ToList();

      private double? drugMassPerBodyWeightFor(IEnumerable<ApplicationParameters> applications, double? bodyWeight)
      {
         var allDrugMassesPerBodyWeight = applications.Select(x => drugMassPerBodyWeightFor(x.DrugMass, bodyWeight)).ToList();
         return allDrugMassesPerBodyWeight.Contains(null) ? null : allDrugMassesPerBodyWeight.Sum();
      }

      private double? drugMassPerBodyWeightFor(IParameter drugMass, double? bodyWeight)
      {
         if (drugMass == null || bodyWeight == null || double.IsNaN(bodyWeight.Value))
            return null;

         return drugMass.Value / bodyWeight.Value;
      }
   }
}