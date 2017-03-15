using System.Collections.Generic;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationParametersFeedbackView : IView<IParameterIdentificationParametersFeedbackPresenter>
   {
      void RefreshData();
      void BindTo(IEnumerable<ParameterFeedbackDTO> parametersDTO, IEnumerable<IRunPropertyDTO> propertiesDTO);
      bool CanExportParametersHistory { get; set; }
   }
}