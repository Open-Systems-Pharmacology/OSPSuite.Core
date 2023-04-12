using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Builder
{
   public class SimulationConfiguration : IVisitable<IVisitor>
   {
      private readonly List<ExpressionProfileBuildingBlock> _expressionProfiles = new List<ExpressionProfileBuildingBlock>();
      private readonly List<ModuleConfiguration> _moduleConfigurations = new List<ModuleConfiguration>();
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

      public IReadOnlyList<T> All<T>() where T : class, IBuildingBlock
      {
         return ModuleConfigurations.Select(x=>x.BuildingBlock<T>()).Where(x=>x!=null).ToList();
      }

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         ModuleConfigurations.Each(x => x.AcceptVisitor(visitor));
         Individual?.AcceptVisitor(visitor);
         _expressionProfiles.Each(x => x.AcceptVisitor(visitor));
      }
   }
}