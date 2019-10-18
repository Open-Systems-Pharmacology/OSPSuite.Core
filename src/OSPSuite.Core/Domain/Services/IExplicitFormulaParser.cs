namespace OSPSuite.Core.Domain.Services
{
   public interface IExplicitFormulaParser
   {
      string FormulaString { set; get; }
      double Compute(double[] variableValues, double[] parameterValues);
      void Parse();
   }
}