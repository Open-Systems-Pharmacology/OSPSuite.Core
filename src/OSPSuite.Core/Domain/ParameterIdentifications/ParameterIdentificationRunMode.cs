using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public abstract class ParameterIdentificationRunMode : IUpdatable
   {
      public string DisplayName { get; }

      public bool IsSingleRun { get; }
      protected ParameterIdentificationRunMode(string displayName, bool isSingleRun)
      {
         DisplayName = displayName;
         IsSingleRun = isSingleRun;
      }

      public ParameterIdentificationRunMode Clone()
      {
         var clone = CreateClone();
         clone.UpdatePropertiesFrom(this, null);
         return clone;
      }

      protected abstract ParameterIdentificationRunMode CreateClone();

      public virtual void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
      }
   }

   public class MultipleParameterIdentificationRunMode : ParameterIdentificationRunMode
   {
      public int NumberOfRuns { get; set; } = Constants.DEFAULT_NUMBER_OF_RUNS_FOR_MULTIPLE_MODE;

      public MultipleParameterIdentificationRunMode() : base(Captions.ParameterIdentification.RunModes.MultipleRuns, isSingleRun:false)
      {
      }

      protected override ParameterIdentificationRunMode CreateClone()
      {
         return new MultipleParameterIdentificationRunMode();
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceMultipleOptimizationOption = source as MultipleParameterIdentificationRunMode;
         if (sourceMultipleOptimizationOption == null) return;
         NumberOfRuns = sourceMultipleOptimizationOption.NumberOfRuns;
      }
   }

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
         get { return CalculationMethodsCache.SelectMany(x => x.All()).Select(x => x.Category).Distinct().ToList(); }
      }

      protected override ParameterIdentificationRunMode CreateClone()
      {
         return new CategorialParameterIdentificationRunMode();
      }

      public bool IsSelected(string compound, CalculationMethod calculationMethod)
      {
         if (AllTheSame)
            return AllTheSameSelection.Contains(calculationMethod);

         var cacheForCompound = CalculationMethodsCache[compound];
         return cacheForCompound != null && cacheForCompound.Contains(calculationMethod);
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

   public class StandardParameterIdentificationRunMode : ParameterIdentificationRunMode
   {
      public StandardParameterIdentificationRunMode() : base(Captions.ParameterIdentification.RunModes.Standard, isSingleRun: true)
      {
      }

      protected override ParameterIdentificationRunMode CreateClone()
      {
         return new StandardParameterIdentificationRunMode();
      }
   }
}