using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Services
{
   public interface IFormulaUsageChecker
   {
      /// <summary>
      ///    Returns true if the formula is used in the given object otherwise false
      /// </summary>
      /// <param name="objectBase">Object that should be parsed to see if the formula given as parameter is being used</param>
      /// <param name="formula">Formula to check</param>
      bool FormulaUsedIn(IObjectBase objectBase, IFormula formula);
   }

   public class FormulaUsageChecker : IVisitor<IUsingFormula>, IVisitor<IParameter>, IVisitor<IMoleculeBuilder>, IFormulaUsageChecker
   {
      private IFormula _formula;

      private bool _formulaUsed;

      public bool FormulaUsedIn(IObjectBase objectBase, IFormula formula)
      {
         if (formula == null)
            return false;

         _formula = formula;
         _formulaUsed = false;
         try
         {
            objectBase.AcceptVisitor(this);
            return _formulaUsed;
         }
         finally
         {
            _formula = null;
         }
      }

      public void Visit(IParameter parameter)
      {
         if (_formulaUsed) return;
         _formulaUsed = parameter.UsesFormula(_formula);
      }

      public void Visit(IUsingFormula usingFormula)
      {
         if (_formulaUsed) return;
         _formulaUsed = usingFormula.Formula == _formula;
      }

      public void Visit(IMoleculeBuilder moleculeBuilder)
      {
         if (_formulaUsed) return;
         _formulaUsed = moleculeBuilder.DefaultStartFormula.Equals(_formula);
      }
   }
}