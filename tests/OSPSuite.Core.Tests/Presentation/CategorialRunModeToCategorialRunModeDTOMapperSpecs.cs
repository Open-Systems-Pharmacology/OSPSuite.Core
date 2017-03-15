using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_CategorialRunModeToCategorialRunModeDTOMapper : ContextSpecification<ICategorialRunModeToCategorialRunModeDTOMapper>
   {
      protected IReadOnlyList<ISimulation> _allSimulations;
      protected IReadOnlyList<CategoryDTO> _allCategories;
      protected CategorialParameterIdentificationRunMode _runMode;
      private ISimulation _simulation1;
      private ISimulation _simulation2;
      protected CategoryDTO _category1;
      protected CategoryDTO _category2;
      protected CalculationMethod _cm1;
      protected CalculationMethod _cm2;
      private CalculationMethod _cm3;

      protected override void Context()
      {
         sut = new CategorialRunModeToCategorialRunModeDTOMapper();
         _simulation1 = A.Fake<ISimulation>();
         A.CallTo(() => _simulation1.CompoundNames).Returns(new[] {"Drug1", "Drug2"});

         _simulation2 = A.Fake<ISimulation>();
         A.CallTo(() => _simulation2.CompoundNames).Returns(new[] {"Drug2", "Drug3"});

         _allSimulations = new List<ISimulation> {_simulation1, _simulation2};
         _category1 = new CategoryDTO().WithName("Category1");
         _category2 = new CategoryDTO().WithName("Category2");

         _cm1 = new CalculationMethod {Category = _category1.Name}.WithName("CM1");
         _cm2 = new CalculationMethod {Category = _category1.Name}.WithName("CM2");
         _cm3 = new CalculationMethod {Category = _category2.Name}.WithName("CM3");

         _category1.Methods = new[] {_cm1, _cm2};
         _category2.Methods = new[] {_cm3};

         _allCategories = new[] {_category1, _category2};

         _runMode = new CategorialParameterIdentificationRunMode();
      }

      protected void ValidateCalculationMethodSelection(DataTable dataTable, string compoundName, string calculationMethodName, bool selected)
      {
         var rowCM = RowForDataField(dataTable, compoundName, calculationMethodName);
         rowCM.ValueAt<bool>(Constants.CategoryOptimizations.VALUE).ShouldBeEqualTo(selected);
      }

      protected DataRow RowForDataField(DataTable dataTable, string compoundName, string calculationMethodName)
      {
         var rowFilter = $"{Constants.CategoryOptimizations.COMPOUND} = '{compoundName}' AND {Constants.CategoryOptimizations.CALCULATION_METHOD} = '{calculationMethodName}'";
         return (from DataRowView dataRowView in new DataView(dataTable, rowFilter, string.Empty, DataViewRowState.CurrentRows)
            select dataRowView.Row).First();
      }
   }

   public class When_mapping_a_categorial_run_mode_to_a_categorial_run_mode_dto : concern_for_CategorialRunModeToCategorialRunModeDTOMapper
   {
      private CategorialRunModeDTO _dto;

      protected override void Context()
      {
         base.Context();
         _runMode.CalculationMethodCacheFor("Drug1").AddCalculationMethod(_cm1);
         _runMode.CalculationMethodCacheFor("Drug3").AddCalculationMethod(_cm2);
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(_runMode, _allCategories, _allSimulations);
      }

      [Observation]
      public void should_select_the_categories_based_on_the_available_calculation_methods()
      {
         _category1.Selected.ShouldBeTrue();
         _category2.Selected.ShouldBeFalse();
      }

      [Observation]
      public void should_have_updated_the_calculation_method_table_as_expected()
      {
         var table = _dto.CalculationMethodSelectionTable;
         table.Rows.Count.ShouldBeEqualTo(6);

         ValidateCalculationMethodSelection(table, "Drug1", _cm1.Name, true);
         ValidateCalculationMethodSelection(table, "Drug1", _cm2.Name, false);
         ValidateCalculationMethodSelection(table, "Drug2", _cm1.Name, false);
         ValidateCalculationMethodSelection(table, "Drug2", _cm2.Name, false);
         ValidateCalculationMethodSelection(table, "Drug3", _cm1.Name, false);
         ValidateCalculationMethodSelection(table, "Drug3", _cm2.Name, true);
      }
   }

   public class When_updating_the_categorial_run_mode_dto_with_the_current_category_selection : concern_for_CategorialRunModeToCategorialRunModeDTOMapper
   {
      private CategorialRunModeDTO _dto;

      protected override void Context()
      {
         base.Context();
         _runMode.CalculationMethodCacheFor("Drug1").AddCalculationMethod(_cm1);
         _dto = sut.MapFrom(_runMode, _allCategories, _allSimulations);
         _category2.Selected = true;
      }

      protected override void Because()
      {
         sut.UpdateCalculationMethodsSelection(_dto, _allCategories, _allSimulations);
      }

      [Observation]
      public void should_keep_the_selected_items_as_is()
      {
         var table = _dto.CalculationMethodSelectionTable;
         ValidateCalculationMethodSelection(table, "Drug1", _cm1.Name, true);
      }

      [Observation]
      public void should_add_the_expected_new_entries_to_the_data_table()
      {
         var table = _dto.CalculationMethodSelectionTable;
         table.Rows.Count.ShouldBeEqualTo(9);
      }
   }

   public class When_updating_the_categorial_run_mode_dto_with_the_all_the_same_flag : concern_for_CategorialRunModeToCategorialRunModeDTOMapper
   {
      private CategorialRunModeDTO _dto;

      protected override void Context()
      {
         base.Context();
         _runMode.CalculationMethodCacheFor("Drug1").AddCalculationMethod(_cm1);
         _dto = sut.MapFrom(_runMode, _allCategories, _allSimulations);
         _runMode.AllTheSame = true;
      }

      protected override void Because()
      {
         sut.UpdateCalculationMethodsSelection(_dto, _allCategories, _allSimulations);
      }

      [Observation]
      public void should_only_have_one_entry_in_the_table_per_available_calculation_method()
      {
         var table = _dto.CalculationMethodSelectionTable;
         table.Rows.Count.ShouldBeEqualTo(2);
      }
   }
}