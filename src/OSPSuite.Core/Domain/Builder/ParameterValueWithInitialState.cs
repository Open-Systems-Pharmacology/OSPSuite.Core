using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder;

public abstract class ParameterValueWithInitialState : ParameterValue
{
   public string InitialFormulaId { set; get; }

   public double? InitialValue { set; get; }

   public Unit InitialUnit { set; get; }

   public bool HasInitialState => InitialValue.HasValue || !string.IsNullOrEmpty(InitialFormulaId) || InitialUnit != null;

}