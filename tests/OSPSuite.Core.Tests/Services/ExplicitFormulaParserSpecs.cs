using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.FuncParser;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ExplicitFormulaParser : ContextSpecification<IExplicitFormulaParser>
   {
      protected ParsedFunction _parsedFunction;
      private IEnumerable<string> _variableNames;
      private IEnumerable<string> _parameterNames;
      protected double[] _variableValues;
      protected double[] _parameterValues;
      private string _var1 = "var1";
      private string _var2 = "var2";
      private string _par1 = "par1";
      private string _par2 = "par2";

      protected override void Context()
      {
         _parsedFunction = new ParsedFunction();
         _variableNames = new List<string> {_var1, _var2};
         _parameterNames = new List<string> {_par1, _par2};
         _variableValues = new[] {1.0, 2.0};
         _parameterValues = new[] {3.0, 4.0};
         sut = new ExplicitFormulaParser(_variableNames, _parameterNames, _parsedFunction);
      }
   }

   
   public class When_setting_the_formula_string_to_the_formula_parser : concern_for_ExplicitFormulaParser
   {
      private string _formulaString = "tralala";

      protected override void Because()
      {
         sut.FormulaString = _formulaString;
      }

      [Observation]
      public void should_initialize_the_parse_function_with_the_formula()
      {
         _parsedFunction.StringToParse.ShouldBeEqualTo(_formulaString);
      }
   }

   
   public class When_getting_the_formula_string : concern_for_ExplicitFormulaParser
   {
      private string _formulaString = "tralala";

      protected override void Context()
      {
         base.Context();
         _parsedFunction.StringToParse = _formulaString;
      }

      [Observation]
      public void should_return_the_string_to_parser()
      {
         sut.FormulaString.ShouldBeEqualTo(_formulaString);
      }
   }

   
   public class When_computing_a_valid_formula : concern_for_ExplicitFormulaParser
   {
      private double _result;

      protected override void Context()
      {
         base.Context();
         _parsedFunction.StringToParse = "var1*par1 + var2*par2";
      }

      protected override void Because()
      {
         _result = sut.Compute(_variableValues, _parameterValues);
      }

      [Observation]
      public void should_return_the_value_calculated_by_the_formula_parser()
      {
         _result.ShouldBeEqualTo(_variableValues[0]*_parameterValues[0] + _variableValues[1]*_parameterValues[1]);
      }
   }

   
   public class When_computing_an_invalid_formula : concern_for_ExplicitFormulaParser
   {
      protected override void Context()
      {
         base.Context();
         _parsedFunction.StringToParse = "var1par1 + var2*par2";
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Compute(_variableValues, _parameterValues)).ShouldThrowAn<FuncParserException>();
      }
   }

   
   public class When_computing_an_valid_formula_but_with_the_wrong_number_of_arguments : concern_for_ExplicitFormulaParser
   {
      protected override void Context()
      {
         base.Context();
         _parsedFunction.StringToParse = "var1par1 + var2*par2";
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Compute(_variableValues, new[]{1.0})).ShouldThrowAn<FuncParserException>();
      }
   }

   
   public class When_parsing_a_valid_formula : concern_for_ExplicitFormulaParser
   {
      protected override void Context()
      {
         base.Context();
         _parsedFunction.StringToParse = "var1*par1 + var2*par2";
      }

      [Observation]
      public void should_not_throw_an_exception()
      {
         sut.Parse();
      }
   }
   
   public class When_parsing_an_invalid_formula : concern_for_ExplicitFormulaParser
   {
      protected override void Context()
      {
         base.Context();
         _parsedFunction.StringToParse = "var1ar1 + var2*par2";
      }

      [Observation]
      public void should_throw_a_func_parser_exception()
      {
         The.Action(() => sut.Parse()).ShouldThrowAn<FuncParserException>();
      }
   }

   public class When_parsing_a_simple_expression_with_exponent : concern_for_ExplicitFormulaParser
   {
      protected override void Context()
      {
         base.Context();
         _parsedFunction.StringToParse = "1e-3";
      }

      [Observation]
      public void should_be_able_to_parse_the_expression()
      {
         sut.Parse();
      }
   }
}  