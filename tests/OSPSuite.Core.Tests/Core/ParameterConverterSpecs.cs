using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterConverter : ContextSpecification<IParameterConverter>
   {
      protected IDimensionMapper _dimensionMapper;
      protected IParameter _parameter;
      protected IDimension _dimension;
      protected IUsingFormulaConverter _usingFormulaConverter;
      protected const double _conversionFactor = 5;

      protected override void Context()
      {
         _dimensionMapper = A.Fake<IDimensionMapper>();
         _dimension = A.Fake<IDimension>();
         _usingFormulaConverter = A.Fake<IUsingFormulaConverter>();
         _parameter = new Parameter().WithDimension(_dimension);
         sut = new ParameterConverter(_dimensionMapper, _usingFormulaConverter);
         A.CallTo(() => _dimensionMapper.ConversionFactor(_parameter)).Returns(_conversionFactor);
      }

      protected override void Because()
      {
         sut.Convert(_parameter, true);
      }
   }

   public class When_converting_a_constant_formula_parameter : concern_for_ParameterConverter
   {
      private ConstantFormula _constantFormula;

      protected override void Context()
      {
         base.Context();
         _constantFormula = new ConstantFormula(5);
         _parameter.Formula = _constantFormula;
      }

      [Observation]
      public void should_not_convert_the_value_of_the_parameter()
      {
         _parameter.IsFixedValue.ShouldBeFalse();
      }

      [Observation]
      public void should_convert_the_formula_dimension()
      {
         A.CallTo(() => _usingFormulaConverter.Convert(_parameter, true)).MustHaveHappened();
      }
   }

   public class When_converting_a_constant_formula_parameter_that_is_fixed : concern_for_ParameterConverter
   {
      private ConstantFormula _constantFormula;

      protected override void Context()
      {
         base.Context();
         _constantFormula = new ConstantFormula(5);
         _parameter.Formula = _constantFormula;
         _parameter.Value = 10;
      }

      [Observation]
      public void should_convert_the_value_of_the_parameter()
      {
         _parameter.Value.ShouldBeEqualTo(10 * _conversionFactor);
      }

      [Observation]
      public void should_convert_the_formula_dimension()
      {
         A.CallTo(() => _usingFormulaConverter.Convert(_parameter, true)).MustHaveHappened();
      }
   }

   public class When_converting_a_parameter_with_a_formula_string : concern_for_ParameterConverter
   {
      private ExplicitFormula _explicitFormula;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula("4+3");
         _parameter.Formula = _explicitFormula;
      }

      [Observation]
      public void should_have_not_converted_the_parametrer()
      {
         _parameter.Value.ShouldBeEqualTo(7);
      }

      [Observation]
      public void should_convert_the_formula_dimension()
      {
         A.CallTo(() => _usingFormulaConverter.Convert(_parameter, true)).MustHaveHappened();
      }
   }

   public class When_converting_a_fixed_parameter_with_a_formula_string : concern_for_ParameterConverter
   {
      private ExplicitFormula _explicitFormula;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula("4+3");
         _parameter.Formula = _explicitFormula;
         _parameter.Value = 10;
      }

      [Observation]
      public void should_have_converted_the_parameter()
      {
         _parameter.Value.ShouldBeEqualTo(10 * _conversionFactor);
      }

      [Observation]
      public void should_have_converted_the_formula()
      {
         A.CallTo(() => _usingFormulaConverter.Convert(_parameter, true)).MustHaveHappened();
      }
   }

   public class When_converting_a_distributed_parameter : concern_for_ParameterConverter
   {
      protected override void Context()
      {
         base.Context();
         _parameter = new DistributedParameter().WithDimension(_dimension);
      }

      [Observation]
      public void should_convert_the_formula_dimension()
      {
         A.CallTo(() => _usingFormulaConverter.Convert(_parameter.Formula)).MustHaveHappened();
      }
   }
}