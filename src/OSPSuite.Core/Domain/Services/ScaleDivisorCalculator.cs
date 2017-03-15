using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public class ScaleDivisor
   {
      /// <summary>
      ///    This is the path of the quantity without the simulation name
      /// </summary>
      public string QuantityPath { get; set; }

      /// <summary>
      ///    This is the scale divisor that was calculated for the quantity with path <see cref="QuantityPath" />
      /// </summary>
      public double Value { get; set; }
   }

   public class ScaleDivisorOptions
   {
      /// <summary>
      ///    Threshold value: The maximum value that the scale divisior can be set to during the computation
      /// </summary>
      public double MinimumScaleDivisor { get; set; }

      /// <summary>
      ///    Specifies if scale divisior value should be rounded to the nearest exponent. Defaut is <c>true</c>
      /// </summary>
      public bool UseRoundedValue { get; set; }

      public ScaleDivisorOptions()
      {
         MinimumScaleDivisor = Constants.DEFAULT_SCALE_DIVISOR_MIN_VALUE;
         UseRoundedValue = true;
      }
   }

   public interface IScaleDivisorCalculator
   {
      /// <summary>
      ///    Calculates the scale divisior for all <see cref="IMoleculeAmount" /> defined in the <paramref name="simulation" />.
      /// </summary>
      /// <remarks>
      ///    We have to keep in mind that the simulation may not contain results for all quantities defined in the model. Hence
      ///    the following strategy is implemented:
      ///    1 - Run the simulation and save up the results for all <see cref="MoleculeAmount" /> defined in the simulation
      ///    2 - Calculate the scale divisiors
      ///    3 - Set the previous results and output selection back to original
      /// </remarks>
      /// <param name="simulation">Simulation for which the scale divisior should be calculated</param>
      /// <param name="scaleDivisorOptions">Options used to compute the scale divisiors</param>
      /// <returns>A collection of all scale divisiors</returns>
      Task<IReadOnlyCollection<ScaleDivisor>> CalculateScaleDivisorsAsync(IModelCoreSimulation simulation, ScaleDivisorOptions scaleDivisorOptions);

      /// <summary>
      ///    Calculates the scale divisior for the <paramref name="allMoleculeAmounts" /> defined in the
      ///    <paramref name="simulation" />.
      /// </summary>
      /// <remarks>
      ///    We have to keep in mind that the simulation may not contain results for all quantities defined in the model. Hence
      ///    the following strategy is implemented:
      ///    1 - Run the simulation and save up the results for all <see cref="MoleculeAmount" /> defined in the simulation
      ///    2 - Calculate the scale divisiors
      ///    3 - Set the previous results and output selection back to original
      /// </remarks>
      /// <param name="simulation">Simulation for which the scale divisior should be calculated</param>
      /// <param name="scaleDivisorOptions">Options used to compute the scale divisiors</param>
      /// <param name="allMoleculeAmounts"></param>
      /// <returns>A collection of all scale divisiors</returns>
      Task<IReadOnlyCollection<ScaleDivisor>> CalculateScaleDivisorsAsync(IModelCoreSimulation simulation, ScaleDivisorOptions scaleDivisorOptions, PathCache<IMoleculeAmount> allMoleculeAmounts);

      /// <summary>
      ///    Resets the scale divisors for the <paramref name="allMoleculeAmounts" /> to 1
      /// </summary>
      IReadOnlyCollection<ScaleDivisor> ResetScaleDivisors(PathCache<IMoleculeAmount> allMoleculeAmounts);
   }

   public class ScaleDivisorCalculator : IScaleDivisorCalculator
   {
      private readonly ISimModelManager _simModelManager;
      private readonly IContainerTask _containerTask;
      private readonly IObjectPathFactory _objectPathFactory;
      private PathCache<IMoleculeAmount> _allMoleculeAmounts;
      private ScaleDivisorOptions _scaleDivisorOptions;
      private string _simulationName;

      public ScaleDivisorCalculator(ISimModelManager simModelManager, IContainerTask containerTask, IObjectPathFactory objectPathFactory)
      {
         _simModelManager = simModelManager;
         _containerTask = containerTask;
         _objectPathFactory = objectPathFactory;
      }

      public async Task<IReadOnlyCollection<ScaleDivisor>> CalculateScaleDivisorsAsync(IModelCoreSimulation simulation, ScaleDivisorOptions scaleDivisorOptions)
      {
         return await CalculateScaleDivisorsAsync(simulation, scaleDivisorOptions, _containerTask.CacheAllChildren<IMoleculeAmount>(simulation.Model.Root));
      }

      public async Task<IReadOnlyCollection<ScaleDivisor>> CalculateScaleDivisorsAsync(IModelCoreSimulation simulation, ScaleDivisorOptions scaleDivisorOptions, PathCache<IMoleculeAmount> allMoleculeAmounts)
      {
         _simulationName = simulation.Name;
         var previousResults = simulation.Results;
         var previousSelections = outputSelectionsFor(simulation);
         _allMoleculeAmounts = allMoleculeAmounts;
         _scaleDivisorOptions = scaleDivisorOptions;
         var previousScaleDivisor = retrievePreviousScaleDivisor();
         try
         {
            resetScaleDivisor();
            settingsFor(simulation).OutputSelections = createOutputSelectionWithAllMoleculeAmountSelected();
            updateSelectedMoleculeAmount(simulation);

            var results = await runSimulation(simulation);
            return computerScaleDivisorBasedOn(results);
         }
         finally
         {
            simulation.Results = previousResults;
            settingsFor(simulation).OutputSelections = previousSelections;
            updateSelectedMoleculeAmount(simulation);
            setPreviousScaleDivisor(previousScaleDivisor);
            _allMoleculeAmounts = null;
            _scaleDivisorOptions = null;
         }
      }

      public IReadOnlyCollection<ScaleDivisor> ResetScaleDivisors(PathCache<IMoleculeAmount> allMoleculeAmounts)
      {
         return allMoleculeAmounts.Keys
            .Select(x => new ScaleDivisor {QuantityPath = x, Value = 1})
            .ToList();
      }

      private void setPreviousScaleDivisor(ICache<string, double> previousScaleDivisor)
      {
         previousScaleDivisor.KeyValues.Each(x => _allMoleculeAmounts[x.Key].ScaleDivisor = x.Value);
      }

      private void resetScaleDivisor()
      {
         _allMoleculeAmounts.Each(m => m.ScaleDivisor = 1);
      }

      private ICache<string, double> retrievePreviousScaleDivisor()
      {
         var previousScaleDivisor = new Cache<string, double>();
         _allMoleculeAmounts.KeyValues.Each(m => previousScaleDivisor.Add(m.Key, m.Value.ScaleDivisor));
         return previousScaleDivisor;
      }

      private IReadOnlyCollection<ScaleDivisor> computerScaleDivisorBasedOn(SimulationRunResults results)
      {
         if (!results.Success)
            return new List<ScaleDivisor>();

         var query = from column in results.Results
            let quantityPath = pathWithoutSimulationName(column.QuantityInfo.PathAsString)
            let moleculeAmount = _allMoleculeAmounts[quantityPath]
            where moleculeAmount != null
            select newScaleDivisor(quantityPath, column);

         return query.ToList();
      }

      private string pathWithoutSimulationName(string simulationPath)
      {
         var quantityPath = _objectPathFactory.CreateObjectPathFrom(simulationPath.ToPathArray());
         quantityPath.Remove(_simulationName);
         return quantityPath.PathAsString;
      }

      private ScaleDivisor newScaleDivisor(string quantityPath, DataColumn column)
      {
         var values = column.Values.AbsoluteValues().Where(x => x > 0).ToList();
         var scaleDivisor = Math.Pow(10, values.ConvertToLog10Array().ArithmeticMean());
         scaleDivisor = Math.Max(scaleDivisor, _scaleDivisorOptions.MinimumScaleDivisor);
         scaleDivisor = roundUp(scaleDivisor);
         return new ScaleDivisor {QuantityPath = quantityPath, Value = scaleDivisor};
      }

      private double roundUp(double scaleDivisor)
      {
         if (_scaleDivisorOptions.UseRoundedValue)
            scaleDivisor = Math.Pow(10, Math.Round(Math.Log10(scaleDivisor)));

         return scaleDivisor;
      }

      private Task<SimulationRunResults> runSimulation(IModelCoreSimulation simulation)
      {
         return Task.Run(() => _simModelManager.RunSimulation(simulation));
      }

      private OutputSelections createOutputSelectionWithAllMoleculeAmountSelected()
      {
         var outputSelection = new OutputSelections();
         foreach (var keyValue in _allMoleculeAmounts.KeyValues)
         {
            outputSelection.AddOutput(new QuantitySelection(keyValue.Key, keyValue.Value.QuantityType));
         }
         return outputSelection;
      }

      private ISimulationSettings settingsFor(IModelCoreSimulation simulation)
      {
         return simulation.BuildConfiguration.SimulationSettings;
      }

      private OutputSelections outputSelectionsFor(IModelCoreSimulation simulation)
      {
         return settingsFor(simulation).OutputSelections;
      }

      private void updateSelectedMoleculeAmount(IModelCoreSimulation simulation)
      {
         var outputSelections = outputSelectionsFor(simulation);
         _allMoleculeAmounts.Each(x => x.Persistable = false);
         foreach (var outputSelection in outputSelections)
         {
            var moleculeAmount = _allMoleculeAmounts[outputSelection.Path];
            if (moleculeAmount == null) continue;
            moleculeAmount.Persistable = true;
         }
      }
   }
}