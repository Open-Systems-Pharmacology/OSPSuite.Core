using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Extensions
{
   public static class ParameterIdentificationExtensions
   {
      public static bool IsRandomizedStartValueRunMode(this ParameterIdentification parameterIdentification)
      {
         return parameterIdentification.Configuration.RunMode.IsRandomizedStartValue();
      }

      public static bool IsCategorialRunMode(this ParameterIdentification parameterIdentification)
      {
         return parameterIdentification.Configuration.RunMode.IsCategorial();
      }

      public static bool IsStandardRunMode(this ParameterIdentification parameterIdentification)
      {
         return parameterIdentification.Configuration.RunMode.IsStandard();
      }
   }
}
