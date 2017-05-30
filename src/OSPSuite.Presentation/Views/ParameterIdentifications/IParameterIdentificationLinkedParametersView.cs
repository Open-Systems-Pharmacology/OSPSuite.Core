using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationLinkedParametersView : IView<IParameterIdentificationLinkedParametersPresenter>
   {
      void BindTo(IEnumerable<LinkedParameterDTO> linkedParameterDTOs);
      void SetVisibility(PathElement pathElement, bool visible );
   }
}