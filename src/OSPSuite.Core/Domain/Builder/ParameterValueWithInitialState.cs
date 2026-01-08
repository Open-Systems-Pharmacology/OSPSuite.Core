using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder;

public abstract class ParameterValueWithInitialState : ParameterValue
{
   public string InitialFormulaId { get; set; }

   public double? InitialValue { get; set; }

   public Unit InitialUnit { get; set; }

   public bool HasInitialState => InitialValue.HasValue || !string.IsNullOrEmpty(InitialFormulaId) || InitialUnit != null;

   public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
   {
      base.UpdatePropertiesFrom(source, cloneManager);

      if (source is not ParameterValueWithInitialState parameterWithInitialState)
         return;

      InitialValue = parameterWithInitialState.InitialValue;
      InitialFormulaId = parameterWithInitialState.InitialFormulaId;
      InitialUnit = parameterWithInitialState.InitialUnit;
   }
}