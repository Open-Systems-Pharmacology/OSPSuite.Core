namespace OSPSuite.Core.Domain.Descriptors
{
   public class InChildrenCondition : TagCondition
   {
      public InChildrenCondition() : base(Constants.IN_CHILDREN, Constants.IN_CHILDREN)
      {
      }

      public override string Condition { get; } = Constants.IN_CHILDREN.ToUpper();

      //false because this condition should never be evaluated as it will need to be replaced
      //by some InContainer conditions in the model
      public override bool IsSatisfiedBy(EntityDescriptor item) => false;

      public override ITagCondition CloneCondition() => new InChildrenCondition();

      public override void Replace(string keyword, string replacement)
      {
         /*nothing to do*/
      }
   }
}