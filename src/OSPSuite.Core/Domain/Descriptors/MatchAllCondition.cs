namespace OSPSuite.Core.Domain.Descriptors
{
   public class MatchAllCondition : TagCondition
   {
      public MatchAllCondition() : base(Constants.ALL_TAG, Constants.ALL_TAG)
      {
      }

      public override string Condition { get; } = Constants.ALL_TAG.ToUpper();

      public override bool IsSatisfiedBy(EntityDescriptor item) => true;

      public override IDescriptorCondition CloneCondition() => new MatchAllCondition();

      public override void Replace(string keyword, string replacement)
      {
         /*nothing to do*/
      }
   }
}