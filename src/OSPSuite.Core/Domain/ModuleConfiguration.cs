using System;
using System.Collections.Generic;
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
      public MoleculeStartValuesBuildingBlock SelectedMoleculeStartValues { get; }

      /// <summary>
      ///    Reference to selected parameter start value in the Module (can be null if none is used)
      /// </summary>
      public ParameterStartValuesBuildingBlock SelectedParameterStartValues { get; }

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

      public virtual IEnumerable<IMoleculeBuilder> AllPresentMolecules()
      {
         if (Module.Molecules == null || SelectedMoleculeStartValues == null)
            return Enumerable.Empty<IMoleculeBuilder>();

         return Module.Molecules.AllPresentFor(SelectedMoleculeStartValues);
      }

      public virtual IEnumerable<MoleculeStartValue> AllPresentMoleculeValues()
      {
         if (Module.Molecules == null)
            return Enumerable.Empty<MoleculeStartValue>();

         return AllPresentMoleculeValuesFor(Module.Molecules.Select(x => x.Name));
      }

      public virtual IEnumerable<MoleculeStartValue> AllPresentMoleculeValuesFor(IEnumerable<string> moleculeNames)
      {
         if (SelectedMoleculeStartValues == null)
            return Enumerable.Empty<MoleculeStartValue>();

         return SelectedMoleculeStartValues
            .Where(msv => moleculeNames.Contains(msv.MoleculeName))
            .Where(msv => msv.IsPresent);
      }
   }
}