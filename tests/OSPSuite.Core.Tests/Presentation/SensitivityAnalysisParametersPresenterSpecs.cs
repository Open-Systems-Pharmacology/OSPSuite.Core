using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.SensitivityAnalyses;
using OSPSuite.Presentation.Mappers.SensitivityAnalyses;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_SensitivityAnalysisParametersPresenter : ContextSpecification<SensitivityAnalysisParametersPresenter>
   {
      protected ISensitivityAnalysisParametersView _sensitivityAnalysisParametersView;
      protected ISensitivityParameterFactory _sensitivityParameterFactory;
      protected ISensitivityParameterToSensitivityParameterDTOMapper _sensitivityParameterToSensitivityParameterDTOMapper;
      protected ISensitivityAnalysisSetValuePresenter _sensitivityAnalysisSetNMaxPresenter;
      protected ISensitivityAnalysisSetValuePresenter _sensitivityAnalysisSetRangePresenter;
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected SensitivityParameter _selectedSensitivityParameter;
      protected SensitivityParameter _unSelectedSensitivityParameter;
      protected IParameterDTO _parameterDTO;
      protected ISensitivityAnalysisTask _sensitivityAnalysisTask;
      protected SensitivityParameterDTO _sensitivityParameterDTO;

      protected override void Context()
      {
         _sensitivityAnalysisParametersView = A.Fake<ISensitivityAnalysisParametersView>();
         _sensitivityParameterFactory = A.Fake<ISensitivityParameterFactory>();
         _sensitivityParameterToSensitivityParameterDTOMapper = A.Fake<ISensitivityParameterToSensitivityParameterDTOMapper>();
         _sensitivityAnalysisSetNMaxPresenter = A.Fake<ISensitivityAnalysisSetValuePresenter>();
         _sensitivityAnalysisSetRangePresenter = A.Fake<ISensitivityAnalysisSetValuePresenter>();
         _sensitivityAnalysisTask= A.Fake<ISensitivityAnalysisTask>();

         sut = new SensitivityAnalysisParametersPresenter(_sensitivityAnalysisParametersView, _sensitivityParameterFactory, 
            _sensitivityParameterToSensitivityParameterDTOMapper, _sensitivityAnalysisSetNMaxPresenter, 
            _sensitivityAnalysisSetRangePresenter, _sensitivityAnalysisTask);
         _sensitivityAnalysis = new SensitivityAnalysis();

         _parameterDTO = A.Fake<IParameterDTO>();
         A.CallTo(() => _parameterDTO.Value).Returns(4.0);
         _selectedSensitivityParameter = A.Fake<SensitivityParameter>();
         _unSelectedSensitivityParameter = A.Fake<SensitivityParameter>();
         _sensitivityAnalysis.AddSensitivityParameter(_selectedSensitivityParameter);
         _sensitivityAnalysis.AddSensitivityParameter(_unSelectedSensitivityParameter);
         A.CallTo(() => _selectedSensitivityParameter.NumberOfStepsParameter).Returns(new Parameter());
         A.CallTo(() => _selectedSensitivityParameter.VariationRangeParameter).Returns(new Parameter());
         A.CallTo(() => _unSelectedSensitivityParameter.NumberOfStepsParameter).Returns(new Parameter());
         A.CallTo(() => _unSelectedSensitivityParameter.VariationRangeParameter).Returns(new Parameter());
         _sensitivityParameterDTO = new SensitivityParameterDTO(_selectedSensitivityParameter); 
         A.CallTo(() => _sensitivityAnalysisParametersView.SelectedParameters()).Returns(new[] { _sensitivityParameterDTO });

         A.CallTo(() => _sensitivityParameterToSensitivityParameterDTOMapper.MapFrom(_selectedSensitivityParameter)).Returns(_sensitivityParameterDTO);
         A.CallTo(() => _sensitivityParameterToSensitivityParameterDTOMapper.MapFrom(_unSelectedSensitivityParameter)).Returns(new SensitivityParameterDTO(_unSelectedSensitivityParameter));
      }
   }

   public class When_setting_a_number_of_steps_for_all : concern_for_SensitivityAnalysisParametersPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      protected override void Because()
      {
         _sensitivityAnalysisSetNMaxPresenter.ApplyAll(_parameterDTO);
      }

      [Observation]
      public void the_number_of_steps_presenter_should_change_all()
      {
         _selectedSensitivityParameter.NumberOfStepsParameter.Value.ShouldBeEqualTo(4);
         _unSelectedSensitivityParameter.NumberOfStepsParameter.Value.ShouldBeEqualTo(4);
      }
   }

   public class When_setting_a_number_of_steps_for_selection : concern_for_SensitivityAnalysisParametersPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      protected override void Because()
      {
         _sensitivityAnalysisSetNMaxPresenter.ApplySelection(_parameterDTO);
      }

      [Observation]
      public void the_number_of_steps_presenter_should_change_the_selected_parameters_only()
      {
         _selectedSensitivityParameter.NumberOfStepsParameter.Value.ShouldBeEqualTo(4);
         _unSelectedSensitivityParameter.NumberOfStepsParameter.Value.ShouldBeEqualTo(double.NaN);
      }
   }

   public class When_setting_a_range_for_all : concern_for_SensitivityAnalysisParametersPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      protected override void Because()
      {
         _sensitivityAnalysisSetRangePresenter.ApplyAll(_parameterDTO);
      }

      [Observation]
      public void the_number_of_steps_presenter_should_change_all()
      {
         _selectedSensitivityParameter.VariationRangeParameter.Value.ShouldBeEqualTo(4);
         _unSelectedSensitivityParameter.VariationRangeParameter.Value.ShouldBeEqualTo(4);
      }
   }

   public class When_setting_a_range_for_selection : concern_for_SensitivityAnalysisParametersPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      protected override void Because()
      {
         _sensitivityAnalysisSetRangePresenter.ApplySelection(_parameterDTO);
      }

      [Observation]
      public void the_number_of_steps_presenter_should_change_the_selected_parameters_only()
      {
         _selectedSensitivityParameter.VariationRangeParameter.Value.ShouldBeEqualTo(4);
         _unSelectedSensitivityParameter.VariationRangeParameter.Value.ShouldBeEqualTo(double.NaN);
      }
   }

   public class When_renaming_a_sensitivity_analysis_parameter : concern_for_SensitivityAnalysisParametersPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      protected override void Because()
      {
         sut.ChangeName(_sensitivityParameterDTO, "NEW NAME");
      }

      [Observation]
      public void should_leverage_the_sensitivity_analysis_task_to_rename_the_parameter_in_the_edited_sensitivity_analysis()
      {
         A.CallTo(() => _sensitivityAnalysisTask.UpdateSensitivityParameterName(_sensitivityAnalysis, _selectedSensitivityParameter, "NEW NAME")).MustHaveHappened();
      }
   }
}
