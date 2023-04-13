using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Builder
{
   public static class ProcessBuilderExtensions
   {
      public static TProcessBuilder WithKinetic<TProcessBuilder>(this TProcessBuilder processBuilder, IFormula formula) where TProcessBuilder : ProcessBuilder
      {
         processBuilder.Formula = formula;
         return processBuilder;
      }
   }
}