using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder;

public abstract class ParameterValueWithInitialState : ParameterValue
{
   public string InitialFormulaId { set; get; }

   public double? InitialValue { set; get; }

   public Unit InitialUnit { set; get; }

   public bool HasInitialState => InitialValue.HasValue || !string.IsNullOrEmpty(InitialFormulaId) || InitialUnit != null;

   public override void UpdatePropertiesFrom(ParameterValue parameterValue)
   {
      base.UpdatePropertiesFrom(parameterValue);

      if (parameterValue is not ParameterValueWithInitialState parameterWithInitialState)
         return;

      InitialValue = parameterWithInitialState.InitialValue;
      InitialFormulaId = parameterWithInitialState.InitialFormulaId;
      InitialUnit = parameterWithInitialState.InitialUnit;
   }
}