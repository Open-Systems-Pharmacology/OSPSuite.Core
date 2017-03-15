using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Converter.v5_2
{
   internal abstract class concern_for_UsingFormulaConverter : ContextSpecification<IUsingFormulaConverter>
   {
      protected IFormulaMapper _formulaMapper;
      protected IDimensionMapper _dimensionMapper;
      protected IFormula _formula;
      protected IUsingDimensionConverter _usingDimensionConverter;
      protected ExplicitFormula _explicitFormula;

      protected override void Context()
      {
         _formulaMapper = A.Fake<IFormulaMapper>();
         _dimensionMapper = A.Fake<IDimensionMapper>();
         _usingDimensionConverter = A.Fake<IUsingDimensionConverter>();
         var dimension = A.Fake<IDimension>();
         _explicitFormula = new ExplicitFormula("A+B").WithDimension(dimension);
         A.CallTo(() => _formulaMapper.NewFormulaFor(_explicitFormula.FormulaString)).Returns(string.Empty);
         A.CallTo(() => _dimensionMapper.ConversionFactor(_formula)).Returns(1);
         _formula = _explicitFormula;
         sut = new UsingFormulaConverter(_dimensionMapper, _formulaMapper, _usingDimensionConverter);
      }

      protected override void Because()
      {
         sut.Convert(_formula);
      }
   }

   internal class When_converting_a_formula_for_which_a_new_formula_is_available : concern_for_UsingFormulaConverter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _formulaMapper.NewFormulaFor(_explicitFormula.FormulaString)).Returns("C+D");
      }

      [Observation]
      public void should_update_the_formula()
      {
         _explicitFormula.FormulaString.ShouldBeEqualTo("C+D");
      }

      [Observation]
      public void should_convert_the_formula_dimension()
      {
         A.CallTo(() => _usingDimensionConverter.Convert(_formula)).MustHaveHappened();
      }
   }

   internal class When_converting_a_constant_formula_parameter : concern_for_UsingFormulaConverter
   {
      protected override void Context()
      {
         base.Context();
         _formula = new ConstantFormula(5);
         A.CallTo(() => _dimensionMapper.ConversionFactor(_formula)).Returns(10);
      }

      [Observation]
      public void should_convert_the_value_in_the_constant_formula()
      {
         _formula.DowncastTo<ConstantFormula>().Value.ShouldBeEqualTo(5 * 10);
      }
   }

   internal class When_converting_a_formula_for_which_a_no_new_formula_is_available_and_the_conversion_factor_is_1 : concern_for_UsingFormulaConverter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dimensionMapper.ConversionFactor(_formula)).Returns(1);
      }

      [Observation]
      public void should_not_update_the_formula()
      {
         _explicitFormula.FormulaString.ShouldBeEqualTo("A+B");
      }

      [Observation]
      public void should_convert_the_formula_dimension()
      {
         A.CallTo(() => _usingDimensionConverter.Convert(_formula)).MustHaveHappened();
      }
   }


   internal class When_converting_an_object_using_a_formula_without_convert_formula_set : concern_for_UsingFormulaConverter
   {
      private IUsingFormula _usingFormula;

      protected override void Context()
      {
         base.Context();
         _usingFormula = A.Fake<IUsingFormula>();
         _usingFormula.Formula = _formula;
         A.CallTo(() => _dimensionMapper.ConversionFactor(_formula)).Returns(15);
      }

      protected override void Because()
      {
         sut.Convert(_usingFormula, false);
      }

      [Observation]
      public void should_update_the_formula_with_the_conversion_factor()
      {
         _explicitFormula.FormulaString.ShouldBeEqualTo("A+B");
      }

      [Observation]
      public void should_convert_the_formula_dimension()
      {
         A.CallTo(() => _usingDimensionConverter.Convert(_formula)).MustNotHaveHappened();
      }
   }

   internal class When_converting_an_object_using_a_formula_with_a_constant_formula : concern_for_UsingFormulaConverter
   {
      private IUsingFormula _usingFormula;
      private ConstantFormula _constantFormula;

      protected override void Context()
      {
         base.Context();
         _constantFormula = new ConstantFormula(2).WithDimension(A.Fake<IDimension>());
         _formula =_constantFormula;
         _usingFormula = A.Fake<IUsingFormula>();
         _usingFormula.Formula = _formula;
         A.CallTo(() => _dimensionMapper.ConversionFactor(_formula)).Returns(15);
      }

      protected override void Because()
      {
         sut.Convert(_usingFormula, false);
      }

      [Observation]
      public void should_update_the_formula_with_the_conversion_factor()
      {
         _constantFormula.Value.ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_convert_the_formula_dimension()
      {
         A.CallTo(() => _usingDimensionConverter.Convert(_formula)).MustHaveHappened();
      }
   }

   internal class When_converting_an_object_using_a_formula_with_NoDimension_Set : concern_for_UsingFormulaConverter
   {
      private IUsingFormula _usingFormula;

      protected override void Context()
      {
         base.Context();
         _explicitFormula.Dimension =  Constants.Dimension.NO_DIMENSION;
         _usingFormula = A.Fake<IUsingFormula>().WithDimension(A.Fake<IDimension>());
         
         _usingFormula.Formula = _formula;
         A.CallTo(() => _dimensionMapper.ConversionFactor(_formula)).Returns(15);
      }

      protected override void Because()
      {
         sut.Convert(_usingFormula, false);
      }

      [Observation]
      public void should_update_the_formula_dimenson()
      {
         _explicitFormula.Dimension.ShouldBeEqualTo(_usingFormula.Dimension);
      }
   }

   internal class When_converting_a_table_formula_for_which_dimension_conversion_is_required : concern_for_UsingFormulaConverter
   {
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _tableFormula.AddPoint(1, 10);
         _tableFormula.AddPoint(2, 20);
         _formula = _tableFormula;
         A.CallTo(() => _dimensionMapper.ConversionFactor(_formula)).Returns(10);
      }

      [Observation]
      public void should_convert_all_values_in_the_Y_column()
      {
         _tableFormula.ValueAt(1).ShouldBeEqualTo(100);
         _tableFormula.ValueAt(2).ShouldBeEqualTo(200);
      }
   }
}