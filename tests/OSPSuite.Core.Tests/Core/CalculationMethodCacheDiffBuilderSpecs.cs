using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_CalculationMethodCacheDiffBuilder : concern_for_ObjectComparer
   {
      protected CalculationMethodCache _calculationMethodCache1;
      protected CalculationMethodCache _calculationMethodCache2;
      protected CalculationMethod _calculationMethodCat1Item1;
      protected CalculationMethod _calculationMethodCat1Item2;
      protected CalculationMethod _calculationMethodCat2Item3;
      protected CalculationMethod _calculationMethodCat2Item4;

      protected override void Context()
      {
         base.Context();

         _calculationMethodCache1 = new CalculationMethodCache();
         _calculationMethodCache2 = new CalculationMethodCache();


         _calculationMethodCat1Item1 = new CalculationMethod {Category = "Cat1", Name = "Item1", DisplayName = "Item1"};
         _calculationMethodCat1Item2 = new CalculationMethod {Category = "Cat1", Name = "Item2", DisplayName = "Item2"};
         _calculationMethodCat2Item3 = new CalculationMethod {Category = "Cat2", Name = "Item3", DisplayName = "Item3"};
         _calculationMethodCat2Item4 = new CalculationMethod {Category = "Cat2", Name = "Item4", DisplayName = "Item4"};

         _calculationMethodCache1.AddCalculationMethod(_calculationMethodCat1Item1);
         _calculationMethodCache1.AddCalculationMethod(_calculationMethodCat2Item3);
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
         _report.Count.ShouldBeEqualTo(2);
         _report[0].Object1.ShouldBeEqualTo(_calculationMethodCat1Item1);
         _report[0].Object2.ShouldBeEqualTo(_calculationMethodCat1Item2);
      }
   }
}