using System;

namespace OSPSuite.Core.Domain.Services
{
   public interface IExplicitFormulaParser
   {
      string FormulaString { set; get; }
      double Compute(double[] variableValues, double[] parameterValues);
      void Parse();
   }

   public class NullExplicitFormulaParser : IExplicitFormulaParser
   {
      public string FormulaString { get; set; }
      public double Compute(double[] variableValues, double[] parameterValues) => double.NaN;

      public void Parse()
      {
         throw new NotImplementedException();
      }
   }
}