using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Builder
{
   public class SimulationConfiguration : IVisitable<IVisitor>, IUpdatable
   {
      private readonly List<ExpressionProfileBuildingBlock> _expressionProfiles = new List<ExpressionProfileBuildingBlock>();
      private readonly List<ModuleConfiguration> _moduleConfigurations = new List<ModuleConfiguration>();
      private readonly List<CoreCalculationMethod> _allCalculationMethods = new List<CoreCalculationMethod>();
      private readonly Cache<string, MoleculeCalculationMethodOverride> _moleculeCalculationMethodOverrides = new(onMissingKey: x => new MoleculeCalculationMethodOverride(x));

      public SimModelExportMode SimModelExportMode { get; set; } = SimModelExportMode.Full;
      public bool ShouldValidate { get; set; } = true;
      /// <summary>
      ///    Indicates whether model construction reports progress (default is <c>true</c>). Set to <c>false</c> for each
      ///    configuration constructed in parallel — every construction otherwise publishes its own progress stream into
      ///    the shared event publisher (see <see cref="IModelConstructor.CreateModelFrom" />).
      /// </summary>
      public bool ShowProgress { get; set; } = true;
      public bool PerformCircularReferenceCheck { get; set; } = true;
      public bool CreateAllProcessRateParameters { get; set; }

      public virtual IndividualBuildingBlock Individual { get; set; }
      public virtual SimulationSettings SimulationSettings { get; set; }

      public virtual IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfiles => _expressionProfiles;
      public virtual IReadOnlyList<CoreCalculationMethod> AllCalculationMethods => _allCalculationMethods;
      public virtual IReadOnlyList<ModuleConfiguration> ModuleConfigurations => _moduleConfigurations;

      public virtual void AddExpressionProfile(ExpressionProfileBuildingBlock expressionProfile) => _expressionProfiles.Add(expressionProfile);

      public virtual void AddModuleConfiguration(ModuleConfiguration moduleConfiguration) => _moduleConfigurations.Add(moduleConfiguration);
      public virtual void RemoveModuleConfiguration(ModuleConfiguration moduleConfiguration) => _moduleConfigurations.Remove(moduleConfiguration);

      public virtual IReadOnlyCollection<MoleculeCalculationMethodOverride> AllCalculationMethodOverrides => _moleculeCalculationMethodOverrides;

      public virtual void AddCalculationMethodOverride(MoleculeCalculationMethodOverride moleculeCalculationMethodOverride)
      {
         AddCalculationMethodsOverridesFor(moleculeCalculationMethodOverride.MoleculeName, moleculeCalculationMethodOverride.UsedCalculationMethods);
      }

      /// <summary>
      ///    Adds an override associated with a <paramref name="moleculeName" />. If the molecule already has an override, the
      ///    provided <paramref name="usedCalculationMethods" /> will be added to the existing override.
      /// </summary>
      public virtual void AddCalculationMethodsOverridesFor(string moleculeName, IReadOnlyCollection<UsedCalculationMethod> usedCalculationMethods)
      {
         if (!_moleculeCalculationMethodOverrides.Contains(moleculeName))
            _moleculeCalculationMethodOverrides[moleculeName] = new MoleculeCalculationMethodOverride(moleculeName);

         usedCalculationMethods.Each(x => _moleculeCalculationMethodOverrides[moleculeName].AddUsedCalculationMethod(x));
      }

      /// <summary>
      ///    Returns calculation method overrides for a <paramref name="moleculeName" />. If no override exists for the given
      ///    molecule, an empty override will be returned.
      /// </summary>
      public virtual MoleculeCalculationMethodOverride CalculationMethodOverridesFor(string moleculeName) => _moleculeCalculationMethodOverrides[moleculeName];

      public virtual void AddCalculationMethod(CoreCalculationMethod calculationMethodToAdd) => _allCalculationMethods.Add(calculationMethodToAdd);

      public IReadOnlyList<T> All<T>() where T : class, IBuildingBlock
      {
         if (typeof(T) == typeof(IBuildingBlock))
            return All().OfType<T>().ToList();

         return ModuleConfigurations.Select(x => x.BuildingBlock<T>()).Where(x => x != null).ToList();
      }

      public IReadOnlyList<IBuildingBlock> All()
      {
         return ModuleConfigurations.SelectMany(x => x.All()).Where(x => x != null).ToList();
      }

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         ModuleConfigurations.Each(x => x.AcceptVisitor(visitor));
         Individual?.AcceptVisitor(visitor);
         _expressionProfiles.Each(x => x.AcceptVisitor(visitor));
      }

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         if (!(source is SimulationConfiguration sourceConfiguration))
            return;

         CopyPropertiesFrom(sourceConfiguration);
         sourceConfiguration.ExpressionProfiles.Each(x => AddExpressionProfile(cloneManager.Clone(x)));
         sourceConfiguration.AllCalculationMethods.Each(AddCalculationMethod);
         sourceConfiguration.ModuleConfigurations.Each(x => AddModuleConfiguration(cloneManager.Clone(x)));
         SimulationSettings = cloneManager.Clone(sourceConfiguration.SimulationSettings);
         Individual = cloneManager.Clone(sourceConfiguration.Individual);
         CreateAllProcessRateParameters = sourceConfiguration.CreateAllProcessRateParameters;
         sourceConfiguration.AllCalculationMethodOverrides.Each(x => AddCalculationMethodOverride(x.Clone()));
      }

      /// <summary>
      ///    Copies the properties from <paramref name="sourceConfiguration" /> but does not clone any of the building blocks
      ///    or module configurations.
      /// </summary>
      /// <param name="sourceConfiguration"></param>
      public void CopyPropertiesFrom(SimulationConfiguration sourceConfiguration)
      {
         SimModelExportMode = sourceConfiguration.SimModelExportMode;
         ShouldValidate = sourceConfiguration.ShouldValidate;
         ShowProgress = sourceConfiguration.ShowProgress;
         PerformCircularReferenceCheck = sourceConfiguration.PerformCircularReferenceCheck;
      }
   }
}