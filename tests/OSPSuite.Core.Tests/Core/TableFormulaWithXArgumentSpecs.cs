using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core
{
   public abstract class concern_for_TableFormulaWithXArgument : ContextSpecification<TableFormulaWithXArgument>
   {
      protected IDimension _dimensionpH;
      protected IDimension _dimensionLength;
      protected TableFormula _tableFormula;
      protected IParameter _xArgumentObject, _tableObject;
      protected IMoleculeAmount _dependentObject;
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

         _dependentObject = new MoleculeAmount {_tableObject, _xArgumentObject};

         var tableObjectPath = new FormulaUsablePath(_tableObjectAlias).WithAlias(_tableObjectAlias);
         sut.AddTableObjectPath(tableObjectPath);
         var xArgumentObjectPath = new FormulaUsablePath(_xArgumentObjectAlias).WithAlias(_xArgumentObjectAlias);
         sut.AddXArgumentObjectPath(xArgumentObjectPath);
      }

      protected double CalcValue()
      {
         return sut.Calculate(_dependentObject);
      }
   }

   public class When_calculating_values_for_table_with_argument_formula : concern_for_TableFormulaWithXArgument
   {
      [Observation]
      public void should_be_able_to_retrieve_the_given_value_for_an_exact_time()
      {
         _xArgumentObject.Value = 1;
         CalcValue().ShouldBeEqualTo(10);

         _xArgumentObject.Value = 3;
         CalcValue().ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_return_the_first_value_for_an_argument_below_the_first_x_sample()
      {
         _xArgumentObject.Value = 0;
         CalcValue().ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_return_the_largest_value_for_an_argument_above_the_last_time_sample()
      {
         _xArgumentObject.Value = 4;
         CalcValue().ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_retun_the_interpolated_value_if_the_argument_is_not_one_of_the_defined_sample()
      {
         _xArgumentObject.Value = 1.5;
         CalcValue().ShouldBeEqualTo(15);
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
         CalcValue().ShouldBeEqualTo(45);
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
         The.Action(() => CalcValue()).ShouldThrowAn<Exception>();
      }
   }
}