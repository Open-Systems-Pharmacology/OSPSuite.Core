using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

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
      /// <param name="numberOfIndividuals">Number of individuals in the population run</param>
      /// <param name="runResults">Results for the simulation run</param>
      /// <param name="dynamicPKParameters">
      ///    Optional list of <see cref="DynamicPKParameter" /> that will be calculated during the
      ///    sensitivity
      /// </param>
      PopulationSimulationPKAnalyses CalculateFor(IModelCoreSimulation simulation, int numberOfIndividuals, SimulationResults runResults, IReadOnlyList<DynamicPKParameter> dynamicPKParameters = null);
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

      public virtual PopulationSimulationPKAnalyses CalculateFor(IModelCoreSimulation simulation, int numberOfIndividuals, SimulationResults runResults, IReadOnlyList<DynamicPKParameter> dynamicPKParameters = null)
      {
         return CalculateFor(simulation, numberOfIndividuals, runResults, dynamicPKParameters ?? Array.Empty<DynamicPKParameter>(), id => { });
      }

      protected virtual PopulationSimulationPKAnalyses CalculateFor(IModelCoreSimulation simulation, int numberOfIndividuals, SimulationResults runResults, IReadOnlyList<DynamicPKParameter> dynamicPKParameters, Action<int> performIndividualScalingAction)
      {
         _lazyLoadTask.Load(simulation as ILazyLoadable);

         var popAnalyses = new PopulationSimulationPKAnalyses();

         foreach (var selectedQuantityForMolecule in simulation.OutputSelections.GroupBy(moleculeNameFrom))
         {
            var moleculeName = selectedQuantityForMolecule.Key;
            var pkCalculationOptions = _pkCalculationOptionsFactory.CreateFor(simulation, moleculeName);
            var allApplicationParameters = _pkCalculationOptionsFactory.AllApplicationParametersOrderedByStartTimeFor(simulation, moleculeName);

            foreach (var selectedQuantity in selectedQuantityForMolecule)
            {
               addPKParametersForOutput(simulation, numberOfIndividuals, runResults, performIndividualScalingAction, selectedQuantity, popAnalyses, moleculeName, pkCalculationOptions, dynamicPKParameters, allApplicationParameters);
            }
         }

         return popAnalyses;
      }

      private void addPKParametersForOutput(
         IModelCoreSimulation simulation,
         int numberOfIndividuals,
         SimulationResults simulationResults,
         Action<int> performIndividualScalingAction,
         QuantitySelection selectedQuantity,
         PopulationSimulationPKAnalyses popAnalyses,
         string moleculeName,
         PKCalculationOptions pkCalculationOptions,
         IReadOnlyList<DynamicPKParameter> dynamicPKParameters,
         IReadOnlyList<PKCalculationOptionsFactory.ApplicationParameters> allApplicationParameters)
      {
         var allPKParameters = _pkParameterRepository.All().Where(p => PKParameterCanBeUsed(p, pkCalculationOptions)).Union(dynamicPKParameters).ToList();

         //create pk parameter for each  pk parameters (predefined and dynamic)
         foreach (var pkParameter in allPKParameters)
         {
            var quantityPKParameter = new QuantityPKParameter { Name = pkParameter.Name, QuantityPath = selectedQuantity.Path, Dimension = pkParameter.Dimension };
            quantityPKParameter.SetNumberOfIndividuals(numberOfIndividuals);
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

            var pkValues = _pkValuesCalculator.CalculatePK(individualResult.Time.Values, values.Values, pkCalculationOptions, dynamicPKParameters);

            foreach (var quantityPKParameter in popAnalyses.AllPKParametersFor(selectedQuantity.Path))
            {
               quantityPKParameter.SetValue(individualResult.IndividualId, pkValues.ValueOrDefaultFor(quantityPKParameter.Name));
            }
         }
      }

      private static string moleculeNameFrom(QuantitySelection selectedQuantity) => selectedQuantity.Path.ToPathArray().MoleculeName();

      public virtual bool PKParameterCanBeUsed(PKParameter p, PKCalculationOptions pkCalculationOptions) => p.Mode.Is(pkCalculationOptions.PKParameterMode);
   }
}