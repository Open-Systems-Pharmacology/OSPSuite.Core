using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface IDimensionMappingView : IModalView<IDimensionMappingPresenter>
   {
      void BindTo(IReadOnlyList<DimensionSelectionDTO> dimensionDTOs);
   }
}