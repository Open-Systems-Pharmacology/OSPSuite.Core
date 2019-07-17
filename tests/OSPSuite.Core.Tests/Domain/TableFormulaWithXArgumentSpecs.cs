using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_TableFormulaWithXArgument : ContextSpecification<TableFormulaWithXArgument>
   {
      protected IDimension _dimensionpH;
      protected IDimension _dimensionLength;
      protected TableFormula _tableFormula;
      protected IParameter _xArgumentObject, _tableObject;
      protected IMoleculeAmount _dependentObject;
      private Container _container;
      protected IParameter _parameter;
      protected const string _tableObjectAlias = "T1";
      protected const string _xArgumentObjectAlias = "P1";

      protected override void Context()
      {
         sut = new TableFormulaWithXArgument();

         _dimensionLength = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Length", "m");
         _dimensionLength.AddUnit("cm", 0.01, 0);
         _dimensionpH = new Dimension(new BaseDimensionRepresentation(), "pH", "");

         _tableFormula = new TableFormula().WithName(_tableObjectAlias);
         _tableFormula.AddPoint(1, 10);
         _tableFormula.AddPoint(2, 20);
         _tableFormula.AddPoint(3, 30);

         _tableObject = new Parameter().WithName(_tableObjectAlias);
         _tableObject.Formula = _tableFormula;
         _xArgumentObject = new Parameter().WithName(_xArgumentObjectAlias).WithValue(5);

         _parameter = new Parameter().WithFormula(sut);

         _container = new Container {_tableObject, _xArgumentObject, _parameter};

         var tableObjectPath = new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, _tableObjectAlias).WithAlias(_tableObjectAlias);
         sut.AddTableObjectPath(tableObjectPath);
         var xArgumentObjectPath = new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, _xArgumentObjectAlias).WithAlias(_xArgumentObjectAlias);
         sut.AddXArgumentObjectPath(xArgumentObjectPath);
      }
   }

   public class When_calculating_values_for_table_with_argument_formula : concern_for_TableFormulaWithXArgument
   {
      [Observation]
      public void should_be_able_to_retrieve_the_given_value_for_an_exact_time()
      {
         _xArgumentObject.Value = 1;
         _parameter.Value.ShouldBeEqualTo(10);

         _xArgumentObject.Value = 3;
         _parameter.Value.ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_return_the_first_value_for_an_argument_below_the_first_x_sample()
      {
         _xArgumentObject.Value = 0;
         _parameter.Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_return_the_largest_value_for_an_argument_above_the_last_time_sample()
      {
         _xArgumentObject.Value = 4;
         _parameter.Value.ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_retun_the_interpolated_value_if_the_argument_is_not_one_of_the_defined_sample()
      {
         _xArgumentObject.Value = 1.5;
         _parameter.Value.ShouldBeEqualTo(15);
      }
   }

   public class When_calculating_values_for_a_table_formula_object_that_is_not_a_table_formula : concern_for_TableFormulaWithXArgument
   {
      protected override void Context()
      {
         base.Context();
         _tableObject.Formula = new ConstantFormula(45);
      }

      [Observation]
      public void should_return_the_value_of_the_formula_instead()
      {
         _parameter.Value.ShouldBeEqualTo(45);
      }
   }

   public class When_calculating_values_for_a_table_formula_object_that_is_not_a_found_by_alias : concern_for_TableFormulaWithXArgument
   {
      protected override void Context()
      {
         base.Context();
         sut.TableObjectAlias = "XXX";
      }

      [Observation]
      public void should_throw_an_exception_containing_the_alias()
      {
         The.Action(() =>
         {
            var v = _parameter.Value;
         }).ShouldThrowAn<Exception>();
      }
   }

   public class When_a_parameter_is_usung_a_table_formula_with_x_argument_and_resolved_object_and_the_x_argument_changes : concern_for_TableFormulaWithXArgument
   {
      protected override void Context()
      {
         base.Context();
         sut.ResolveObjectPathsFor(_parameter);

         _xArgumentObject.Value = 1;
         _parameter.Value.ShouldBeEqualTo(10);
      }

      protected override void Because()
      {
         _xArgumentObject.Value = 3;
      }

      [Observation]
      public void should_have_updated_the_value()
      {
         _parameter.Value.ShouldBeEqualTo(30);
      }
   }

   public class When_a_parameter_is_usung_a_table_formula_with_x_argument_and_resolved_object_and_the_table_formula_changes : concern_for_TableFormulaWithXArgument
   {
      private ICloneManager _cloneManager;
      private TableFormula _anotherTableFormula;

      protected override void Context()
      {
         base.Context();
         _cloneManager= A.Fake<ICloneManager>();
         sut.ResolveObjectPathsFor(_parameter);

         _xArgumentObject.Value = 1;
         _parameter.Value.ShouldBeEqualTo(10);

         _anotherTableFormula = new TableFormula();
         _anotherTableFormula.AddPoint(1, 100);
         _anotherTableFormula.AddPoint(2, 200);
         _anotherTableFormula.AddPoint(3, 300);

      }

      protected override void Because()
      {
         _tableFormula.UpdatePropertiesFrom(_anotherTableFormula, _cloneManager);
      }

      [Observation]
      public void should_have_updated_the_value()
      {
         _parameter.Value.ShouldBeEqualTo(100);
      }
   }

}