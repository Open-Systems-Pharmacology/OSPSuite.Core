using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_SensitivityParameterFactory : ContextSpecification<ISensitivityParameterFactory>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      protected IContainerTask _containerTask;
      protected IDimensionFactory _dimensionFactory;
      protected IFullPathDisplayResolver _fullPathDisplayResolver;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _containerTask = A.Fake<IContainerTask>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _fullPathDisplayResolver= A.Fake<IFullPathDisplayResolver>();
         sut = new SensitivityParameterFactory(_objectBaseFactory, _containerTask, _dimensionFactory,_fullPathDisplayResolver);
      }
   }

   public class When_the_sensitivity_parameter_factory_is_creating_a_sensitivity_parameter_for_a_parameter_selection : concern_for_SensitivityParameterFactory
   {
      private ParameterSelection _parameterSelection;
      private SensitivityAnalysis _sensitivityAnalysis;
      private SensitivityParameter _result;
      private IDimension _fractionDimension;

      protected override void Context()
      {
         base.Context();
         _fractionDimension= A.Fake<IDimension>();
         _parameterSelection = A.Fake<ParameterSelection>();
         _parameterSelection.Parameter.Name = "XX";
         _sensitivityAnalysis = new SensitivityAnalysis();
         A.CallTo(_containerTask).WithReturnType<string>().Returns("YY");
         A.CallTo(() => _objectBaseFactory.Create<ConstantFormula>()).ReturnsLazily(x=>new ConstantFormula());
         A.CallTo(() => _objectBaseFactory.Create<SensitivityParameter>()).Returns(new SensitivityParameter());
         A.CallTo(() => _objectBaseFactory.Create<IParameter>()).ReturnsLazily(x=>new Parameter());
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.FRACTION)).Returns(_fractionDimension);
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_parameterSelection, _sensitivityAnalysis);
      }

      [Observation]
      public void should_return_a_new_sensntivity_parameter_with_the_expected_sub_parameters()
      {
         _result.Name.ShouldBeEqualTo("YY");
         _result.NumberOfStepsParameter.ShouldNotBeNull();
         _result.NumberOfStepsParameter.Dimension.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION);
         _result.NumberOfStepsParameter.Value.ShouldBeEqualTo(2);
         _result.NumberOfStepsValue.ShouldBeEqualTo(2);

         _result.VariationRangeParameter.ShouldNotBeNull();
         _result.VariationRangeParameter.Dimension.ShouldBeEqualTo(_fractionDimension);
         _result.VariationRangeParameter.Value.ShouldBeEqualTo(0.1);
         _result.VariationRangeValue.ShouldBeEqualTo(0.1);
      }
   }
}