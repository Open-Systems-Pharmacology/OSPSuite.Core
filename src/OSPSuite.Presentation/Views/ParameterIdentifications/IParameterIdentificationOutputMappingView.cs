using System.Collections.Generic;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationOutputMappingView : IView<IParameterIdentificationOutputMappingPresenter>
   {
      void BindTo(IEnumerable<OutputMappingDTO> outputMappingList);
      void CloseEditor();
   }
}