using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class IndividualBuildingBlockDiffBuilder : PathAndValueEntityBuildingBlockFromPKSimDiffBuilder<IndividualBuildingBlock, IndividualParameter>
   {
      private readonly IObjectComparer _objectComparer;

      public IndividualBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer, IObjectComparer objectComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IndividualBuildingBlock> comparison)
      {
         base.Compare(comparison);
         _objectComparer.Compare(comparison.ChildComparison(x => x.OriginData));
      }
   }

   public class IndividualParameterDiffBuilder : PathAndValueEntityDiffBuilder<IndividualParameter>
   {
      public IndividualParameterDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder) : base(objectComparer, entityDiffBuilder)
      {
      }
   }
}