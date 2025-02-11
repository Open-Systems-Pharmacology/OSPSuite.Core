using System;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{

   public class ModuleConfiguration : IVisitable<IVisitor>, IUpdatable
   {
      public Module Module { get; private set; }

      /// <summary>
      ///    Reference to selected initial conditions in the Module (can be null if none is used)
      /// </summary>
      public InitialConditionsBuildingBlock SelectedInitialConditions { get; set; }

      /// <summary>
      ///    Reference to selected parameter value in the Module (can be null if none is used)
      /// </summary>
      public ParameterValuesBuildingBlock SelectedParameterValues { get; set; }

      /// <summary>
      /// Merge behavior for merging spatial structures from different modules.
      /// </summary>
      public MergeBehavior MergeBehavior => Module.MergeBehavior;

      [Obsolete("For serialization")]
      public ModuleConfiguration()
      {
      }

      /// <summary>
      ///    Create a new module configuration for the given module and will assign the first molecule and parameter values
      ///    if defined
      /// </summary>
      /// <param name="module">Module</param>
      public ModuleConfiguration(Module module) : this(module, module.InitialConditionsCollection.FirstOrDefault(), module.ParameterValuesCollection.FirstOrDefault())
      {
      }

      /// <summary>
      ///    Create a new module configuration for the given module and the selected molecule and parameter values
      /// </summary>
      /// <param name="module">Module</param>
      /// <param name="selectedInitialConditionBuilding">Initial Conditions. Assuming this belongs to the module or is null</param>
      /// <param name="selectedParameterValues">Parameter values. Assuming this belongs to the module or is null</param>
      public ModuleConfiguration(Module module, InitialConditionsBuildingBlock selectedInitialConditionBuilding, ParameterValuesBuildingBlock selectedParameterValues)
      {
         Module = module;
         SelectedInitialConditions = selectedInitialConditionBuilding;
         SelectedParameterValues = selectedParameterValues;
      }

      public void AcceptVisitor(IVisitor visitor)
      {
         Module.AcceptVisitor(visitor);
      }

      public T BuildingBlock<T>() where T : class, IBuildingBlock
      {
         switch (typeof(T))
         {
            case Type eventType when eventType == typeof(EventGroupBuildingBlock):
               return Module.EventGroups as T;
            case Type moleculeType when moleculeType == typeof(MoleculeBuildingBlock):
               return Module.Molecules as T;
            case Type observerType when observerType == typeof(ObserverBuildingBlock):
               return Module.Observers as T;
            case Type reactionType when reactionType == typeof(ReactionBuildingBlock):
               return Module.Reactions as T;
            case Type transportType when transportType == typeof(PassiveTransportBuildingBlock):
               return Module.PassiveTransports as T;
            case Type spStrType when spStrType == typeof(SpatialStructure):
               return Module.SpatialStructure as T;
            case Type molStartValuesType when molStartValuesType == typeof(InitialConditionsBuildingBlock):
               return SelectedInitialConditions as T;
            case Type paramStartValuesType when paramStartValuesType == typeof(ParameterValuesBuildingBlock):
               return SelectedParameterValues as T;
            default:
               return null;
         }
      }

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         if (!(source is ModuleConfiguration sourceConfiguration))
            return;

         Module = cloneManager.Clone(sourceConfiguration.Module);
         SelectedInitialConditions = Module.InitialConditionsCollection.FindByName(sourceConfiguration.SelectedInitialConditions?.Name);
         SelectedParameterValues = Module.ParameterValuesCollection.FindByName(sourceConfiguration.SelectedParameterValues?.Name);
      }
   }
}