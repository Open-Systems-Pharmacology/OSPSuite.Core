using OSPSuite.Utility.Format;
using OSPSuite.Presentation.DTO.ParameterIdentifications;

namespace OSPSuite.Presentation.Services.ParameterIdentifications
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