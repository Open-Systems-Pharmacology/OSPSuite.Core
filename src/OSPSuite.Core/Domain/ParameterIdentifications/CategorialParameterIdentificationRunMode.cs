using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class CategorialParameterIdentificationRunMode : ParameterIdentificationRunMode
   {
      public bool AllTheSame { get; set; }
      public CalculationMethodCache AllTheSameSelection { get; } = new CalculationMethodCache();
      public Cache<string, CalculationMethodCache> CalculationMethodsCache { get; } = new Cache<string, CalculationMethodCache>(onMissingKey: x => null);

      public CategorialParameterIdentificationRunMode() : base(Captions.ParameterIdentification.RunModes.Category, isSingleRun: false)
      {
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceCategorialParameterIdentification = source as CategorialParameterIdentificationRunMode;
         if (sourceCategorialParameterIdentification == null) return;
         AllTheSame = sourceCategorialParameterIdentification.AllTheSame;
         CalculationMethodsCache.Clear();
         foreach (var keyValue in sourceCategorialParameterIdentification.CalculationMethodsCache.KeyValues)
         {
            CalculationMethodsCache[keyValue.Key] = keyValue.Value.Clone();
         }
      }

      public IReadOnlyList<string> SelectedCategories
      {
         get
         {
            if (AllTheSame)
               return distinctCategoriesFrom(AllTheSameSelection.All());

            return distinctCategoriesFrom(CalculationMethodsCache.SelectMany(x => x.All()));
         }
      }

      private IReadOnlyList<string> distinctCategoriesFrom(IEnumerable<CalculationMethod> calculationMethods) => calculationMethods.Select(x => x.Category).Distinct().ToList();

      protected override ParameterIdentificationRunMode CreateClone()
      {
         return new CategorialParameterIdentificationRunMode();
      }

      public bool IsSelected(string compound, CalculationMethod calculationMethod)
      {
         return CalculationMethodCacheFor(compound).Contains(calculationMethod);
      }

      public CalculationMethodCache CalculationMethodCacheFor(string compoundName)
      {
         if (AllTheSame)
            return AllTheSameSelection;

         if (!CalculationMethodsCache.Contains(compoundName))
            CalculationMethodsCache[compoundName] = new CalculationMethodCache();

         return CalculationMethodsCache[compoundName];
      }
   }
}