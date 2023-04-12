using System;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   public class ModuleConfiguration : IVisitable<IVisitor>
   {
      public Module Module { get; }

      /// <summary>
      ///    Reference to selected molecule start value in the Module (can be null if none is used)
      /// </summary>
      public MoleculeStartValuesBuildingBlock SelectedMoleculeStartValues { get; set; }

      /// <summary>
      ///    Reference to selected parameter start value in the Module (can be null if none is used)
      /// </summary>
      public ParameterStartValuesBuildingBlock SelectedParameterStartValues { get; set; }

      [Obsolete("For serialization")]
      public ModuleConfiguration()
      {
      }

      /// <summary>
      ///    Create a new module configuration for the given module and will assign the first molecule and parameter start values
      ///    if defined
      /// </summary>
      /// <param name="module">Module</param>
      public ModuleConfiguration(Module module) : this(module, module.MoleculeStartValuesCollection.FirstOrDefault(), module.ParameterStartValuesCollection.FirstOrDefault())
      {
         Module = module;
      }

      /// <summary>
      ///    Create a new module configuration for the given module and the selected molecule and parameter start values
      /// </summary>
      /// <param name="module">Module</param>
      /// <param name="selectedMoleculeStartValueBuilding">molecule start values. Assuming this belongs to the module or is null</param>
      /// <param name="selectedParameterStartValues">parameter start values. Assuming this belongs to the module or is null</param>
      public ModuleConfiguration(Module module, MoleculeStartValuesBuildingBlock selectedMoleculeStartValueBuilding, ParameterStartValuesBuildingBlock selectedParameterStartValues)
      {
         Module = module;
         SelectedMoleculeStartValues = selectedMoleculeStartValueBuilding;
         SelectedParameterStartValues = selectedParameterStartValues;
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
            case Type molStartValuesType when molStartValuesType == typeof(MoleculeStartValuesBuildingBlock):
               return SelectedMoleculeStartValues as T;
            case Type paramStartValuesType when paramStartValuesType == typeof(ParameterStartValuesBuildingBlock):
               return SelectedParameterStartValues as T;
            default:
               return null;
         }
      }
   }
}