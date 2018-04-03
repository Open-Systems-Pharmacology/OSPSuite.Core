using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;

namespace OSPSuite.Presentation.Mappers.ParameterIdentifications
{
   public interface ICategorialRunModeToCategorialRunModeDTOMapper
   {
      CategorialRunModeDTO MapFrom(CategorialParameterIdentificationRunMode runMode, IReadOnlyList<CategoryDTO> calculationMethodCategories, IReadOnlyList<ISimulation> simulations);
      void UpdateCalculationMethodsSelection(CategorialRunModeDTO categorialRunModeDTO, IReadOnlyList<CategoryDTO> calculationMethodCategories, IReadOnlyList<ISimulation> simulations);
   }

   public class CategorialRunModeToCategorialRunModeDTOMapper : ICategorialRunModeToCategorialRunModeDTOMapper
   {
      public CategorialRunModeDTO MapFrom(CategorialParameterIdentificationRunMode categorialRunMode, IReadOnlyList<CategoryDTO> calculationMethodCategories, IReadOnlyList<ISimulation> simulations)
      {
         updateCategorySelection(categorialRunMode, calculationMethodCategories);

         return new CategorialRunModeDTO(categorialRunMode)
         {
            CalculationMethodSelectionTable = createSelectionTableFor(categorialRunMode, calculationMethodCategories, simulations)
         };
      }

      private DataTable createSelectionTableFor(CategorialParameterIdentificationRunMode categorialRunMode, IReadOnlyList<CategoryDTO> calculationMethodCategories, IReadOnlyList<ISimulation> simulations)
      {
         var dataTable = new DataTable();
         dataTable.AddColumns<string>(Constants.CategoryOptimizations.COMPOUND, Constants.CategoryOptimizations.CATEGORY, Constants.CategoryOptimizations.CATEGORY_DISPLAY,
            Constants.CategoryOptimizations.CALCULATION_METHOD, Constants.CategoryOptimizations.CALCULATION_METHOD_DISPLAY);

         dataTable.AddColumn<bool>(Constants.CategoryOptimizations.VALUE);

         var compounds = compoundsFromSimulations(simulations, categorialRunMode.AllTheSame);
         compounds.Each(compound => addCategories(categorialRunMode, compound, calculationMethodCategories, dataTable));

         return dataTable;
      }

      private void updateCategorySelection(CategorialParameterIdentificationRunMode categorialRunMode, IReadOnlyList<CategoryDTO> calculationMethodCategories)
      {
         calculationMethodCategories.Each(x => x.Selected = false);
         categorialRunMode.SelectedCategories.Each(c =>
         {
            var category = calculationMethodCategories.FindByName(c);
            if (category != null)
               category.Selected = true;
         });
      }

      public void UpdateCalculationMethodsSelection(CategorialRunModeDTO categorialRunModeDTO, IReadOnlyList<CategoryDTO> calculationMethodCategories, IReadOnlyList<ISimulation> simulations)
      {
         var categorialRunMode = categorialRunModeDTO.CategorialRunMode;

         var compounds = compoundsFromSimulations(simulations, categorialRunMode.AllTheSame);
         compounds.Each(compoundName =>
         {
            removeUnusedCalculationMethodFromCache(categorialRunMode.CalculationMethodCacheFor(compoundName), calculationMethodCategories);
         });

         categorialRunModeDTO.CalculationMethodSelectionTable = createSelectionTableFor(categorialRunMode, calculationMethodCategories, simulations);
      }

      private void removeUnusedCalculationMethodFromCache(CalculationMethodCache calculationMethodCache, IReadOnlyList<CategoryDTO> calculationMethodCategories)
      {
         foreach (var calculationMethod in calculationMethodCache.ToList())
         {
            if (!calculationMethodCategories.FindByName(calculationMethod.Category).Selected)
               calculationMethodCache.RemoveCalculationMethod(calculationMethod);
         }
      }

      private IEnumerable<string> compoundsFromSimulations(IEnumerable<ISimulation> simulations, bool allTheSame)
      {
         if (allTheSame)
            return new[] {Captions.ParameterIdentification.All};

         return simulations.SelectMany(x => x.CompoundNames).Distinct().OrderBy(x => x);
      }

      private void addCategories(CategorialParameterIdentificationRunMode categorialRunMode, string compound, IEnumerable<CategoryDTO> categories, DataTable dataTable)
      {
         categories.Where(category => category.Selected).Each(category => addMethods(categorialRunMode, compound, category, dataTable));
      }

      private void addMethods(CategorialParameterIdentificationRunMode categorialRunMode, string compound, CategoryDTO category, DataTable dataTable)
      {
         category.Methods.Each(method => addMethods(categorialRunMode, compound, category, method, dataTable));
      }

      private void addMethods(CategorialParameterIdentificationRunMode categorialRunMode, string compound, CategoryDTO category, CalculationMethod calculationMethod, DataTable dataTable)
      {
         var row = dataTable.NewRow();
         row[Constants.CategoryOptimizations.COMPOUND] = compound;
         row[Constants.CategoryOptimizations.CATEGORY] = category.Name;
         row[Constants.CategoryOptimizations.CATEGORY_DISPLAY] = category.DisplayName;
         row[Constants.CategoryOptimizations.CALCULATION_METHOD] = calculationMethod.Name;
         row[Constants.CategoryOptimizations.CALCULATION_METHOD_DISPLAY] = calculationMethod.DisplayName;
         row[Constants.CategoryOptimizations.VALUE] = categorialRunMode.IsSelected(compound, calculationMethod);
         dataTable.Rows.Add(row);
      }
   }
}