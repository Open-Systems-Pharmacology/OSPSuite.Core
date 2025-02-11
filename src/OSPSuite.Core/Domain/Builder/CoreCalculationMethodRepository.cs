using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ICoreCalculationMethodRepository
   {
      IEnumerable<CoreCalculationMethod> GetAllCalculationMethodsFor(string category);
      void AddCalculationMethod(CoreCalculationMethod calculationMethod);
      IEnumerable<CoreCalculationMethod> GetAllCategoriesDefault();
      IEnumerable<CoreCalculationMethod> All();
   }

   public class CoreCalculationMethodRepository : ICoreCalculationMethodRepository
   {
      private readonly List<CoreCalculationMethod> _allMethods;
      private bool _cachedDefaults;
      private IEnumerable<CoreCalculationMethod> _defaults;

      public CoreCalculationMethodRepository()
      {
         _allMethods = new List<CoreCalculationMethod>();
      }

      public IEnumerable<CoreCalculationMethod> GetAllCalculationMethodsFor(string category)
      {
         return _allMethods.Where(cm => cm.Category.Equals(category));
      }

      public void AddCalculationMethod(CoreCalculationMethod calculationMethod)
      {
         _allMethods.Add(calculationMethod);
      }

      public IEnumerable<CoreCalculationMethod> GetAllCategoriesDefault()
      {
         if (!_cachedDefaults)
         {
            var defaults = new List<CoreCalculationMethod>();
            GetAllCategories().Each(cat => defaults.Add(GetAllCalculationMethodsFor(cat).First()));
            _defaults = defaults;
            _cachedDefaults = true;
         }

         return _defaults;
      }

      public IEnumerable<CoreCalculationMethod> All() => _allMethods.Select(x => x);

      public IEnumerable<string> GetAllCategories() => _allMethods.Select(cm => cm.Category).Distinct();
   }
}