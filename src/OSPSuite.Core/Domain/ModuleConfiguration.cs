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

      public ModuleConfiguration(Module module, MoleculeStartValuesBuildingBlock selectedMoleculeStartValueBuilding = null, ParameterStartValuesBuildingBlock selectedParameterStartValues = null)
      {
         Module = module;
         SelectedMoleculeStartValues = selectedMoleculeStartValueBuilding ?? module.MoleculeStartValuesCollection.FirstOrDefault();
         SelectedParameterStartValues = selectedParameterStartValues ?? module.ParameterStartValuesCollection.FirstOrDefault();
      }

      public void AcceptVisitor(IVisitor visitor)
      {
         Module.AcceptVisitor(visitor);
      }
   }
}