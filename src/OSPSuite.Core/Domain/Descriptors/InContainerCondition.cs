using System;

namespace OSPSuite.Core.Domain.Descriptors
{
   public class InContainerCondition : ITagCondition
   {
      [Obsolete("For serialization")]
      public InContainerCondition()
      {
      }

      public InContainerCondition(string tag)
      {
         Tag = tag;
      }

      public string Tag { get; private set; }

      public string Condition => $"{Constants.IN_CONTAINER.ToUpper()} {Tag}";

      public IDescriptorCondition CloneCondition() => new InContainerCondition(Tag);

      public void Replace(string keyword, string replacement)
      {
         if (string.Equals(Tag, keyword))
            Tag = replacement;
      }

      public bool IsSatisfiedBy(EntityDescriptor entityDescriptor)
      {
         return string.Equals(entityDescriptor?.Container?.Value, Tag);
      }

      protected bool Equals(InContainerCondition other)
      {
         return string.Equals(Tag, other.Tag);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != this.GetType()) return false;
         return Equals((InContainerCondition) obj);
      }

      public override int GetHashCode() => Condition.GetHashCode();

      public override string ToString() => Condition;
   }
}