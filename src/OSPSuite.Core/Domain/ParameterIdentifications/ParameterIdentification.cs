using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class ParameterIdentification : ObjectBase, IParameterAnalysable, IUsesObservedData
   {
      private readonly List<ISimulation> _allSimulations = new List<ISimulation>();
      private readonly List<IdentificationParameter> _allIdentificationParameters = new List<IdentificationParameter>();
      private readonly List<ParameterIdentificationRunResult> _results = new List<ParameterIdentificationRunResult>();
      public virtual ParameterIdentificationConfiguration Configuration { get; } = new ParameterIdentificationConfiguration();
      public virtual OutputMappings OutputMappings { get; set; } = new OutputMappings();

      public bool IsLoaded { get; set; }
      private readonly List<ISimulationAnalysis> _allSimulationAnalyses = new List<ISimulationAnalysis>();

      /// <summary>
      ///    Indicates if a parameter identification was changed and hence needs to be saved
      /// </summary>
      public bool HasChanged { get; set; }

      public virtual void AddSimulation(ISimulation simulation)
      {
         if (simulation == null) return;
         if (UsesSimulation(simulation))
            return;

         _allSimulations.Add(simulation);
         HasChanged = true;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceParameterIdentification = source as ParameterIdentification;
         if (sourceParameterIdentification == null) return;

         Configuration.UpdatePropertiesFrom(sourceParameterIdentification.Configuration, cloneManager);
         IsLoaded = sourceParameterIdentification.IsLoaded;

         _allSimulations.Clear();
         _allSimulations.AddRange(sourceParameterIdentification.AllSimulations);

         OutputMappings.UpdatePropertiesFrom(sourceParameterIdentification.OutputMappings, cloneManager);

         _allIdentificationParameters.Clear();
         sourceParameterIdentification.AllIdentificationParameters.Select(cloneManager.Clone).Each(AddIdentificationParameter);
      }

      public virtual bool UsesSimulation(ISimulation simulation)
      {
         return (simulation != null && _allSimulations.Contains(simulation)) || AnyOutputOfSimulationMapped(simulation);
      }

      public virtual void RemoveSimulation(ISimulation simulation)
      {
         if (!UsesSimulation(simulation))
            return;

         _allSimulations.Remove(simulation);

         OutputMappings.RemoveOutputsReferencing(simulation);

         removeLinkedParametersReferencing(simulation);

         removeEmptyIdentificationParameters();

         HasChanged = true;
      }

      public virtual bool IsSingleRun => Configuration.RunMode.IsSingleRun;

      private void removeEmptyIdentificationParameters()
      {
         _allIdentificationParameters.Where(x => !x.AllLinkedParameters.Any())
            .ToList().Each(RemoveIdentificationParameter);
      }

      private void removeLinkedParametersReferencing(ISimulation simulation)
      {
         _allIdentificationParameters.Each(x => x.RemovedLinkedParametersForSimulation(simulation));
      }

      public virtual bool HasIdentificationParameter(string name)
      {
         return IdentificationParameterByName(name) != null;
      }

      public virtual void RemoveAllSimulations()
      {
         _allSimulations.ToList().Each(RemoveSimulation);
      }

      public virtual void AddOutputMapping(OutputMapping outputMapping)
      {
         OutputMappings.Add(outputMapping);
         HasChanged = true;
      }

      public virtual void RemoveOutputMapping(OutputMapping outputMapping)
      {
         OutputMappings.Remove(outputMapping);
         HasChanged = true;
      }

      public virtual IReadOnlyList<OutputMapping> AllOutputMappings => OutputMappings.All;
      public virtual IReadOnlyList<IdentificationParameter> AllIdentificationParameters => _allIdentificationParameters;
      public virtual IEnumerable<IdentificationParameter> AllVariableIdentificationParameters => AllIdentificationParameters.Where(x => !x.IsFixed);
      public virtual IEnumerable<IdentificationParameter> AllFixedIdentificationParameters => AllIdentificationParameters.Where(x => x.IsFixed);
      public virtual IReadOnlyList<ISimulation> AllSimulations => _allSimulations;
      public virtual OptimizationAlgorithmProperties AlgorithmProperties => Configuration.AlgorithmProperties;

      public virtual bool AnyOutputOfSimulationMapped(ISimulation simulation)
      {
         return OutputMappings.UsesSimulation(simulation);
      }

      public virtual IdentificationParameter IdentificationParameterByName(string name)
      {
         return _allIdentificationParameters.FindByName(name);
      }

      public virtual IdentificationParameter IdentificationParameterByLinkedPath(string linkedPath)
      {
         return _allIdentificationParameters.FirstOrDefault(x => string.Equals(x.LinkedPath, linkedPath));
      }

      public virtual void AddIdentificationParameter(IdentificationParameter identificationParameter)
      {
         _allIdentificationParameters.Add(identificationParameter);
         identificationParameter.ParameterIdentification = this;
         HasChanged = true;
      }

      public virtual void RemoveIdentificationParameter(IdentificationParameter identificationParameter)
      {
         _allIdentificationParameters.Remove(identificationParameter);
         HasChanged = true;
      }

      public virtual bool IdentifiesParameter(ParameterSelection parameterSelection)
      {
         return _allIdentificationParameters.Any(x => x.LinksParameter(parameterSelection));
      }

      public virtual IReadOnlyList<string> PathOfOptimizedParameterBelongingTo(ISimulation simulation)
      {
         var parameterPaths = new List<string>();
         foreach (var identificationParameter in _allIdentificationParameters)
         {
            parameterPaths.AddRange(identificationParameter.LinkedParametersFor(simulation).Select(x => x.Path));
         }

         return parameterPaths;
      }

      public virtual IReadOnlyList<ParameterIdentificationRunResult> Results => _results;

      public virtual void UpdateResultsWith(IEnumerable<ParameterIdentificationRunResult> results)
      {
         _results.Clear();
         results.Each(AddResult);
      }

      public void AddResult(ParameterIdentificationRunResult runResult)
      {
         _results.Add(runResult);
         HasChanged = true;
      }

      public virtual bool HasResults => _results.Any();

      public virtual void RemoveAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         _allSimulationAnalyses.Remove(simulationAnalysis);
         HasChanged = true;
      }

      public virtual void AddAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         _allSimulationAnalyses.Add(simulationAnalysis);
         simulationAnalysis.Analysable = this;
         HasChanged = true;
      }

      public IEnumerable<ISimulationAnalysis> Analyses => _allSimulationAnalyses;
      public bool HasUpToDateResults => AllSimulations.All(x => x.HasUpToDateResults);
      public bool ComesFromPKSim => AllSimulations.All(x => x.ComesFromPKSim);

      public virtual IEnumerable<OutputMapping> OutputMappingsUsingDataRepository(DataRepository dataRepository) => OutputMappings.OutputMappingsUsingDataRepository(dataRepository);

      public virtual IEnumerable<DataRepository> AllDataRepositoryMappedFor(ISimulation simulation) => OutputMappings.AllDataRepositoryMappedFor(simulation);

      public virtual IEnumerable<OutputMapping> AllOutputMappingsFor(ISimulation simulation) => OutputMappings.AllOutputMappingsFor(simulation);

      public virtual IEnumerable<QuantitySelection> AllOutputsMappedFor(ISimulation simulation) => OutputMappings.AllOutputsMappedFor(simulation);

      public virtual bool UsesOutputMapping(OutputMapping outputMapping) => OutputMappings.Contains(outputMapping);

      public virtual float MinObservedDataTime
      {
         get { return AllObservedData.Select(x => x.BaseGrid.Values.First()).Min(); }
      }

      public virtual float MaxObservedDataTime
      {
         get { return AllObservedData.Select(x => x.BaseGrid.Values.Last()).Max(); }
      }

      public virtual IEnumerable<DataRepository> AllObservedData => AllOutputMappings.Select(x => x.WeightedObservedData.ObservedData);

      public virtual bool UsesObservedData(DataRepository observedData)
      {
         return AllObservedData.Contains(observedData);
      }

      public virtual IEnumerable<DataColumn> AllObservationColumnsFor(string fullOutputPath)
      {
         return AllOutputMappings.Where(x => string.Equals(x.FullOutputPath, fullOutputPath))
            .SelectMany(x => x.WeightedObservedData.ObservedData.ObservationColumns());
      }

      public IEnumerable<string> DistinctCompoundNames()
      {
         return AllSimulations.SelectMany(simulation => simulation.CompoundNames).Distinct();
      }

      public virtual void SwapSimulations(ISimulation oldSimulation, ISimulation newSimulation)
      {
         AllOutputMappings.Where(x => x.UsesSimulation(oldSimulation)).Each(outputMapping => outputMapping.UpdateSimulation(newSimulation));
         AllIdentificationParameters.Each(identificationParameter => identificationParameter.SwapSimulation(oldSimulation, newSimulation));
         _allSimulations.Remove(oldSimulation);
         _allSimulations.Add(newSimulation);
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _allIdentificationParameters.ToList().Each(x => x.AcceptVisitor(visitor));
      }
   }
}