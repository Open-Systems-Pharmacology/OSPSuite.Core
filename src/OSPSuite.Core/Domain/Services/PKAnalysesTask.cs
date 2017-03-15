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
      PopulationSimulationPKAnalyses CalculateFor(ISimulation simulation, int numberOfIndividuals, SimulationResults runResults);
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

      public virtual PopulationSimulationPKAnalyses CalculateFor(ISimulation simulation, int numberOfIndividuals, SimulationResults runResults)
      {
         return CalculateFor(simulation, numberOfIndividuals, runResults, id => { });
      }

      protected virtual PopulationSimulationPKAnalyses CalculateFor(ISimulation simulation, int numberOfIndividuals, SimulationResults runResults, Action<int> performIndividualScalingAction)
      {
         _lazyLoadTask.Load(simulation);

         var popAnalyses = new PopulationSimulationPKAnalyses();

         foreach (var selectedQuantityForMolecule in simulation.OutputSelections.GroupBy(moleculeNameFrom))
         {
            var moleculeName = selectedQuantityForMolecule.Key;
            var pkCalculationOptions = _pkCalculationOptionsFactory.CreateFor(simulation, moleculeName);
            var allApplicationParameters = _pkCalculationOptionsFactory.AllApplicationParametersOrderedByStartTimeFor(simulation, moleculeName);

            foreach (var selectedQuantity in selectedQuantityForMolecule)
            {
               addPKParametersForOutput(simulation, numberOfIndividuals, runResults, performIndividualScalingAction, selectedQuantity, popAnalyses, moleculeName, pkCalculationOptions, allApplicationParameters);
            }
         }

         return popAnalyses;
      }

      private void addPKParametersForOutput(ISimulation simulation, int numberOfIndividuals, SimulationResults runResults, Action<int> performIndividualScalingAction,
         QuantitySelection selectedQuantity, PopulationSimulationPKAnalyses popAnalyses, string moleculeName,
         PKCalculationOptions pkCalculationOptions, IReadOnlyList<PKCalculationOptionsFactory.ApplicationParameters> allApplicationParameters)
      {
         var availablePKParameters = _pkParameterRepository.All().Where(p => PKParameterCanBeUsed(p, pkCalculationOptions)).ToList();

         //create pk parameter for each quantities
         foreach (var pkParameter in availablePKParameters)
         {
            var quantityPKParameter = new QuantityPKParameter {Name = pkParameter.Name, QuantityPath = selectedQuantity.Path, Dimension = pkParameter.Dimension};
            quantityPKParameter.SetNumberOfIndividuals(numberOfIndividuals);
            popAnalyses.AddPKAnalysis(quantityPKParameter);
         }

         //add the values for each individual
         foreach (var individualResult in runResults.AllIndividualResults)
         {
            performIndividualScalingAction(individualResult.IndividualId);
            _pkCalculationOptionsFactory.UpdateAppliedDose(simulation, moleculeName, pkCalculationOptions, allApplicationParameters);

            var values = individualResult.ValuesFor(selectedQuantity.Path);
            var pkValues = _pkValuesCalculator.CalculatePK(individualResult.Time.Values, values.Values, pkCalculationOptions);

            foreach (var pkParameter in availablePKParameters)
            {
               var quantityPKParameter = popAnalyses.PKParameterFor(selectedQuantity.Path, pkParameter.Name);
               quantityPKParameter.SetValue(individualResult.IndividualId, pkValues.ValueOrDefaultFor(pkParameter.Name));
            }
         }
      }

      private static string moleculeNameFrom(QuantitySelection selectedQuantity)
      {
         return selectedQuantity.Path.ToPathArray().MoleculeName();
      }

      public virtual bool PKParameterCanBeUsed(PKParameter p, PKCalculationOptions pkCalculationOptions)
      {
         return p.Mode.Is(pkCalculationOptions.PKParameterMode);
      }
   }
}