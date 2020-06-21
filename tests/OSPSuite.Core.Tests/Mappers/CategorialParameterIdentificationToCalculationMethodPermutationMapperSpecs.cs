using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_CategorialParameterIdentificationToCalculationMethodPermutationMapper : ContextSpecification<CategorialParameterIdentificationToCalculationMethodPermutationMapper>
   {
      protected ParameterIdentification _parameterIdentification;
      protected CalculationMethodCache _cache1;
      protected CalculationMethodCache _cache2;
      protected CategorialParameterIdentificationRunMode _runMode;

      protected override void Context()
      {
         sut = new CategorialParameterIdentificationToCalculationMethodPermutationMapper();
         _parameterIdentification = new ParameterIdentification();
         _runMode = new CategorialParameterIdentificationRunMode();
         _parameterIdentification.Configuration.RunMode = _runMode;
      }
   }

   public class When_mapping_unique_combinations_from_a_parameter_identification_when_methods_should_be_all_the_same : concern_for_CategorialParameterIdentificationToCalculationMethodPermutationMapper
   {
      private ISimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _runMode.AllTheSame = true;
         _cache1 = _runMode.CalculationMethodCacheFor("compound1");

         _cache1.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method1" });
         _cache1.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method2" });

         _cache1.AddCalculationMethod(new CalculationMethod { Category = "Category2", Name = "method3" });
         _cache1.AddCalculationMethod(new CalculationMethod { Category = "Category2", Name = "method4" });

         _simulation = A.Fake<ISimulation>();
         A.CallTo(() => _simulation.CompoundNames).Returns(new[] { "compound1", "compound2" });

         _parameterIdentification.AddSimulation(_simulation);
      }

      [Observation]
      public void should_map_one_combination_for_all_permutations_of_compounds_and_categories()
      {
         // should be number of categories x number of methods (2 categories x 2 methods) and exclude the number of compounds
         sut.MapFrom(_parameterIdentification).Count().ShouldBeEqualTo(4);
      }
   }

   public class When_mapping_and_the_simulation_does_not_have_all_compounds : concern_for_CategorialParameterIdentificationToCalculationMethodPermutationMapper
   {
      private ISimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _cache1 = _runMode.CalculationMethodCacheFor("compound1");
         _cache2 = _runMode.CalculationMethodCacheFor("compound2");
         _cache1.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method1" });
         _cache1.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method2" });

         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method1" });
         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method2" });

         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category2", Name = "method3" });
         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category2", Name = "method4" });
         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category2", Name = "method5" });
         _simulation = A.Fake<ISimulation>();
         _simulation = A.Fake<ISimulation>();
         A.CallTo(() => _simulation.CompoundNames).Returns(new[] {"compound2" });

         _parameterIdentification.AddSimulation(_simulation);
      }

      [Observation]
      public void should_map_only_for_compounds_present_in_the_identification()
      {
         sut.MapFrom(_parameterIdentification).Count().ShouldBeEqualTo(6);
      }
   }

   public class When_mapping_unique_combinations_from_a_parameter_identification : concern_for_CategorialParameterIdentificationToCalculationMethodPermutationMapper
   {
      private ISimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _cache1 = _runMode.CalculationMethodCacheFor("compound1");
         _cache2 = _runMode.CalculationMethodCacheFor("compound2");
         _cache1.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method1" });
         _cache1.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method2" });

         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method1" });
         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category1", Name = "method2" });

         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category2", Name = "method3" });
         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category2", Name = "method4" });
         _cache2.AddCalculationMethod(new CalculationMethod { Category = "Category2", Name = "method5" });
         _simulation = A.Fake<ISimulation>();
         _simulation = A.Fake<ISimulation>();
         A.CallTo(() => _simulation.CompoundNames).Returns(new[] {"compound1", "compound2"});

         _parameterIdentification.AddSimulation(_simulation);
      }

      [Observation]
      public void should_map_one_combination_for_all_permutations_of_compounds_and_categories()
      {
         sut.MapFrom(_parameterIdentification).Count().ShouldBeEqualTo(12);
      }
   }
}
