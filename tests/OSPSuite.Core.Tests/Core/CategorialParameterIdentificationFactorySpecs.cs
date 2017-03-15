using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace OSPSuite.Core
{
   public abstract class concern_for_CategorialParameterIdentificationCreator : ContextSpecification<CategorialParameterIdentificationRunFactory>
   {
      private ICategorialParameterIdentificationToCalculationMethodPermutationMapper _mapper;
      protected ParameterIdentification _parameterIdentification;
      private List<CalculationMethodCombination> _calculationMethodCombinations;
      protected List<CalculationMethodWithCompoundName> _aCombination;
      protected List<CalculationMethodWithCompoundName> _anotherCombination;
      protected IParameterIdentificationRunInitializerFactory _runInitializerFactory;

      protected override void Context()
      {
         _mapper = A.Fake<ICategorialParameterIdentificationToCalculationMethodPermutationMapper>();
         _runInitializerFactory = A.Fake<IParameterIdentificationRunInitializerFactory>();
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.Configuration.RunMode = new CategorialParameterIdentificationRunMode();
         _aCombination = new List<CalculationMethodWithCompoundName>();
         _anotherCombination = new List<CalculationMethodWithCompoundName>();

         _calculationMethodCombinations = new List<CalculationMethodCombination> { new CalculationMethodCombination(_aCombination), new CalculationMethodCombination(_anotherCombination) };

         _aCombination.Add(new CalculationMethodWithCompoundName(new CalculationMethod { Category = "category1", DisplayName = "method1"}, "compound1"));
         _aCombination.Add(new CalculationMethodWithCompoundName(new CalculationMethod { Category = "category2", DisplayName = "method2" }, "compound1"));

         _anotherCombination.Add(new CalculationMethodWithCompoundName(new CalculationMethod { Category = "category1", DisplayName = "method3" }, "compound1"));
         _anotherCombination.Add(new CalculationMethodWithCompoundName(new CalculationMethod { Category = "category2", DisplayName = "method4" }, "compound1"));

         sut = new CategorialParameterIdentificationRunFactory(_runInitializerFactory, _mapper);

   
         A.CallTo(() => _mapper.MapFrom(A<ParameterIdentification>._)).Returns(_calculationMethodCombinations);
      }
   }

   public class When_creating_new_identifications : concern_for_CategorialParameterIdentificationCreator
   {
      private IReadOnlyList<IParameterIdentificationRun> _result;

   
      protected override void Because()
      {
         _result = sut.CreateFor(_parameterIdentification, new CancellationToken());
      }

      [Observation]
      public void should_return_only_one_entry_being_a_clone_of_the_parameter_identification()
      {
         _result.Count().ShouldBeEqualTo(2);
      }

   }
}
