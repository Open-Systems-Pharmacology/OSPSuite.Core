using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public class CalculationMethodCache : IUpdatable, IEnumerable<CalculationMethod>
   {
      private readonly ICache<string, CalculationMethod> _calculationMethods;

      public CalculationMethodCache()
      {
         _calculationMethods = new Cache<string, CalculationMethod>(cm => cm.Name);
      }

      public virtual void AddCalculationMethod(CalculationMethod calculationMethod)
      {
         _calculationMethods.Add(calculationMethod);
      }

      public virtual void RemoveCalculationMethod(CalculationMethod calculationMethod)
      {
         if (!Contains(calculationMethod))
            return;

         _calculationMethods.Remove(calculationMethod.Name);
      }

      /// <summary>
      /// Returns the calculation method defined for the category named <paramref name="category"/> or null if the category is not defined
      /// </summary>
      public virtual CalculationMethod CalculationMethodFor(string category)
      {
         return _calculationMethods.FirstOrDefault(x => string.Equals(x.Category, category));
      }

      public virtual bool Contains(CalculationMethod calculationMethod)
      {
         if (calculationMethod == null)
            return false;
         return Contains(calculationMethod.Name);
      }

      public virtual bool Contains(string calculationMethod)
      {
         return _calculationMethods.Contains(calculationMethod);
      }

      public virtual IEnumerable<CalculationMethod> All()
      {
         return _calculationMethods;
      }

      public CalculationMethodCache Clone()
      {
         var clone = new CalculationMethodCache();
         clone.UpdatePropertiesFrom(this, null);
         return clone;
      }

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         var sourceCalculationMethodCache = source as CalculationMethodCache;
         if (sourceCalculationMethodCache == null) return;
         Clear();
         sourceCalculationMethodCache.Each(AddCalculationMethod);
      }

      public IEnumerator<CalculationMethod> GetEnumerator()
      {
         return _calculationMethods.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public void Clear()
      {
         _calculationMethods.Clear();
      }
   }
}