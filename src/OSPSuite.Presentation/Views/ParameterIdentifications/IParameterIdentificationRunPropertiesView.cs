using System.Collections.Generic;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationRunPropertiesView : IView<IParameterIdentificationRunPropertiesPresenter>, IResizableView
   {
      void BindTo(IEnumerable<IRunPropertyDTO> properties);
   }
}