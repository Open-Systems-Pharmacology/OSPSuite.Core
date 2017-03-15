namespace OSPSuite.Core.Domain.Descriptors
{
   public class MatchAllCondition : ITagCondition
   {
      public bool IsSatisfiedBy(Tags item)
      {
         return true;
      }

      public IDescriptorCondition CloneCondition()
      {
         return new MatchAllCondition();
      }

      public override string ToString()
      {
         return Constants.ALL_TAG.ToUpper();
      }

      public void Replace(string keyword, string replacement)
      {
         /*nothing to do*/
      }

      public string Tag
      {
         get { return Constants.ALL_TAG; }
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

      public override int GetHashCode()
      {
         return Tag.GetHashCode();
      }
   }
}