namespace OSPSuite.Core.Domain.Descriptors
{
   public class InParentCondition : TagCondition   
   {
      public InParentCondition() : base(Constants.IN_PARENT, Constants.IN_PARENT)
      {
      }

      public override string Condition { get; } = Constants.IN_PARENT.ToUpper();

      //false because this condition should never be evaluated as it will need to be replaced
      //by some InContainer conditions in the model
      public override bool IsSatisfiedBy(EntityDescriptor item) => false;

      public override ITagCondition CloneCondition() => new InParentCondition();

      public override void Replace(string keyword, string replacement)
      {
         /*nothing to do*/
      }
   }
}