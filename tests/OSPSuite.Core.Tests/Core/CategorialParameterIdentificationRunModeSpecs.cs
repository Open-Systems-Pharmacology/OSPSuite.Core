using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core
{
   public abstract class concern_for_CategorialParameterIdentificationRunMode : ContextSpecification<CategorialParameterIdentificationRunMode>
   {
      protected CalculationMethod _calculationMethod1;
      protected CalculationMethod _calculationMethod2;
      protected CalculationMethod _calculationMethod3;

      protected override void Context()
      {
         sut = new CategorialParameterIdentificationRunMode();
         _calculationMethod1 = new CalculationMethod {Name = "C1", Category = "Cat1"};
         _calculationMethod2 = new CalculationMethod {Name = "C2", Category = "Cat1"};
         _calculationMethod3 = new CalculationMethod {Name = "C3", Category = "Cat2"};
      }
   }

   public class When_returning_the_number_of_selected_category_for_an_all_the_same_categorial_parameter_identification_run_mode : concern_for_CategorialParameterIdentificationRunMode
   {
      private IReadOnlyList<string> _selectedCategories;

      protected override void Context()
      {
         base.Context();
         sut.AllTheSame = true;
         sut.AllTheSameSelection.AddCalculationMethod(_calculationMethod1);
         sut.AllTheSameSelection.AddCalculationMethod(_calculationMethod2);
         sut.AllTheSameSelection.AddCalculationMethod(_calculationMethod3);
      }
      protected override void Because()
      {
         _selectedCategories = sut.SelectedCategories;
      }

      [Observation]
      public void should_return_the_expected_categories()
      {
         _selectedCategories.ShouldOnlyContain(_calculationMethod1.Category, _calculationMethod3.Category);
      }
   }

   public class When_returning_the_number_of_selected_category_for_a_compound_speecific_categorial_parameter_identification_run_mode : concern_for_CategorialParameterIdentificationRunMode
   {
      private IReadOnlyList<string> _selectedCategories;

      protected override void Context()
      {
         base.Context();
         sut.AllTheSame = false;
         var cacheComp1 = new CalculationMethodCache();
         cacheComp1.AddCalculationMethod(_calculationMethod1);
         cacheComp1.AddCalculationMethod(_calculationMethod2);

         var cacheComp2 = new CalculationMethodCache();
         cacheComp2.AddCalculationMethod(_calculationMethod3);
         cacheComp2.AddCalculationMethod(_calculationMethod2);

         sut.CalculationMethodsCache["Comp1"] = cacheComp1;
         sut.CalculationMethodsCache["Comp2"] = cacheComp2;
         sut.AllTheSameSelection.AddCalculationMethod(_calculationMethod1);
         sut.AllTheSameSelection.AddCalculationMethod(_calculationMethod2);
         sut.AllTheSameSelection.AddCalculationMethod(_calculationMethod3);
      }
      protected override void Because()
      {
         _selectedCategories = sut.SelectedCategories;
      }

      [Observation]
      public void should_return_the_expected_categories()
      {
         _selectedCategories.ShouldOnlyContain(_calculationMethod1.Category, _calculationMethod3.Category);
      }
   }
}	