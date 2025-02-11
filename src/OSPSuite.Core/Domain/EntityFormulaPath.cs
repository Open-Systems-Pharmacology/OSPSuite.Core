using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Simple class to hold an entity (typically an IUsingFormula), the referenced formula and given path entry in the
   ///    formula
   ///    <remarks>
   ///       We are using IEntity instead of IUsingFormula to ensure that the caller is not tempted to do
   ///       usingFormula.Formula. In most cases, this would be right, except when dealing with RHSFormula in parameters.
   ///    </remarks>
   /// </summary>
   public class EntityFormulaPath
   {
      public IEntity Entity { get; }
      public IFormula Formula { get; }
      public FormulaUsablePath Path { get; }

      public EntityFormulaPath(IEntity entity, FormulaUsablePath path, IFormula formula)
      {
         Entity = entity;
         Formula = formula;
         Path = path;
      }

      public void Deconstruct(out IEntity entity, out FormulaUsablePath path, out IFormula formula)
      {
         entity = Entity;
         formula = Formula;
         path = Path;
      }

      public void Deconstruct(out IEntity entity, out FormulaUsablePath path)
      {
         entity = Entity;
         path = Path;
      }
   }
}