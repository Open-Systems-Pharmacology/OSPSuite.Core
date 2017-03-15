using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Data
{
   /// <summary>
   ///    Simple cache from parameter names to values in core dimension
   /// </summary>
   public class PKValues
   {
      private readonly ICache<string, float?> _pkValues;

      public PKValues()
      {
         //returns null for an unknown value
         _pkValues = new Cache<string, float?>(s => (float?) null);
      }

      public virtual void AddValue(string pkParameter, float? value)
      {
         _pkValues[pkParameter] = value;
      }

      public virtual float? ValueFor(string pkParameter)
      {
         return _pkValues[pkParameter];
      }

      public virtual bool HasValueFor(string pkParameter)
      {
         return _pkValues.Contains(pkParameter);
      }

      /// <summary>
      ///    Returns the value defined for the parameter if available or <c>float.NaN</c> otherwise
      /// </summary>
      public virtual float? this[string pkParameter]
      {
         get { return ValueFor(pkParameter); }
      }

      public virtual float ValueOrDefaultFor(string pkParameter)
      {
         return ValueFor(pkParameter).GetValueOrDefault(float.NaN);
      }
   }
}
