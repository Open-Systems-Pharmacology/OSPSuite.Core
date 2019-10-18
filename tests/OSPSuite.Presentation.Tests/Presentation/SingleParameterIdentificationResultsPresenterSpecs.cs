using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SingleParameterIdentificationResultsPresenter : ContextSpecification<ISingleParameterIdentificationResultsPresenter>
   {
      protected IParameterIdentificationRunResultToRunResultDTOMapper _runResultDTOMapper;
      protected ISingleParameterIdentificationResultsView _view;
      protected IParameterIdentificationRunPropertiesPresenter _runPropertiesPresenter;
      protected ParameterIdentificationRunResult _runResult;
      protected ParameterIdentification _parameterIdentification;
      private ITransferOptimizedParametersToSimulationsTask _transferParametersToSimulationTask;

      protected override void Context()
      {
         _runResultDTOMapper = A.Fake<IParameterIdentificationRunResultToRunResultDTOMapper>();
         _view = A.Fake<ISingleParameterIdentificationResultsView>();
         _runPropertiesPresenter = A.Fake<IParameterIdentificationRunPropertiesPresenter>();
         _transferParametersToSimulationTask= A.Fake<ITransferOptimizedParametersToSimulationsTask>();
         sut = new SingleParameterIdentificationResultsPresenter(_view, _runPropertiesPresenter, _runResultDTOMapper, _transferParametersToSimulationTask);

         _parameterIdentification = new ParameterIdentification();
         _runResult = A.Fake<ParameterIdentificationRunResult>();
         _parameterIdentification.AddResult(_runResult);
      }
   }

   public class When_editing_the_the_single_result_of_a_parameter_identification : concern_for_SingleParameterIdentificationResultsPresenter
   {
      private ParameterIdentificationRunResultDTO _resultDTO;

      protected override void Context()
      {
         base.Context();
         _resultDTO = new ParameterIdentificationRunResultDTO(_runResult);
         A.CallTo(() => _runResultDTOMapper.MapFrom(_parameterIdentification, _runResult)).Returns(_resultDTO);
      }

      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_map_the_first_result_of_the_parameter_identification_as_a_result_dto()
      {
         _resultDTO.ShouldNotBeNull();
      }

      [Observation]
      public void should_bind_the_result_dto_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_resultDTO)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_proprties_of_the_run_result()
      {
         A.CallTo(() => _runPropertiesPresenter.Edit(_runResult)).MustHaveHappened();
      }
   }
}