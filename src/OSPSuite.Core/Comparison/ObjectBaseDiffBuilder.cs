using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class ObjectBaseDiffBuilder : DiffBuilder<IObjectBase>
   {
      public override void Compare(IComparison<IObjectBase> comparison)
      {
         if (comparison.Settings.OnlyComputingRelevant)
            return;

         CompareStringValues(x => x.Name, x => x.Name, comparison);
         CompareStringValues(x => x.Icon, x => x.Icon, comparison);
         CompareStringValues(x => x.Description, x => x.Description, comparison);
      }
   }
}