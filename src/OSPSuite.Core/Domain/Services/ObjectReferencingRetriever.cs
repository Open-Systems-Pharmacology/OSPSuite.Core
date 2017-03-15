using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain.Services
{
   public interface IObjectReferencingRetriever
   {
      /// <summary>
      ///    Returns all <see cref="IUsingFormula" /> defined in the <see cref="Model" /> of the given
      ///    <paramref name="simulation" /> referencing the <paramref name="reference" />.
      ///    The reference check is done using object references and not using object path.
      /// </summary>
      /// <param name="reference">object whose usage in formula should be checked</param>
      /// <param name="simulation">simulation</param>
      IReadOnlyCollection<IUsingFormula> AllUsingFormulaReferencing(IFormulaUsable reference, IModelCoreSimulation simulation);

      /// <summary>
      ///    Returns all <see cref="IUsingFormula" /> defined in the <paramref name="model" /> referencing the
      ///    <paramref name="reference" />.
      ///    The reference check is done using object references and not using object path.
      /// </summary>
      /// <param name="reference">object whose usage in formula should be checked</param>
      /// <param name="model">model</param>
      IReadOnlyCollection<IUsingFormula> AllUsingFormulaReferencing(IFormulaUsable reference, IModel model);
   }

   public class ObjectReferencingRetriever : IObjectReferencingRetriever
   {
      public IReadOnlyCollection<IUsingFormula> AllUsingFormulaReferencing(IFormulaUsable reference, IModelCoreSimulation simulation)
      {
         return AllUsingFormulaReferencing(reference, simulation.Model);
      }

      public IReadOnlyCollection<IUsingFormula> AllUsingFormulaReferencing(IFormulaUsable reference, IModel model)
      {
         return model.Root.GetAllChildren<IUsingFormula>(o => isReferencing(o, reference));
      }

      private bool isReferencing(IUsingFormula usingFormula, IFormulaUsable reference)
      {
         var formula = usingFormula.Formula;
         return formulaReferences(formula, reference) || rhsReferences(usingFormula as IParameter, reference);
      }

      private bool rhsReferences(IParameter parameter, IFormulaUsable reference)
      {
         return parameter != null && formulaReferences(parameter.RHSFormula, reference);
      }

      private bool formulaReferences(IFormula formula, IFormulaUsable reference)
      {
         return formula != null && formula.ObjectReferences.Select(x => x.Object).Contains(reference);
      }
   }
}