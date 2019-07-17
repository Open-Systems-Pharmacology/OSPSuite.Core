using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Builder
{
   public static class ProcessBuilderExtensions
   {
      public static TProcessBuilder WithKinetic<TProcessBuilder>(this TProcessBuilder processBuilder, IFormula formula) where TProcessBuilder : IProcessBuilder
      {
         processBuilder.Formula = formula;
         return processBuilder;
      }
   }
}