namespace OSPSuite.Core.Domain.Descriptors
{
   public class InChildrenCondition : TagCondition
   {
      public InChildrenCondition() : base(Constants.IN_CHILDREN, Constants.IN_CHILDREN)
      {
      }

      public override string Condition { get; } = Constants.IN_CHILDREN.ToUpper();

      public override bool IsSatisfiedBy(EntityDescriptor item) => false;

      public override ITagCondition CloneCondition() => new InChildrenCondition();

      public override void Replace(string keyword, string replacement)
      {
         /*nothing to do*/
      }
   }
}