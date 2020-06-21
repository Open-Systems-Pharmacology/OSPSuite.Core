using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Data
{
   /// <summary>
   ///    Simple cache from parameter names to values in core dimension
   /// </summary>
   public class PKValues
   {
      //returns null for an unknown value
      public ICache<string, float?> Values { get; } = new Cache<string, float?>(s => (float?)null);

      public virtual void AddValue(string pkParameter, float? value) => Values[pkParameter] = value;

      public virtual float? ValueFor(string pkParameter) => Values[pkParameter];

      public virtual bool HasValueFor(string pkParameter) => Values.Contains(pkParameter);

      /// <summary>
      ///    Returns the value defined for the parameter if available or <c>float.NaN</c> otherwise
      /// </summary>
      public virtual float? this[string pkParameter] => ValueFor(pkParameter);

      public virtual float ValueOrDefaultFor(string pkParameter) => ValueFor(pkParameter).GetValueOrDefault(float.NaN);
   }
}
