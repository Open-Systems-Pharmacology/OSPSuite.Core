using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Extensions
{
   public static class ParameterIdentificationRunModeExtensions
   {
      public static bool IsRandomizedStartValue(this ParameterIdentificationRunMode runMode)
      {
         return runMode.IsAnImplementationOf<MultipleParameterIdentificationRunMode>();
      }

      public static bool IsCategorial(this ParameterIdentificationRunMode runMode)
      {
         return runMode.IsAnImplementationOf<CategorialParameterIdentificationRunMode>();
      }

      public static bool IsStandard(this ParameterIdentificationRunMode runMode)
      {
         return runMode.IsAnImplementationOf<StandardParameterIdentificationRunMode>();
      }
   }
}
