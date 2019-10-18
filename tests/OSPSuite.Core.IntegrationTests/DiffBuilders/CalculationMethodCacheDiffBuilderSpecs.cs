using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.DiffBuilders
{
   public abstract class concern_for_CalculationMethodCacheDiffBuilder : concern_for_ObjectComparer
   {
      protected CalculationMethodCache _calculationMethodCache1;
      protected CalculationMethodCache _calculationMethodCache2;
      protected CalculationMethod _calculationMethodCat1Item1;
      protected CalculationMethod _calculationMethodCat1Item2;
      protected CalculationMethod _calculationMethodCat2Item3;
      protected CalculationMethod _calculationMethodCat2Item4;
      protected CalculationMethod _calculationMethodCat3Item5;

      protected override void Context()
      {
         base.Context();

         _calculationMethodCache1 = new CalculationMethodCache();
         _calculationMethodCache2 = new CalculationMethodCache();

         _calculationMethodCat1Item1 = new CalculationMethod {Category = "Cat1", Name = "Item1", DisplayName = "Item1"};
         _calculationMethodCat1Item2 = new CalculationMethod {Category = "Cat1", Name = "Item2", DisplayName = "Item2"};
         _calculationMethodCat2Item3 = new CalculationMethod {Category = "Cat2", Name = "Item3", DisplayName = "Item3"};
         _calculationMethodCat2Item4 = new CalculationMethod {Category = "Cat2", Name = "Item4", DisplayName = "Item4"};
         _calculationMethodCat3Item5 = new CalculationMethod {Category = "Cat3", Name = "Item5", DisplayName = "Item5"};

         _calculationMethodCache1.AddCalculationMethod(_calculationMethodCat1Item1);
         _calculationMethodCache1.AddCalculationMethod(_calculationMethodCat2Item3);
         _calculationMethodCache1.AddCalculationMethod(_calculationMethodCat3Item5);
         _calculationMethodCache2.AddCalculationMethod(_calculationMethodCat1Item2);
         _calculationMethodCache2.AddCalculationMethod(_calculationMethodCat2Item4);

         _object1 = _calculationMethodCache1;
         _object2 = _calculationMethodCache2;
      }
   }

   public class When_comparing_two_calculation_method_caches : concern_for_CalculationMethodCacheDiffBuilder
   {
      [Observation]
      public void should_return_the_expected_difference()
      {
         _report.Count.ShouldBeEqualTo(3);
         _report[0].Object1.ShouldBeEqualTo(_calculationMethodCat1Item1);
         _report[0].Object2.ShouldBeEqualTo(_calculationMethodCat1Item2);

         _report[1].Object1.ShouldBeEqualTo(_calculationMethodCat2Item3);
         _report[1].Object2.ShouldBeEqualTo(_calculationMethodCat2Item4);

         var missingItem = _report[2].DowncastTo<MissingDiffItem>();
         missingItem.Object1.ShouldBeEqualTo(_calculationMethodCache1);
         missingItem.Object2.ShouldBeEqualTo(_calculationMethodCache2);
         missingItem.MissingObject1.ShouldBeEqualTo(_calculationMethodCat3Item5);
         missingItem.MissingObject2.ShouldBeNull();
      }
   }
}