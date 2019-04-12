namespace OSPSuite.Core.Domain.Descriptors
{
   public class MatchAllCondition : ITagCondition
   {
      public string Tag { get; } = Constants.ALL_TAG;
      public string Condition {get;} =  Constants.ALL_TAG.ToUpper();

      public bool IsSatisfiedBy(EntityDescriptor item)
      {
         return true;
      }

      public IDescriptorCondition CloneCondition()
      {
         return new MatchAllCondition();
      }

      public override string ToString() => Condition;

      public void Replace(string keyword, string replacement)
      {
         /*nothing to do*/
      }


      public override bool Equals(object otherObject)
      {
         var other = otherObject as MatchAllCondition;
         if (other == null) return false;
         return Tag.Equals(other.Tag);
      }

      public bool Equals(MatchAllCondition other)
      {
         return other != null;
      }

      public override int GetHashCode() => Condition.GetHashCode();
   }
}