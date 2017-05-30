using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class IdentificationParameterDTO : DxValidatableDTO<IdentificationParameter>
   {
      public IdentificationParameter IdentificationParameter { get; }
      public IParameterDTO StartValueParameter { get; set; }
      public IParameterDTO MinValueParameter { get; set; }
      public IParameterDTO MaxValueParameter { get; set; }
      public string Name => IdentificationParameter.Name;

      public IdentificationParameterDTO(IdentificationParameter identificationParameter) : base(identificationParameter)
      {
         IdentificationParameter = identificationParameter;
      }

      public bool UseAsFactor
      {
         get => IdentificationParameter.UseAsFactor;
         set => IdentificationParameter.UseAsFactor = value;
      }

      public bool IsFixed
      {
         get => IdentificationParameter.IsFixed;
         set => IdentificationParameter.IsFixed = value;
      }

      public Scalings Scaling
      {
         get => IdentificationParameter.Scaling;
         set => IdentificationParameter.Scaling = value;
      }

      /// <summary>
      ///    Start value value in display unit
      /// </summary>
      public double StartValue
      {
         get => StartValueParameter.Value;
         set => StartValueParameter.Value = value;
      }

      /// <summary>
      ///    Min value value in display unit
      /// </summary>
      public double MinValue
      {
         get => MinValueParameter.Value;
         set => MinValueParameter.Value = value;
      }

      /// <summary>
      ///    Max value value in display unit
      /// </summary>
      public double MaxValue
      {
         get => MaxValueParameter.Value;
         set => MaxValueParameter.Value = value;
      }
   }
}