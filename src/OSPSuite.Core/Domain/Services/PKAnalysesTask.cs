using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IPKAnalysesTask
   {
      /// <summary>
      ///    Calculates the PK-Analyses based on the given <paramref name="simulation" /> and corresponding
      ///    <paramref name="runResults" />.
      ///    Note: If simulation for a specific individual failed, the length of <paramref name="runResults" /> might be less
      ///    than the number of overall individuals.
      /// </summary>
      /// <param name="simulation">Simulation used to perform the population run</param>
      /// <param name="runResults">Results for the simulation run</param>
      PopulationSimulationPKAnalyses CalculateFor(IModelCoreSimulation simulation, SimulationResults runResults);
   }

   public class PKAnalysesTask : IPKAnalysesTask
   {
      private readonly ILazyLoadTask _lazyLoadTask;
      private readonly IPKParameterRepository _pkParameterRepository;
      private readonly IPKCalculationOptionsFactory _pkCalculationOptionsFactory;
      private readonly IPKValuesCalculator _pkValuesCalculator;

      public PKAnalysesTask(ILazyLoadTask lazyLoadTask, IPKValuesCalculator pkValuesCalculator, IPKParameterRepository pkParameterRepository, IPKCalculationOptionsFactory pkCalculationOptionsFactory)
      {
         _lazyLoadTask = lazyLoadTask;
         _pkParameterRepository = pkParameterRepository;
         _pkCalculationOptionsFactory = pkCalculationOptionsFactory;
         _pkValuesCalculator = pkValuesCalculator;
      }

      public virtual PopulationSimulationPKAnalyses CalculateFor(IModelCoreSimulation simulation,  SimulationResults runResults)
      {
         return CalculateFor(simulation,  runResults, id => { }, (results, options, moleculeName) => { });
      }

      protected virtual PopulationSimulationPKAnalyses CalculateFor(IModelCoreSimulation simulation, SimulationResults runResults, Action<int> performIndividualScalingAction,
         Action<IndividualResults, PKCalculationOptions, string> calculateAppSpecificPKAnalysis)
      {
         _lazyLoadTask.Load(simulation as ILazyLoadable);

         var popAnalyses = new PopulationSimulationPKAnalyses();

         foreach (var selectedQuantityForMolecule in simulation.OutputSelections.GroupBy(moleculeNameFrom))
         {
            var moleculeName = selectedQuantityForMolecule.Key;
            var pkCalculationOptions = _pkCalculationOptionsFactory.CreateFor(simulation, moleculeName);
            var allApplicationParameters = simulation.AllApplicationParametersOrderedByStartTimeFor(moleculeName);

            foreach (var selectedQuantity in selectedQuantityForMolecule)
            {
               addPKParametersForOutput(simulation,  runResults, performIndividualScalingAction, selectedQuantity, popAnalyses, moleculeName, pkCalculationOptions, allApplicationParameters, calculateAppSpecificPKAnalysis);
            }
         }

         return popAnalyses;
      }

      private void addPKParametersForOutput(
         IModelCoreSimulation simulation,
         SimulationResults simulationResults,
         Action<int> performIndividualScalingAction,
         QuantitySelection selectedQuantity,
         PopulationSimulationPKAnalyses popAnalyses,
         string moleculeName,
         PKCalculationOptions pkCalculationOptions,
         IReadOnlyList<ApplicationParameters> allApplicationParameters,
         Action<IndividualResults, PKCalculationOptions, string> appSpecificPKAnalysis)
      {
         var allPKParameters = _pkParameterRepository.All().Where(p => PKParameterCanBeUsed(p, pkCalculationOptions)).ToList();
         var allUserDefinedPKParameters = allPKParameters.OfType<UserDefinedPKParameter>().ToList();

         //create pk parameter for each  pk parameters (predefined and dynamic)
         foreach (var pkParameter in allPKParameters)
         {
            var quantityPKParameter = new QuantityPKParameter { Name = pkParameter.Name, QuantityPath = selectedQuantity.Path, Dimension = pkParameter.Dimension };
            popAnalyses.AddPKAnalysis(quantityPKParameter);
         }

         //add the values for each individual
         foreach (var individualResult in simulationResults.AllIndividualResults)
         {
            performIndividualScalingAction(individualResult.IndividualId);
            _pkCalculationOptionsFactory.UpdateTotalDrugMassPerBodyWeight(simulation, moleculeName, pkCalculationOptions, allApplicationParameters);

            var values = individualResult.QuantityValuesFor(selectedQuantity.Path);
            //This can happen is the results do not match the simulation
            if (values == null)
               continue;

            var pkValues = _pkValuesCalculator.CalculatePK(individualResult.Time.Values, values.Values, pkCalculationOptions, allUserDefinedPKParameters);

            foreach (var quantityPKParameter in popAnalyses.AllPKParametersFor(selectedQuantity.Path))
            {
               quantityPKParameter.SetValue(individualResult.IndividualId, pkValues.ValueOrDefaultFor(quantityPKParameter.Name));
            }
            appSpecificPKAnalysis(individualResult, pkCalculationOptions, moleculeName);
         }
      }

      private static string moleculeNameFrom(QuantitySelection selectedQuantity) => selectedQuantity.Path.ToPathArray().MoleculeName();

      public virtual bool PKParameterCanBeUsed(PKParameter p, PKCalculationOptions pkCalculationOptions) => p.Mode.Is(pkCalculationOptions.PKParameterMode);
   }
}