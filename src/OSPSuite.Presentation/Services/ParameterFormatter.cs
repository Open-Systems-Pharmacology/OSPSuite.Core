using OSPSuite.Assets;
using OSPSuite.Utility.Format;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Services
{
   public class ParameterFormatter : NumericFormatter<double>
   {
      private readonly IParameterDTO _parameterDTO;

      public ParameterFormatter(IParameterDTO parameterDTO) : base(NumericFormatterOptions.Instance)
      {
         _parameterDTO = parameterDTO;
      }

      public override string Format(double valueToFormat)
      {
         if (double.IsNaN(valueToFormat))
         {
            if (_parameterDTO.Parameter.Editable)
               return Captions.EnterAValue;

            return Captions.NaN;
         }

         if (_parameterDTO.IsDiscrete)
            return _parameterDTO.ListOfValues[valueToFormat];

         var formattedValue = base.Format(valueToFormat);
         return $"{formattedValue} {_parameterDTO.DisplayUnit}";
      }
   }
}