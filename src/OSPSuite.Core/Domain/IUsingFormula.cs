using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
   public interface IUsingFormula : IEntity, IWithDimension
   {
      IFormula Formula { get; set; }
   }
}