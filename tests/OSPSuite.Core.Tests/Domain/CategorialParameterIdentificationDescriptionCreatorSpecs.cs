using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CategorialParameterIdentificationDescriptionCreator : ContextSpecification<CategorialParameterIdentificationDescriptionCreator>
   {
      private IDisplayNameProvider _displayNameProvider;
      protected CalculationMethodCombination _aCombination;
      protected List<CalculationMethodCombination> _calculationMethodCombinations;
      protected List<CalculationMethodWithCompoundName> _calculationMethodWithCompoundNames;
      protected CategorialParameterIdentificationRunMode _runMode;

      protected override void Context()
      {

         _runMode= new CategorialParameterIdentificationRunMode();
         _calculationMethodWithCompoundNames = new List<CalculationMethodWithCompoundName>
         {
            new CalculationMethodWithCompoundName(new CalculationMethod { Category = "category1", DisplayName = "method1" }, "compound1"),
            new CalculationMethodWithCompoundName(new CalculationMethod { Category = "category2", DisplayName = "method2" }, "compound1")
         };
         _aCombination = new CalculationMethodCombination(_calculationMethodWithCompoundNames);

         _displayNameProvider = A.Fake<IDisplayNameProvider>();
         sut = new CategorialParameterIdentificationDescriptionCreator(_displayNameProvider);
         A.CallTo(() => _displayNameProvider.DisplayNameFor(A<Category<CalculationMethod>>._)).ReturnsLazily((object category) => category.DowncastTo<Category<CalculationMethod>>().Name);
      }

      protected static bool DescriptionDoesNotContain(string description, string stringValue)
      {
         return description.IndexOf(stringValue, StringComparison.Ordinal) == -1;
      }

      protected static bool DescriptionContains(string description, string stringValue)
      {
         return description.IndexOf(stringValue, StringComparison.Ordinal) != -1;
      }
   }

   public class When_creating_descriptions_for_parameter_identifications_with_multiple_compound_and_multiple_category : concern_for_CategorialParameterIdentificationDescriptionCreator
   {
      private string _result;

      protected override void Context()
      {
         base.Context();
         _calculationMethodWithCompoundNames.Add(new CalculationMethodWithCompoundName(new CalculationMethod { Category = "category2" }, "compound2"));
      }

      protected override void Because()
      {
         _result = sut.CreateDescriptionFor(_aCombination, _runMode, isSingleCategory: false);
      }

      [Observation]
      public void the_description_should_include_compound_category_and_method()
      {
         DescriptionContains(_result, "compound1").ShouldBeTrue();
         DescriptionContains(_result, "compound2").ShouldBeTrue();
         DescriptionContains(_result, "category1").ShouldBeTrue();
         DescriptionContains(_result, "category2").ShouldBeTrue();
         DescriptionContains(_result, "method").ShouldBeTrue();
      }
   }

   public class When_creating_descriptions_for_parameter_identifications_with_multiple_compound_and_one_category : concern_for_CategorialParameterIdentificationDescriptionCreator
   {
      private string _result;

      protected override void Context()
      {
         base.Context();

         _calculationMethodWithCompoundNames.Add(new CalculationMethodWithCompoundName(new CalculationMethod { Category = "category1" }, "compound2"));  
      }

      protected override void Because()
      {
         _result = sut.CreateDescriptionFor(_aCombination, _runMode, isSingleCategory:true);
      }

      [Observation]
      public void the_description_should_not_include_the_category()
      {
         DescriptionContains(_result, "compound1").ShouldBeTrue();
         DescriptionContains(_result, "compound2").ShouldBeTrue();
         DescriptionDoesNotContain(_result, "category1").ShouldBeTrue();
         DescriptionContains(_result, "method").ShouldBeTrue();
      }
   }

   public class When_creating_descriptions_for_parameter_identifications_with_one_compound_and_one_category : concern_for_CategorialParameterIdentificationDescriptionCreator
   {
      private string _result;

      protected override void Context()
      {
         base.Context();
         _aCombination.CalculationMethods.ElementAt(1).CalculationMethod.Category = "category1";
      }

      protected override void Because()
      {
         _result = sut.CreateDescriptionFor(_aCombination, _runMode, isSingleCategory:true);
      }

      [Observation]
      public void the_description_should_not_include_the_name_of_the_compound_or_the_category()
      {
         DescriptionDoesNotContain(_result, "compound1").ShouldBeTrue();
         DescriptionDoesNotContain(_result, "compound2").ShouldBeTrue();
         DescriptionDoesNotContain(_result, "category1").ShouldBeTrue();
         DescriptionDoesNotContain(_result, "category2").ShouldBeTrue();
         DescriptionContains(_result, "method").ShouldBeTrue();
      }
   }

   public class When_creating_descriptions_for_parameter_identifications_with_all_the_same : concern_for_CategorialParameterIdentificationDescriptionCreator
   {
      private string _result;

      protected override void Context()
      {
         base.Context();
         _runMode.AllTheSame = true;
      }

      protected override void Because()
      {
         _result = sut.CreateDescriptionFor(_aCombination, _runMode, isSingleCategory:false);
      }

      [Observation]
      public void the_description_should_not_include_the_name_of_the_compound()
      {
         DescriptionDoesNotContain(_result, "compound1").ShouldBeTrue();
         DescriptionContains(_result, "category1").ShouldBeTrue();
         DescriptionContains(_result, "category2").ShouldBeTrue();
         DescriptionContains(_result, "method").ShouldBeTrue();
      }
   }

   public class When_creating_descriptions_for_parameter_identification_with_one_compound : concern_for_CategorialParameterIdentificationDescriptionCreator
   {
      private string _result;

      protected override void Because()
      {
         _result = sut.CreateDescriptionFor(_aCombination, _runMode, isSingleCategory:false);
      }

      [Observation]
      public void the_description_should_not_include_the_name_of_the_compound()
      {
         DescriptionDoesNotContain(_result, "compound1").ShouldBeTrue();
         DescriptionDoesNotContain(_result, "compound2").ShouldBeTrue();
         DescriptionContains(_result, "category1").ShouldBeTrue();
         DescriptionContains(_result, "category2").ShouldBeTrue();
         DescriptionContains(_result, "method").ShouldBeTrue();
      }
   }
}
