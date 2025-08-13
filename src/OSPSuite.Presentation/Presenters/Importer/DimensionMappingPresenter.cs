using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IDimensionMappingPresenter : IPresenter<IDimensionMappingView>, IDisposablePresenter
   {
      void EditUnitToDimensionMap(IReadOnlyList<DimensionSelectionDTO> ambiguousDimensionDTOs);
      void ApplyToAllMatching(DimensionSelectionDTO templateDTO);
   }

   public class DimensionMappingPresenter : AbstractDisposablePresenter<IDimensionMappingView, IDimensionMappingPresenter>, IDimensionMappingPresenter
   {
      private IReadOnlyList<DimensionSelectionDTO> _dimensionDTOs;

      public DimensionMappingPresenter(IDimensionMappingView view) : base(view)
      {
      }

      public void EditUnitToDimensionMap(IReadOnlyList<DimensionSelectionDTO> ambiguousDimensionDTOs)
      {
         _dimensionDTOs = ambiguousDimensionDTOs;
         _view.BindTo(_dimensionDTOs);
         _view.Display();
         if (_view.Canceled)
            _dimensionDTOs.Each(x => x.SelectedDimension = null);
      }

      public void ApplyToAllMatching(DimensionSelectionDTO templateDTO)
      {
         _dimensionDTOs.Where(x => templateDTO.Dimensions.ContainsAll(x.Dimensions)).Each(dto => dto.SelectedDimension = templateDTO.SelectedDimension);
      }

      public override bool CanClose => _dimensionDTOs.All(x => x.SelectedDimension != null);
   }
}