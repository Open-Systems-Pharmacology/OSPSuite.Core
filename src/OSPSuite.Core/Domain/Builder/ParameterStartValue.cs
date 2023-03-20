using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class ParameterStartValue : StartValueBase, IWithDefaultState
   {
      public string ParameterName => Name;
      public bool IsDefault { get; set; }

      public bool IsEquivalentTo(ParameterStartValue parameterStartValue)
      {
         var isBaseEquivalent = base.IsEquivalentTo(parameterStartValue);
         var isEquivalent = NullableEqualsCheck(ParameterName, parameterStartValue.ParameterName);
         return isBaseEquivalent && isEquivalent;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceParameterStartValue = source as ParameterStartValue;
         if (sourceParameterStartValue == null) return;
         IsDefault = sourceParameterStartValue.IsDefault;
      }
   }
}