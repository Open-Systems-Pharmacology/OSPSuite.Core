using OSPSuite.Utility.Format;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Services;

namespace OSPSuite.UI.Extensions
{
   public static class ParameterDTOExtensions
   {
      public static IFormatter<double> ParameterFormatter(this IParameterDTO parameterDTO)
      {
         return new ParameterFormatter(parameterDTO);
      }

      public static IFormatter<uint> IntParameterFormatter(this IParameterDTO parameterDTO)
      {
         return new IntParameterFormatter(parameterDTO);
      }
   }
}