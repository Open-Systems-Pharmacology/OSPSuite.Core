﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.R.Domain
{
   public class Simulation : ISimulation
   {
      public IModelCoreSimulation CoreSimulation { get; }

      public bool IsLoaded { get; set; } = true;
      public bool HasUpToDateResults { get; } = true;
      public bool ComesFromPKSim { get; } = false;
      public bool UsesObservedData(DataRepository observedData) => false;
      public IEnumerable<CurveChart> Charts { get; } = new List<CurveChart>();
      public IReadOnlyList<OutputMapping> AllOutputMappings { get; }

      public void RemoveAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         // nothing to do in R
      }

      public IEnumerable<ISimulationAnalysis> Analyses { get; } = new List<ISimulationAnalysis>();

      public void AddAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         // nothing to do in R
      }

      public Simulation(IModelCoreSimulation modelCoreSimulation)
      {
         CoreSimulation = modelCoreSimulation;
      }

      public string Name
      {
         get => CoreSimulation.Name;
         set => CoreSimulation.Name = value;
      }

      public string Id
      {
         get => CoreSimulation.Id;
         set => CoreSimulation.Id = value;
      }

      public event PropertyChangedEventHandler PropertyChanged
      {
         add => CoreSimulation.PropertyChanged += value;
         remove => CoreSimulation.PropertyChanged -= value;
      }

      public event Action<object> Changed
      {
         add => CoreSimulation.Changed += value;
         remove => CoreSimulation.Changed -= value;
      }

      public void AcceptVisitor(IVisitor visitor) => CoreSimulation.AcceptVisitor(visitor);

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         CoreSimulation.UpdatePropertiesFrom(source, cloneManager);
      }

      public string Description
      {
         get => CoreSimulation.Description;
         set => CoreSimulation.Description = value;
      }

      public string Icon
      {
         get => CoreSimulation.Icon;
         set => CoreSimulation.Icon = value;
      }

      public CreationMetaData Creation
      {
         get => CoreSimulation.Creation;
         set => CoreSimulation.Creation = value;
      }

      public IModel Model
      {
         get => CoreSimulation.Model;
         set => CoreSimulation.Model = value;
      }

      public IBuildConfiguration BuildConfiguration => CoreSimulation.BuildConfiguration;

      public OutputSelections OutputSelections => CoreSimulation.OutputSelections;

      public double? EndTime => CoreSimulation.EndTime;

      public ISimulationSettings SimulationSettings => CoreSimulation.SimulationSettings;

      public IReactionBuildingBlock Reactions => CoreSimulation.Reactions;

      public IReadOnlyList<string> CompoundNames => CoreSimulation.CompoundNames;

      public IEnumerable<T> All<T>() where T : class, IEntity => CoreSimulation.All<T>();

      public IParameter BodyWeight => CoreSimulation.BodyWeight;

      public IParameter TotalDrugMassFor(string moleculeName) => CoreSimulation.TotalDrugMassFor(moleculeName);

      public double? MolWeightFor(IQuantity quantity) => CoreSimulation.MolWeightFor(quantity);

      public double? MolWeightFor(string quantityPath) => CoreSimulation.MolWeightFor(quantityPath);
   }
}