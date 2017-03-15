using System.Collections.Generic;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationIdentificationParametersView : IView<IParameterIdentificationIdentificationParametersPresenter>
   {
      void BindTo(IEnumerable<IdentificationParameterDTO> allIdentificationParameterDTOs);
      IdentificationParameterDTO SelectedIdentificationParameter { get; set; }
   }
}