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
      private readonly ICache<IObjectBase, IObjectBase> _builderCache = new Cache<IObjectBase, IObjectBase>(onMissingKey: x => null);
      private readonly List<ICoreCalculationMethod> _allCalculationMethods = new List<ICoreCalculationMethod>();

      public SimModelExportMode SimModelExportMode { get; set; } = SimModelExportMode.Full;

      public bool ShouldValidate { get; set; } = true;
      public bool ShowProgress { get; set; } = true;
      public bool PerformCircularReferenceCheck { get; set; } = true;

      public virtual Module Module { get; set; }
      public virtual IndividualBuildingBlock Individual { get; set; }
      public virtual SimulationSettings SimulationSettings { get; set; }

      public virtual IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfiles => _expressionProfiles;

      public virtual IReadOnlyList<ICoreCalculationMethod> AllCalculationMethods => _allCalculationMethods;

      //TODO
      //just helper method to transition easily to new API
      public virtual ISpatialStructure SpatialStructure => Module?.SpatialStructure;
      public virtual MoleculeBuildingBlock Molecules => Module?.Molecule;
      public virtual IPassiveTransportBuildingBlock PassiveTransports => Module?.PassiveTransport;
      public virtual IReactionBuildingBlock Reactions => Module?.Reaction;
      public virtual IReadOnlyList<ParameterStartValuesBuildingBlock> ParameterStartValuesCollection => Module?.ParameterStartValuesCollection;
      public virtual IReadOnlyList<MoleculeStartValuesBuildingBlock> MoleculeStartValuesCollection => Module?.MoleculeStartValuesCollection;
      public virtual IEventGroupBuildingBlock EventGroups => Module?.EventGroup;
      public virtual IObserverBuildingBlock Observers => Module?.Observer;

      public virtual void AddExpressionProfile(ExpressionProfileBuildingBlock expressionProfile) => _expressionProfiles.Add(expressionProfile);

      public virtual void AddCalculationMethod(ICoreCalculationMethod calculationMethodToAdd)
      {
         _allCalculationMethods.Add(calculationMethodToAdd);
      }

      public virtual IEnumerable<IMoleculeBuilder> AllPresentMolecules()
      {
         if (Module?.Molecule == null)
            return Enumerable.Empty<IMoleculeBuilder>();

         return Module.Molecule.AllPresentFor(Module.MoleculeStartValuesCollection);
      }

      public virtual IEnumerable<MoleculeStartValue> AllPresentMoleculeValues()
      {
         if (Module?.Molecule == null)
            return Enumerable.Empty<MoleculeStartValue>();

         return AllPresentMoleculeValuesFor(Module.Molecule.Select(x => x.Name));
      }

      public virtual IEnumerable<MoleculeStartValue> AllPresentMoleculeValuesFor(IEnumerable<string> moleculeNames)
      {
         if (Module.Molecule == null)
            return Enumerable.Empty<MoleculeStartValue>();

         return Module.MoleculeStartValuesCollection.SelectMany(x => x)
            .Where(msv => moleculeNames.Contains(msv.MoleculeName))
            .Where(msv => msv.IsPresent);
      }

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
         Module?.AcceptVisitor(visitor);
         Individual?.AcceptVisitor(visitor);
         _expressionProfiles.Each(x => x.AcceptVisitor(visitor));
      }
   }
}