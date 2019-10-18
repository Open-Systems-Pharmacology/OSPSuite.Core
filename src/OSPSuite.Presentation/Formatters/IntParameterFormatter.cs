using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class IntParameterFormatter : NumericFormatter<uint>
   {
      private readonly IParameterDTO _parameterDTO;

      public IntParameterFormatter(IParameterDTO parameterDTO)
         : base(NumericFormatterOptions.Instance)
      {
         _parameterDTO = parameterDTO;
      }

      public override string Format(uint valueToFormat)
      {
         return $"{valueToFormat} {_parameterDTO.DisplayUnit}";
      }
   }
}