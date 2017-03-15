using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class MatchTagCondition : ITagCondition
   {
      public string Tag { get; private set; }

      [Obsolete("For serialization")]
      public MatchTagCondition()
      {
      }

      public override string ToString()
      {
         return Tag;
      }

      public MatchTagCondition(string matchTag)
      {
         Tag = matchTag;
      }

      public bool IsSatisfiedBy(Tags tags)
      {
         return tags.Contains(Tag);
      }

      public IDescriptorCondition CloneCondition()
      {
         return new MatchTagCondition(Tag);
      }

      public void Replace(string keyword, string replacement)
      {
         if(string.Equals(Tag,keyword))
            Tag = replacement;
      }

      public override bool Equals(object otherObject)
      {
         var other = otherObject as MatchTagCondition;
         if (other == null) return false;
         return Tag.Equals(other.Tag);
      }

      public bool Equals(MatchTagCondition other)
      {
         if (other == null) return false;
         return string.Equals(Tag,other.Tag);
      }

      public override int GetHashCode()
      {
         //Match and NoMatch CANNOT return the same hash for the same tag
         int hash = 29; //one prime number that should differ from the one in Not Match
         if (Tag != null)
            hash = hash + Tag.GetHashCode();
         return hash;
      }
   }
}