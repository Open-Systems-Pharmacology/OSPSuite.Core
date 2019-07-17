using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class IdentificationParameterNameFormatter : IFormatter<string>
   {
      private readonly IdentificationParameterDTO _identificationParameterDTO;

      public IdentificationParameterNameFormatter(IdentificationParameterDTO identificationParameterDTO)
      {
         _identificationParameterDTO = identificationParameterDTO;
      }

      public string Format(string name)
      {
         return $"{name} ({_identificationParameterDTO.IdentificationParameter.AllLinkedParameters.Count})";
      }
   }
}