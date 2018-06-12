using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class SensitivityAnalysis : ObjectBase, IParameterAnalysable
   {
      public virtual ISimulation Simulation { get; set; }
      private readonly List<ISimulationAnalysis> _allSimulationAnalyses = new List<ISimulationAnalysis>();
      private readonly List<SensitivityParameter> _allSensitivityParameters = new List<SensitivityParameter>();

      public SensitivityAnalysisRunResult Results { get; set; }

      public bool IsLoaded { get; set; }

      public bool HasChanged { get; private set; }

      public virtual IReadOnlyList<SensitivityParameter> AllSensitivityParameters => _allSensitivityParameters;

      public virtual void RemoveAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         _allSimulationAnalyses.Remove(simulationAnalysis);
      }

      public virtual void AddAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         _allSimulationAnalyses.Add(simulationAnalysis);
         simulationAnalysis.Analysable = this;
      }

      public IEnumerable<ISimulationAnalysis> Analyses => _allSimulationAnalyses;

      public virtual bool HasResults => Results != null && Results.AllPKParameterSensitivities.Any();

      public virtual bool HasUpToDateResults => Simulation != null && Simulation.HasUpToDateResults;

      public virtual bool ComesFromPKSim => Simulation != null && Simulation.ComesFromPKSim;

      public bool UsesObservedData(DataRepository observedData) => false;

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceSensitivityAnalysis = source as SensitivityAnalysis;
         if (sourceSensitivityAnalysis == null) return;

         IsLoaded = sourceSensitivityAnalysis.IsLoaded;

         _allSensitivityParameters.Clear();
         sourceSensitivityAnalysis.AllSensitivityParameters.Select(cloneManager.Clone).Each(AddSensitivityParameter);
         Simulation = sourceSensitivityAnalysis.Simulation;
      }

      public virtual void AddSensitivityParameter(SensitivityParameter sensitivityParameter)
      {
         _allSensitivityParameters.Add(sensitivityParameter);
         sensitivityParameter.SensitivityAnalysis = this;
         HasChanged = true;
      }

      public virtual void RemoveSensitivityParameter(SensitivityParameter sensitivityParameter)
      {
         _allSensitivityParameters.Remove(sensitivityParameter);
         HasChanged = true;
      }

      public IReadOnlyList<ISimulation> AllSimulations
      {
         get
         {
            var list = new List<ISimulation>();
            if (Simulation != null)
               list.Add(Simulation);

            return list;
         }
      }

      public void SwapSimulations(ISimulation oldSimulation, ISimulation newSimulation)
      {
         AllSensitivityParameters.Each(x => x.UpdateSimulation(newSimulation));
         Simulation = newSimulation;
         HasChanged = true;
      }

      public bool AnalyzesParameter(ParameterSelection parameterSelection)
      {
         return _allSensitivityParameters.Any(x => x.Analyzes(parameterSelection));
      }

      public SensitivityParameter SensitivityParameterByName(string name)
      {
         return _allSensitivityParameters.FindByName(name);
      }

      public IEnumerable<IReadOnlyList<double>> AllParameterVariationsFor(SensitivityParameter sensitivityParameter)
      {
         var parameterIndex = _allSensitivityParameters.IndexOf(sensitivityParameter);
         foreach (var value in sensitivityParameter.VariationValues())
         {
            var currentValues = defaultParameterValues;
            currentValues[parameterIndex] = value;
            yield return currentValues;
         }
      }

      private double[] defaultParameterValues => _allSensitivityParameters.Select(x => x.Parameter.Value).ToArray();

      public IReadOnlyList<string> AllSensitivityParameterPaths => _allSensitivityParameters.Select(x => x.ParameterSelection.Path).ToList();

      public bool Uses(IParameter parameter)
      {
         return _allSensitivityParameters.Select(x => x.Parameter).Contains(parameter);
      }

      public IReadOnlyList<PKParameterSensitivity> AllPKParameterSensitivitiesFor(string pkParameterName, string outputPath, double totalSensitivityThreshold)
      {
         var allPossiblePKParameterSensitivities = allPKParametersForSelectionWithDefinedSensitivity(pkParameterName, outputPath).OrderByDescending(x => Math.Abs(x.Value));
         return sensitivitiesUpToTotalSensitivity(allPossiblePKParameterSensitivities, totalSensitivityThreshold).ToList();
      }

      private IEnumerable<PKParameterSensitivity> allPKParametersForSelectionWithDefinedSensitivity(string pkParameterName, string outputPath)
      {
         return Results.AllFor(pkParameterName, outputPath).Where(x => !double.IsNaN(x.Value));
      }

      private static IEnumerable<PKParameterSensitivity> sensitivitiesUpToTotalSensitivity(IOrderedEnumerable<PKParameterSensitivity> orderedSensitivities, double totalSensitivityThreshold)
      {
         var totalSensitivity = orderedSensitivities.Sum(x => Math.Abs(x.Value));
         var runningSensitivity = 0.0;
         return orderedSensitivities.TakeWhile(x =>
         {
            if (runningSensitivity / totalSensitivity >= totalSensitivityThreshold)
               return false;

            runningSensitivity += Math.Abs(x.Value);
            return true;
         });
      }

      public bool UsesSimulation(ISimulation oldSimulation) => Equals(Simulation, oldSimulation);

      public void RemoveAllSensitivityParameters()
      {
         AllSensitivityParameters.ToList().Each(RemoveSensitivityParameter);
      }
   }
}