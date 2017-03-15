using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Comparison
{
   public class ObjectPathDiffBuilder : DiffBuilder<ObjectPath>
   {
      public override void Compare(IComparison<ObjectPath> comparison)
      {
         CompareValues(x => x.PathAsString, Captions.Diff.ObjectPath, comparison, (s, s1) => arePathEquals(s, s1, comparison.CommonAncestor as IFormula));
      }

      private bool arePathEquals(string path1, string path2, IFormula formula)
      {
         if (string.Equals(path1, path2))
            return true;

         //only compare path without the first item for formula with resolved references
         if (formula == null || !formula.AreReferencesResolved)
            return false;

         var objectPath1 = new ObjectPath(path1.ToPathArray());
         var objectPath2 = new ObjectPath(path2.ToPathArray());

         if (objectPath1.Count != objectPath2.Count)
            return false;

         //we need at least two items in the path to remove the first entry
         if (objectPath1.Count > 1) { 
            objectPath1.RemoveFirst();
            objectPath2.RemoveFirst();
         }

         return Equals(objectPath1, objectPath2);
      }
   }
}