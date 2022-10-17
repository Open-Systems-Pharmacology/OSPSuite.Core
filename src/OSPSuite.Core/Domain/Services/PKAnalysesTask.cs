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
         Action<int, PKCalculationOptions, string> calculateAppSpecificPKAnalysis)
      {
         _lazyLoadTask.Load(simulation as ILazyLoadable);

         var popAnalyses = new PopulationSimulationPKAnalyses();

         foreach (var selectedQuantityForMolecule in simulation.OutputSelections.GroupBy(moleculeNameFrom))
         {
            var moleculeName = selectedQuantityForMolecule.Key;
            var pkCalculationOptions = _pkCalculationOptionsFactory.CreateFor(simulation, moleculeName);
            var allApplicationParameters = simulation.AllApplicationParametersOrderedByStartTimeFor(moleculeName);

            // calculate application specific PK Analysis
            scaledIndividualIteration(simulation, runResults, performIndividualScalingAction, moleculeName, pkCalculationOptions, allApplicationParameters, results =>
            {
               // To prevent re-writing of the individual body weight scaling logic, we are using this application specific action that is called for each individual
               // after scaling has been done
               calculateAppSpecificPKAnalysis(results.IndividualId, pkCalculationOptions, moleculeName);
            });

            // add the values for each output
            foreach (var selectedQuantity in selectedQuantityForMolecule)
            {
               addPKParametersForOutput(simulation,  runResults, performIndividualScalingAction, selectedQuantity, popAnalyses, moleculeName, pkCalculationOptions, allApplicationParameters);
            }
         }

         return popAnalyses;
      }

      private void scaledIndividualIteration(IModelCoreSimulation simulation, SimulationResults simulationResults,
         Action<int> performIndividualScalingAction,
         string moleculeName,
         PKCalculationOptions pkCalculationOptions,
         IReadOnlyList<ApplicationParameters> allApplicationParameters, Action<IndividualResults> actionToPerform)
      {
         foreach (var individualResult in simulationResults.AllIndividualResults)
         {
            performIndividualScalingAction(individualResult.IndividualId);
            _pkCalculationOptionsFactory.UpdateTotalDrugMassPerBodyWeight(simulation, moleculeName, pkCalculationOptions, allApplicationParameters);

            actionToPerform(individualResult);

         }
      }

      private void addPKParametersForOutput(
         IModelCoreSimulation simulation,
         SimulationResults simulationResults,
         Action<int> performIndividualScalingAction,
         QuantitySelection selectedQuantity,
         PopulationSimulationPKAnalyses popAnalyses,
         string moleculeName,
         PKCalculationOptions pkCalculationOptions,
         IReadOnlyList<ApplicationParameters> allApplicationParameters)
      {
         var allPKParameters = _pkParameterRepository.All().Where(p => PKParameterCanBeUsed(p, pkCalculationOptions)).ToList();
         var allUserDefinedPKParameters = allPKParameters.OfType<UserDefinedPKParameter>().ToList();

         //create pk parameter for each  pk parameters (predefined and dynamic)
         foreach (var pkParameter in allPKParameters)
         {
            var quantityPKParameter = new QuantityPKParameter { Name = pkParameter.Name, QuantityPath = selectedQuantity.Path, Dimension = pkParameter.Dimension };
            popAnalyses.AddPKAnalysis(quantityPKParameter);
         }

         scaledIndividualIteration(simulation, simulationResults, performIndividualScalingAction, moleculeName, pkCalculationOptions, allApplicationParameters, individualResult =>
         {
            var values = individualResult.QuantityValuesFor(selectedQuantity.Path);
            //This can happen is the results do not match the simulation
            if (values == null)
               return;

            var pkValues = _pkValuesCalculator.CalculatePK(individualResult.Time.Values, values.Values, pkCalculationOptions, allUserDefinedPKParameters);

            foreach (var quantityPKParameter in popAnalyses.AllPKParametersFor(selectedQuantity.Path))
            {
               quantityPKParameter.SetValue(individualResult.IndividualId, pkValues.ValueOrDefaultFor(quantityPKParameter.Name));
            }
         });

      }

      private static string moleculeNameFrom(QuantitySelection selectedQuantity) => selectedQuantity.Path.ToPathArray().MoleculeName();

      public virtual bool PKParameterCanBeUsed(PKParameter p, PKCalculationOptions pkCalculationOptions) => p.Mode.Is(pkCalculationOptions.PKParameterMode);
   }
}