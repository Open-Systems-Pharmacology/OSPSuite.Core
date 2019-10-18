using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Infrastructure.Reporting.Items
{
   public class FormulaTextBox
   {
      public string Caption { get; }

      public IFormula Formula { get; }

      public FormulaTextBox(string caption, IFormula formula)
      {
         Caption = caption;
         Formula = formula;
      }
   }
}