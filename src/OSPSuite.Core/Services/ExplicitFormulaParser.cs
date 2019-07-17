using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.FuncParser;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Services
{
   /// <summary>
   /// </summary>
   /// <remarks>
   ///    Errors in formula will only shown if parse or compute is called.
   ///    Not if any property(Variables, FormulaString) is changed.
   ///    Therefore it would be useful, to call parse on every change.
   /// </remarks>
   public class ExplicitFormulaParser : IExplicitFormulaParser
   {
      private readonly ParsedFunction _parserFunc;

      public ExplicitFormulaParser(IEnumerable<string> variableNames, IEnumerable<string> parameterNames)
         : this(variableNames, parameterNames, new ParsedFunction())
      {
      }

      public ExplicitFormulaParser(IEnumerable<string> variableNames, IEnumerable<string> parameterNames, ParsedFunction parserFunction)
      {
         _parserFunc = parserFunction;
         _parserFunc.SetVariableNames(variableNames.ToList());
         _parserFunc.SetParameterNames(parameterNames.ToList());
         _parserFunc.LogicalNumericMixAllowed = true;
         _parserFunc.LogicOperatorsAllowed = true;
         _parserFunc.SimplifyParametersAllowed = true;
      }

      public string FormulaString
      {
         set => _parserFunc.StringToParse = value;
         get => _parserFunc.StringToParse;
      }

      public double Compute(double[] variableValues, double[] parameterValues)
      {
         try
         {
            _parserFunc.SetParameterValues(parameterValues);
            return _parserFunc.CalcExpression(variableValues);
         }
         catch (FuncParserException)
         {
            throw;
         }
         catch (Exception e)
         {
            throw new FuncParserException("Unable to compute expression", e);
         }
      }

      public void Parse()
      {
         try
         {
            _parserFunc.Parse();
         }
         catch (FuncParserException)
         {
            throw;
         }
         catch (Exception e)
         {
            throw new FuncParserException("Unable to parse expression", e);
         }
      }
   }

   public class FuncParserException : OSPSuiteException
   {
      public FuncParserException()
      {
      }

      public FuncParserException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}