using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Builder
{
   public static class MoleculeDependentBuilderExtensions
   {
      public static void AddMoleculeName(this IMoleculeDependentBuilder moleculeDependentBuilder, string molecule)
      {
         moleculeDependentBuilder.MoleculeList.AddMoleculeName(molecule);
      }

      public static void RemoveMoleculeName(this IMoleculeDependentBuilder moleculeDependentBuilder, string molecule)
      {
         moleculeDependentBuilder.MoleculeList.RemoveMoleculeName(molecule);
      }

      public static void AddMoleculeNameToExclude(this IMoleculeDependentBuilder moleculeDependentBuilder, string molecule)
      {
         moleculeDependentBuilder.MoleculeList.AddMoleculeNameToExclude(molecule);
      }

      public static void RemoveMoleculeNameToExclude(this IMoleculeDependentBuilder moleculeDependentBuilder, string molecule)
      {
         moleculeDependentBuilder.MoleculeList.RemoveMoleculeNameToExclude(molecule);
      }

      public static IEnumerable<string> MoleculeNames(this IMoleculeDependentBuilder moleculeDependentBuilder)
      {
         return moleculeDependentBuilder.MoleculeList.MoleculeNames;
      }

      public static IEnumerable<string> MoleculeNamesToExclude(this IMoleculeDependentBuilder moleculeDependentBuilder)
      {
         return moleculeDependentBuilder.MoleculeList.MoleculeNamesToExclude;
      }
   }
}