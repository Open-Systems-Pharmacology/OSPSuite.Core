using OSPSuite.Presentation.Formatters;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.DTO
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