using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class WeightedObservedDataFormatter : IFormatter<DataRepository>
   {
      private readonly OutputMappingDTO _outputMappingDTO;

      public WeightedObservedDataFormatter(OutputMappingDTO outputMappingDTO)
      {
         _outputMappingDTO = outputMappingDTO;
      }

      public string Format(DataRepository observedData)
      {
         //Input value is not used as the observedData is already stored in the WeightedObservedDate. However the display needs to be adjusted
         return _outputMappingDTO?.WeightedObservedData?.DisplayName;
      }
   }
}