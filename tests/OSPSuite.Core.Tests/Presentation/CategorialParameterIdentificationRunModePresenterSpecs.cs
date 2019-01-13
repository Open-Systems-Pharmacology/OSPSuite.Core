using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_CategorialParameterIdentificationRunModePresenter : ContextSpecification<CategorialParameterIdentificationRunModePresenter>
   {
      protected ICategorialParameterIdentificationRunModeView _view;
      protected ICategorialRunModeToCategorialRunModeDTOMapper _categorialDTOMapper;
      protected ICategoryDTORepository _categoryDTORepository;

      protected ParameterIdentification _parameterIdentification;
      protected CategorialParameterIdentificationRunMode _runMode;

      protected CategorialRunModeDTO _dto;
      protected ICategorialParameterIdentificationToCalculationMethodPermutationMapper _categorialToCalculationMethodPermutationMapper;

      protected override void Context()
      {
         _view = A.Fake<ICategorialParameterIdentificationRunModeView>();
         _categorialDTOMapper = A.Fake<ICategorialRunModeToCategorialRunModeDTOMapper>();
         _categoryDTORepository = A.Fake<ICategoryDTORepository>();
         _categorialToCalculationMethodPermutationMapper = A.Fake<ICategorialParameterIdentificationToCalculationMethodPermutationMapper>();
         sut = new CategorialParameterIdentificationRunModePresenter(_view, _categorialDTOMapper, _categorialToCalculationMethodPermutationMapper, _categoryDTORepository);

         _parameterIdentification = new ParameterIdentification();
         _runMode = new CategorialParameterIdentificationRunMode();
         _parameterIdentification.Configuration.RunMode = _runMode;

         A.CallTo(() => _view.BindTo(A<CategorialRunModeDTO>._))
            .Invokes(x => _dto = x.GetArgument<CategorialRunModeDTO>(0));

         A.CallTo(() => _categorialDTOMapper.MapFrom(_runMode, A<IReadOnlyList<CategoryDTO>>._, _parameterIdentification.AllSimulations))
            .Returns(new CategorialRunModeDTO(_runMode));

         A.CallTo(() => _view.BindTo(A<CategorialRunModeDTO>._))
            .Invokes(x => _dto = x.GetArgument<CategorialRunModeDTO>(0));

      }
   }

   public class When_asking_for_the_category_options_and_an_option_has_not_been_edited : concern_for_CategorialParameterIdentificationRunModePresenter
   {
      [Observation]
      public void should_return_a_new_instance()
      {
         sut.RunMode.ShouldBeAnInstanceOf<CategorialParameterIdentificationRunMode>();
      }

      [Observation]
      public void should_be_able_to_edit_a_category_optimization()
      {
         sut.CanEdit(_parameterIdentification).ShouldBeTrue();
      }
   }

   public class When_editing_new_category_options : concern_for_CategorialParameterIdentificationRunModePresenter
   {
      protected override void Because()
      {
         sut.Edit(_parameterIdentification);
      }

      [Observation]
      public void the_options_should_be_updated_to_use_the_edited()
      {
         sut.RunMode.ShouldBeEqualTo(_runMode);
      }

      [Observation]
      public void should_create_a_new_categorial_run_dto_and_bind_it_to_the_view()
      {
         _dto.CategorialRunMode.ShouldBeEqualTo(_runMode);
      }
   }

   public class When_the_categorial_run_mode_presenter_is_notified_that_the_flag_all_the_same_was_set_to_true : concern_for_CategorialParameterIdentificationRunModePresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_parameterIdentification);
         _runMode.CalculationMethodsCache.Add("Comp", new CalculationMethodCache());
         _runMode.AllTheSame = true;
      }

      protected override void Because()
      {
         sut.AllTheSameChanged();
      }

      [Observation]
      public void should_remove_all_calculation_method_ccahe_defined_in_the_parameter_identificaiton_run()
      {
         _runMode.CalculationMethodsCache.Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_rebind_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_dto)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_the_categorial_run_mode_presenter_is_notified_that_the_flag_all_the_same_was_set_to_false : concern_for_CategorialParameterIdentificationRunModePresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_parameterIdentification);
         _runMode.AllTheSameSelection.AddCalculationMethod(new CalculationMethod());
         _runMode.AllTheSame = false;
      }

      protected override void Because()
      {
         sut.AllTheSameChanged();
      }

      [Observation]
      public void should_remove_all_calculation_method_ccahe_defined_for_all_compounds()
      {
         _runMode.AllTheSameSelection.Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_rebind_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_dto)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_the_categorial_run_mode_presenter_is_notified_that_a_category_selection_has_changed : concern_for_CategorialParameterIdentificationRunModePresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.CategorySelectionChanged(new CategoryDTO());
      }

      [Observation]
      public void should_rebind_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_dto)).MustHaveHappenedTwiceExactly();
      }
   }


   public class When_the_categorial_run_mode_presenter_is_notified_that_a_calculation_method_is_selected_for_a_given_compound : concern_for_CategorialParameterIdentificationRunModePresenter
   {
      private CategoryDTO _categoryDTO;
      private CalculationMethod _calculationMethod;

      protected override void Context()
      {
         base.Context();
         _categoryDTO = new CategoryDTO().WithName("Category");
         _calculationMethod = new CalculationMethod().WithName("CM");
         _categoryDTO.Methods = new[] {_calculationMethod};
         A.CallTo(() => _categoryDTORepository.All()).Returns(new [] {_categoryDTO });

         sut = new CategorialParameterIdentificationRunModePresenter(_view, _categorialDTOMapper, _categorialToCalculationMethodPermutationMapper, _categoryDTORepository);

         sut.Edit(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.CalculationMethodSelectionChanged("Drug", "Category", "CM", true);
      }

      [Observation]
      public void should_add_the_selected_calculation_method_to_the_run()
      {
         _runMode.CalculationMethodsCache["Drug"].Contains("CM").ShouldBeTrue();
      }
   }

   public class When_the_categorial_run_mode_presenter_is_notified_that_a_calculation_method_is_deselected_for_a_given_compound : concern_for_CategorialParameterIdentificationRunModePresenter
   {
      private CategoryDTO _categoryDTO;
      private CalculationMethod _calculationMethod;


      protected override void Context()
      {
         base.Context();
         _categoryDTO = new CategoryDTO().WithName("Category");
         _calculationMethod = new CalculationMethod().WithName("CM");
         _categoryDTO.Methods = new[] { _calculationMethod };
         A.CallTo(() => _categoryDTORepository.All()).Returns(new[] { _categoryDTO });

         sut = new CategorialParameterIdentificationRunModePresenter(_view, _categorialDTOMapper, _categorialToCalculationMethodPermutationMapper, _categoryDTORepository);

         sut.Edit(_parameterIdentification);
         sut.CalculationMethodSelectionChanged("Drug", "Category", "CM", true);
      }

      protected override void Because()
      {
         sut.CalculationMethodSelectionChanged("Drug", "Category", "CM", false);
      }

      [Observation]
      public void should_add_the_selected_calculation_method_to_the_run()
      {
         _runMode.CalculationMethodsCache["Drug"].Contains("CM").ShouldBeFalse();
      }
   }
}