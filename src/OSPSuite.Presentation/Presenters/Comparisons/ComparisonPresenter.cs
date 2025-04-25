using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Views.Comparisons;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Comparisons
{
   public interface IComparisonPresenter : IPresenter<IComparisonView>
   {
      void StartComparison(IObjectBase leftObject, IObjectBase rightObject, string leftCaption, string rightCaption, ComparerSettings comparerSettings);
      DataTable ComparisonAsTable();
      void Clear();
   }

   public class ComparisonPresenter : AbstractPresenter<IComparisonView, IComparisonPresenter>, IComparisonPresenter
   {
      private readonly IObjectComparer _objectComparer;
      private readonly IDiffItemToDiffItemDTOMapper _diffItemDTOMapper;
      private readonly IDiffItemDTOsToDataTableMapper _dataTableMapper;
      private IList<DiffItemDTO> _allDiffItemDTO;

      public ComparisonPresenter(IComparisonView view, IObjectComparer objectComparer, IDiffItemToDiffItemDTOMapper diffItemDTOMapper, IDiffItemDTOsToDataTableMapper dataTableMapper)
         : base(view)
      {
         _objectComparer = objectComparer;
         _diffItemDTOMapper = diffItemDTOMapper;
         _dataTableMapper = dataTableMapper;
         _view.DifferenceTableVisible = false;
      }

      private DiffItemDTO mapFrom(DiffItem diffItem)
      {
         return _diffItemDTOMapper.MapFrom(diffItem);
      }

      public void StartComparison(IObjectBase leftObject, IObjectBase rightObject, string leftCaption, string rightCaption, ComparerSettings comparerSettings)
      {
         var diffReport = _objectComparer.Compare(leftObject, rightObject, comparerSettings);
         _view.LeftCaption = leftCaption;
         _view.RightCaption = rightCaption;
         _allDiffItemDTO = diffReport.Select(mapFrom).ToList();
         _view.BindTo(_allDiffItemDTO);
         EnumHelper.AllValuesFor<PathElementId>().Each(updateColumnVisibility);

         //Always hide name
         _view.SetVisibility(PathElementId.Name, visible: false);

         if (isSimulationComparison(leftObject, rightObject))
            _view.SetVisibility(PathElementId.Simulation, visible: false);

         _view.DifferenceTableVisible = _allDiffItemDTO.Any();
      }

      private bool isSimulationComparison(IObjectBase leftObject, IObjectBase rightObject) => leftObject is ModelCoreSimulation && rightObject is ModelCoreSimulation;

      public DataTable ComparisonAsTable()
      {
         return _dataTableMapper.MapFrom(_allDiffItemDTO, _view.LeftCaption, _view.RightCaption);
      }

      public void Clear()
      {
         _view.ClearBinding();
      }

      private void updateColumnVisibility(PathElementId pathElementId)
      {
         _view.SetVisibility(pathElementId, !_allDiffItemDTO.HasOnlyEmptyValuesAt(pathElementId));
      }
   }
}