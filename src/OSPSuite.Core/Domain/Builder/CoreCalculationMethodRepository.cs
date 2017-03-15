using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ICoreCalculationMethodRepository
   {
      IEnumerable<ICoreCalculationMethod> GetAllCalculationMethodsFor(string category);
      void AddCalculationMethod(ICoreCalculationMethod calculationMethod);
      IEnumerable<ICoreCalculationMethod> GetAllCategoriesDefault();
      IEnumerable<ICoreCalculationMethod> All();
   }

   public class CoreCalculationMethodRepository : ICoreCalculationMethodRepository
   {
      private readonly IList<ICoreCalculationMethod> _allMethods;
      private bool _cachedDefaults;
      private IEnumerable<ICoreCalculationMethod> _defaults;

      public CoreCalculationMethodRepository()
      {
         _allMethods = new List<ICoreCalculationMethod>();
      }

      public IEnumerable<ICoreCalculationMethod> GetAllCalculationMethodsFor(string category)
      {
         return _allMethods.Where(cm => cm.Category.Equals(category));
      }

      public void AddCalculationMethod(ICoreCalculationMethod calculationMethod)
      {
         _allMethods.Add(calculationMethod);
      }

      public IEnumerable<ICoreCalculationMethod> GetAllCategoriesDefault()
      {
         if (!_cachedDefaults)
         {
            var defaults = new List<ICoreCalculationMethod>();
            GetAllCategories().Each(cat => defaults.Add(GetAllCalculationMethodsFor(cat).First()));
            _defaults = defaults;
            _cachedDefaults = true;
         }
         return _defaults;
      }

      public IEnumerable<ICoreCalculationMethod> All()
      {
         return _allMethods.Select(x => x);
      }

      public IEnumerable<string> GetAllCategories()
      {
         return _allMethods.Select(cm => cm.Category).Distinct();
      }
   }
}