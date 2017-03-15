namespace OSPSuite.Core.Domain.Descriptors
{
   public class Tag
   {
      public string Value { get; set; }

      public Tag() : this(string.Empty)
      {
      }

      public Tag(string value)
      {
         Value = value;
      }

      public bool Equals(Tag other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Equals(other.Value, Value);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != typeof (Tag)) return false;
         return Equals((Tag) obj);
      }

      public override int GetHashCode()
      {
         return (Value != null ? Value.GetHashCode() : 0);
      }
   }
}