using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting.Items;
using OSPSuite.TeXReporting.Builder;

namespace OSPSuite.Infrastructure.Reporting.TeXBuilder
{
   internal class FormulaTeXBuilder : OSPSuiteTeXBuilder<Formula>
   {
      public const string FORMULA = "Formula";

      private readonly ITeXBuilderRepository _builderRepository;

      public FormulaTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(Formula formula, OSPSuiteTracker buildTracker)
      {
         if (string.IsNullOrEmpty(formula.Name))
            return;

         var listToReport = new List<object>
         {
            new FormulaTextBox(FORMULA, formula)
         };
         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}