using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation.Presenters
{
   /// <summary>
   /// Represents a presenter that shows units in columns (typically to change the unit in a whole column at once)
   /// </summary>
   public interface IUnitsInColumnPresenter<T> : IPresenter
   {
      void ChangeUnit(T colummnIdentifier, Unit newUnit);
      Unit DisplayUnitFor(T colummnIdentifier);
      IEnumerable<Unit> AvailableUnitsFor(T columnIndentifier);
   }
}