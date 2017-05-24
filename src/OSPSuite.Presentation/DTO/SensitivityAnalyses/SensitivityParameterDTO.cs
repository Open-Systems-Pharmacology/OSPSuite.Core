using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Presentation.DTO.SensitivityAnalyses
{
   public class SensitivityParameterDTO : DxValidatableDTO<SensitivityParameter>
   {
      public SensitivityParameter SensitivityParameter { get; }
      public string DisplayPath { get; set; }

      public SensitivityParameterDTO(SensitivityParameter sensitivityParameter) : base(sensitivityParameter)
      {
         SensitivityParameter = sensitivityParameter;
      }

      public string Name
      {
         get => SensitivityParameter.Name;
         set => SensitivityParameter.Name = value;
      }

      public IParameterDTO VariationRangeParameter { get; set; }
      public IParameterDTO NumberOfStepsParameter { get; set; }

      public double VariationRange
      {
         get => VariationRangeParameter.Value;
         set => VariationRangeParameter.Value = value;
      }

      public uint NumberOfSteps
      {
         get => NumberOfStepsParameter.Value.ConvertedTo<uint>();
         set => NumberOfStepsParameter.Value = value;
      }
   }
}