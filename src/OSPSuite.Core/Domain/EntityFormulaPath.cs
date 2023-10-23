using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
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