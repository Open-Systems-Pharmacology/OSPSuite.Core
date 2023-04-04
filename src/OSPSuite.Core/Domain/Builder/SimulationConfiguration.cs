using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Builder
{
   public class SimulationConfiguration : IVisitable<IVisitor>
   {
      private readonly List<ExpressionProfileBuildingBlock> _expressionProfiles = new List<ExpressionProfileBuildingBlock>();
      private readonly List<ModuleConfiguration> _moduleConfigurations = new List<ModuleConfiguration>();
      private readonly ICache<IObjectBase, IObjectBase> _builderCache = new Cache<IObjectBase, IObjectBase>(onMissingKey: x => null);
      private readonly List<ICoreCalculationMethod> _allCalculationMethods = new List<ICoreCalculationMethod>();

      public SimModelExportMode SimModelExportMode { get; set; } = SimModelExportMode.Full;

      public bool ShouldValidate { get; set; } = true;
      public bool ShowProgress { get; set; } = true;
      public bool PerformCircularReferenceCheck { get; set; } = true;

      public virtual IndividualBuildingBlock Individual { get; set; }
      public virtual SimulationSettings SimulationSettings { get; set; }

      public virtual IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfiles => _expressionProfiles;
      public virtual IReadOnlyList<ICoreCalculationMethod> AllCalculationMethods => _allCalculationMethods;
      public virtual IReadOnlyList<ModuleConfiguration> ModuleConfigurations => _moduleConfigurations;

      public virtual void AddExpressionProfile(ExpressionProfileBuildingBlock expressionProfile) => _expressionProfiles.Add(expressionProfile);

      public virtual void AddModuleConfiguration(ModuleConfiguration moduleConfiguration) => _moduleConfigurations.Add(moduleConfiguration);

      public virtual void AddCalculationMethod(ICoreCalculationMethod calculationMethodToAdd) => _allCalculationMethods.Add(calculationMethodToAdd);

      public virtual IReadOnlyList<ISpatialStructure> SpatialStructures => all(x => x.SpatialStructure);
      public virtual IReadOnlyList<IPassiveTransportBuildingBlock> PassiveTransports => all(x => x.PassiveTransports);
      public virtual IReadOnlyList<IReactionBuildingBlock> Reactions => all(x => x.Reactions);
      public virtual IReadOnlyList<ParameterStartValuesBuildingBlock> ParameterStartValues => all(x => x.ParameterStartValuesCollection);
      public virtual IReadOnlyList<MoleculeStartValuesBuildingBlock> MoleculeStartValues => all(x => x.MoleculeStartValuesCollection);
      public virtual IReadOnlyList<IEventGroupBuildingBlock> EventGroups => all(x => x.EventGroups);
      public virtual IReadOnlyList<IObserverBuildingBlock> Observers => all(x => x.Observers);

      //There are ways to cache this a bit better
      public virtual IReadOnlyList<MoleculeBuildingBlock> Molecules => all(x => x.Molecules);

      public IMoleculeBuilder MoleculeByName(string name)
      {
         //NOT EFFICIENT!!
         return Molecules.SelectMany(x => x).FindByName(name);
      }

      private IReadOnlyList<T> all<T>(Func<Module, T> propAccess) where T : IBuildingBlock =>
         _moduleConfigurations.Select(x => propAccess(x.Module)).ToList();
         
      private IReadOnlyList<T> all<T>(Func<Module, IReadOnlyList<T>> propAccess) where T : IBuildingBlock =>
         _moduleConfigurations.SelectMany(x => propAccess(x.Module)).ToList();

      public virtual IEnumerable<IMoleculeBuilder> AllPresentMolecules() => 
         _moduleConfigurations.SelectMany(x => x.AllPresentMolecules());

      public virtual IEnumerable<MoleculeStartValue> AllPresentMoleculeValues() =>
         _moduleConfigurations.SelectMany(x => x.AllPresentMoleculeValues());

      public virtual IEnumerable<MoleculeStartValue> AllPresentMoleculeValuesFor(IEnumerable<string> moleculeNames) => 
         _moduleConfigurations.SelectMany(x => x.AllPresentMoleculeValuesFor(moleculeNames));

      public virtual IEnumerable<IMoleculeBuilder> AllFloatingMolecules() => Molecules.SelectMany(x => x.AllFloating());

      public virtual IReadOnlyList<string> AllPresentMoleculeNames() => AllPresentMoleculeNames(x => true);

      //Uses toArray so that the marshaling to R works out of the box (array vs list)
      public virtual IReadOnlyList<string> AllPresentMoleculeNames(Func<IMoleculeBuilder, bool> query) =>
         AllPresentMolecules().Where(query).Select(x => x.Name).ToArray();

      public virtual IReadOnlyList<string> AllPresentFloatingMoleculeNames() =>
         AllPresentMoleculeNames(m => m.IsFloating);

      public virtual IReadOnlyList<string> AllPresentStationaryMoleculeNames() =>
         AllPresentMoleculeNames(m => !m.IsFloating);

      public virtual IReadOnlyList<string> AllPresentXenobioticFloatingMoleculeNames() =>
         AllPresentMoleculeNames(m => m.IsFloating && m.IsXenobiotic);

      public virtual IReadOnlyList<string> AllPresentEndogenousStationaryMoleculeNames() =>
         AllPresentMoleculeNames(m => !m.IsFloating && !m.IsXenobiotic);

      public virtual IReadOnlyList<string> AllPresentEndogenousMoleculeNames() => AllPresentMoleculeNames(m => !m.IsXenobiotic);

      public virtual IObjectBase BuilderFor(IObjectBase modelObject) => _builderCache[modelObject];

      public virtual void ClearCache() => _builderCache.Clear();

      public virtual void AddBuilderReference(IObjectBase modelObject, IObjectBase builder)
      {
         _builderCache[modelObject] = builder;
      }

      //TODO clone? Update from?

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         ModuleConfigurations.Each(x => x.AcceptVisitor(visitor));
         Individual?.AcceptVisitor(visitor);
         _expressionProfiles.Each(x => x.AcceptVisitor(visitor));
      }
   }
}