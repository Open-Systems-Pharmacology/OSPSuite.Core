using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Views.Comparisons;

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
         EnumHelper.AllValuesFor<PathElement>().Each(updateColumnVisibility);

         //Always hide name
         _view.SetVisibility(PathElement.Name, visible: false);
         _view.DifferenceTableVisible = _allDiffItemDTO.Any();
      }

      public DataTable ComparisonAsTable()
      {
         return _dataTableMapper.MapFrom(_allDiffItemDTO, _view.LeftCaption, _view.RightCaption);
      }

      public void Clear()
      {
         _view.ClearBinding();
      }

      private void updateColumnVisibility(PathElement pathElement)
      {
         _view.SetVisibility(pathElement, !_allDiffItemDTO.HasOnlyEmptyValuesAt(pathElement));
      }
   }
}