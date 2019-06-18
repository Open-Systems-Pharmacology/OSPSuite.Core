namespace OSPSuite.Core.Domain.Descriptors
{
   public interface ITagCondition : IDescriptorCondition
   {
      /// <summary>
      ///    Returns the underlying tag associated with the condition
      /// </summary>
      string Tag { get; }

      /// <summary>
      ///    Returns the semantic display of the condition for the tag
      /// </summary>
      string Condition { get; }
   }

   public abstract class TagCondition : ITagCondition
   {
      private readonly string _type;

      public string Tag { get; private set; }

      protected TagCondition(string type)
      {
         _type = type;
      }

      protected TagCondition(string tag, string type) : this(type)
      {
         Tag = tag;
      }

      public virtual string Condition => $"{_type.ToUpper()} {Tag}";

      public abstract IDescriptorCondition CloneCondition();

      public virtual void Replace(string keyword, string replacement)
      {
         if (string.Equals(Tag, keyword))
            Tag = replacement;
      }

      protected bool Equals(TagCondition other)
      {
         return string.Equals(Tag, other.Tag);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != this.GetType()) return false;
         return Equals((TagCondition) obj);
      }

      public override int GetHashCode() => Condition.GetHashCode();

      public abstract bool IsSatisfiedBy(EntityDescriptor entityDescriptor);

      public override string ToString() => Condition;
   }
}