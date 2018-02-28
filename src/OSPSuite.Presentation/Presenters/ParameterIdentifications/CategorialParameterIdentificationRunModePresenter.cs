using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface ICategorialParameterIdentificationRunModePresenter : IPresenter<ICategorialParameterIdentificationRunModeView>, IParameterIdentificationRunModePresenter, ILatchable
   {
      void CategorySelectionChanged(CategoryDTO categoryDTO);
      IEnumerable<string> RowAreaColumns { get; }
      IEnumerable<string> VisibleColumnAreaColumns { get; }
      IEnumerable<string> HiddenColumnAreaColumns { get; }
      string ValueAreaColumn { get; }
      int WarningThreshold { get; }
      bool HasCategories { get; }
      void AllTheSameChanged();
      void CalculationMethodSelectionChanged(string compoundName, string categoryName, string calculationMethodName, bool selected);
   }

   public class CategorialParameterIdentificationRunModePresenter : ParameterIdentificationRunModePresenter<ICategorialParameterIdentificationRunModeView, ICategorialParameterIdentificationRunModePresenter, CategorialParameterIdentificationRunMode>, ICategorialParameterIdentificationRunModePresenter
   {
      private readonly ICategorialRunModeToCategorialRunModeDTOMapper _categorialDTOMapper;
      private readonly ICategorialParameterIdentificationToCalculationMethodPermutationMapper _categorialCalculationMethodPermutationMapper;
      private CategorialRunModeDTO _runModeDTO;
      private readonly List<CategoryDTO> _allCategoriesDTO;
      public string ValueAreaColumn { get; } = Constants.CategoryOptimizations.VALUE;
      public int WarningThreshold { get; } = Constants.CategoryOptimizations.WARNING_THRESHOLD;
      public bool IsLatched { get; set; }

      public CategorialParameterIdentificationRunModePresenter(ICategorialParameterIdentificationRunModeView view,
         ICategorialRunModeToCategorialRunModeDTOMapper categorialDTOMapper, ICategorialParameterIdentificationToCalculationMethodPermutationMapper categorialCalculationMethodPermutationMapper,
         ICategoryDTORepository categoryDTORepository) : base(view)
      {
         _categorialDTOMapper = categorialDTOMapper;
         _categorialCalculationMethodPermutationMapper = categorialCalculationMethodPermutationMapper;
         _allCategoriesDTO = categoryDTORepository.All().OrderBy(x => x.DisplayName).ToList();
         _view.BindTo(_allCategoriesDTO);
      }

      public bool HasCategories => _allCategoriesDTO.Any();

      public override void Edit(ParameterIdentification parameterIdentification)
      {
         base.Edit(parameterIdentification);
         this.DoWithinLatch(() =>
         {
            _runModeDTO = _categorialDTOMapper.MapFrom(_runMode, _allCategoriesDTO, _parameterIdentification.AllSimulations);
            _view.BindTo(_runModeDTO);
         });
         updateOptimizationCount();
      }

      private void rebind()
      {
         this.DoWithinLatch(() =>
         {
            _categorialDTOMapper.UpdateCalculationMethodsSelection(_runModeDTO, _allCategoriesDTO, _parameterIdentification.AllSimulations);
            _view.BindTo(_runModeDTO);
         });
         updateOptimizationCount();
      }

      public void CategorySelectionChanged(CategoryDTO categoryDTO)
      {
         rebind();
      }

      public void AllTheSameChanged()
      {
         if (_runMode.AllTheSame)
            _runMode.CalculationMethodsCache.Clear();
         else
            _runMode.AllTheSameSelection.Clear();

         rebind();
      }

      public void CalculationMethodSelectionChanged(string compoundName, string categoryName, string calculationMethodName, bool selected)
      {
         var calculationMethodCache = _runMode.CalculationMethodCacheFor(compoundName);
         var category = _allCategoriesDTO.FindByName(categoryName);
         var calculationMethod = category.Methods.FindByName(calculationMethodName);

         if (selected)
            calculationMethodCache.AddCalculationMethod(calculationMethod);
         else
            calculationMethodCache.RemoveCalculationMethod(calculationMethod);

         updateOptimizationCount();
      }

      private void updateOptimizationCount()
      {
         View.UpdateParameterIdentificationCount(_categorialCalculationMethodPermutationMapper.MapFrom(_parameterIdentification).Count());
      }

      public IEnumerable<string> RowAreaColumns
      {
         get { yield return Constants.CategoryOptimizations.COMPOUND; }
      }

      public IEnumerable<string> VisibleColumnAreaColumns
      {
         get
         {
            yield return Constants.CategoryOptimizations.CATEGORY_DISPLAY;
            yield return Constants.CategoryOptimizations.CALCULATION_METHOD_DISPLAY;
         }
      }

      public IEnumerable<string> HiddenColumnAreaColumns
      {
         get
         {
            yield return Constants.CategoryOptimizations.CATEGORY;
            yield return Constants.CategoryOptimizations.CALCULATION_METHOD;
         }
      }
   }
}