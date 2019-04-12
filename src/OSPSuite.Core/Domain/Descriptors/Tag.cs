namespace OSPSuite.Core.Domain.Descriptors
{
   public class Tag
   {
      public string Value { get; }

      public Tag() : this(string.Empty)
      {
      }

      public Tag(string value)
      {
         Value = value ?? string.Empty;
      }

      protected bool Equals(Tag other)
      {
         if (other == null) return false;
         return string.Equals(Value, other.Value);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != this.GetType()) return false;
         return Equals((Tag) obj);
      }

      public override int GetHashCode()
      {
         return (Value != null ? Value.GetHashCode() : 0);
      }
   }
}