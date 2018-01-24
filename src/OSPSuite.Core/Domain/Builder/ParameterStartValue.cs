using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IParameterStartValue : IStartValue, IWithDefaultState
   {
      string ParameterName { get; }

      /// <summary>
      ///    Tests whether or not the value is public-member-equivalent to the target
      /// </summary>
      /// <param name="parameterStartValue">The comparable object</param>
      /// <returns>True if all the public members are equal, otherwise false</returns>
      bool IsEquivalentTo(IParameterStartValue parameterStartValue);

      /// <summary>
      ///    This option is only used at the moment in PKSim when cloning a simulation. The goal is to not replace the formula of
      ///    a parameter
      ///    changed in the simulation with constant. This parameter is temporary is should not be serialized or updated when
      ///    cloning etc.
      ///    Default value is <c>true</c>
      /// </summary>
      bool OverrideFormulaWithValue { get; set; }
   }

   public class ParameterStartValue : StartValueBase, IParameterStartValue
   {
      public string ParameterName => Name;
      public bool OverrideFormulaWithValue { get; set; } = true;
      public bool IsDefault { get; set; }

      public bool IsEquivalentTo(IParameterStartValue parameterStartValue)
      {
         var isBaseEquivalent = base.IsEquivalentTo(parameterStartValue);
         var isEquivalent = NullableEqualsCheck(ParameterName, parameterStartValue.ParameterName);

         return isBaseEquivalent && isEquivalent;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceParameterStartValue = source as IParameterStartValue;
         if (sourceParameterStartValue == null) return;
         IsDefault = sourceParameterStartValue.IsDefault;
      }
   }
}