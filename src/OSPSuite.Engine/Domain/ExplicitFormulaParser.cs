using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.FuncParser;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Engine.Domain
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
      private readonly IParsedFunction _parserFunc;

      public ExplicitFormulaParser(IEnumerable<string> variableNames, IEnumerable<string> parameterNames)
         : this(variableNames, parameterNames, new ParsedFunction())
      {
      }

      public ExplicitFormulaParser(IEnumerable<string> variableNames, IEnumerable<string> parameterNames, IParsedFunction parserFunction)
      {
         _parserFunc = parserFunction;
         _parserFunc.VariableNames = variableNames.ToList();
         _parserFunc.ParameterNames = parameterNames.ToList();
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
            var parserError = new FuncParserErrorData();
            _parserFunc.ParameterValues = parameterValues;
            double value = _parserFunc.CalcExpression(variableValues, parserError);
            if (parserError.ErrorNumber != 0)
               throw new FuncParserException(parserError);

            return value;
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
            var parserError = new FuncParserErrorData();
            _parserFunc.Parse(parserError);
            if (parserError.ErrorNumber != 0)
               throw new FuncParserException(parserError);
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
      public FuncParserException(IFuncParserErrorData parserError)
         : base(parserError.Description)
      {
      }

      public FuncParserException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}