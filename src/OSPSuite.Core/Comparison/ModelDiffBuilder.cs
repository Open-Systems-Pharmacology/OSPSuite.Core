using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class ModelDiffBuilder : DiffBuilder<IModel>
   {
      private readonly IObjectComparer _objectComparer;

      public ModelDiffBuilder(IObjectComparer objectComparer)
      {
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IModel> comparison)
      {
         //Pass common ancestor here (simulation) so that message and type displayed to user reflects its expectations. (Model is only used internally)
         _objectComparer.Compare(comparison.ChildComparison(x => x.Root, comparison.CommonAncestor));

         //Neighborhood are defined as child container of the Model root container so no need to compare neighborhoods as well
      }
   }
}