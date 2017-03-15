using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class NotMatchTagCondition : ITagCondition
   {
      public string Tag { get; private set; }

      [Obsolete("For serialization")]
      public NotMatchTagCondition()
      {
      }

      public override string ToString()
      {
         return $"{Constants.NOT.ToUpper()} {Tag}";
      }

      public NotMatchTagCondition(string notMatchTag)
      {
         Tag = notMatchTag;
      }

      public bool IsSatisfiedBy(Tags tags)
      {
         return !tags.Contains(Tag);
      }

      public IDescriptorCondition CloneCondition()
      {
         return new NotMatchTagCondition(Tag);
      }

      public void Replace(string keyword, string replacement)
      {
         if (string.Equals(Tag, keyword))
            Tag = replacement;
      }

      public override bool Equals(object otherObject)
      {
         var other = otherObject as NotMatchTagCondition;
         if (other == null) return false;
         return Tag.Equals(other.Tag);
      }

      public bool Equals(NotMatchTagCondition other)
      {
         if (other==null) return false;
         return string.Equals(Tag, other.Tag);
      }

      public override int GetHashCode()
      {
         //Match and NoMatch CANNOT return the same hash for the same tag
         int hash = 31; //one prime number that should differ from the one in Match
         if (Tag != null)
            hash = hash + Tag.GetHashCode();
         return hash;
      }
   }
}