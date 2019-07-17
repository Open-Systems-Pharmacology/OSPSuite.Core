using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_MultipleParameterIdentificationResultsPresenter : ContextSpecification<IMultipleParameterIdentificationResultsPresenter>
   {
      protected IParameterIdentificationRunResultToRunResultDTOMapper _runResultDTOMapper;
      protected IMultipleParameterIdentificationResultsView _view;
      protected ITransferOptimizedParametersToSimulationsTask _transferOptimizedParametersToSimulationTask;
      protected ParameterIdentification _parameterIdentification;
      protected ParameterIdentificationRunResult _runResult1;
      protected ParameterIdentificationRunResult _runResult2;
      protected ParameterIdentificationRunResultDTO _dto1;
      protected ParameterIdentificationRunResultDTO _dto2;
      protected List<ParameterIdentificationRunResultDTO> _allParameterIdentificationRunResultDTO;
      protected ICommandCollector _commandCollector;

      protected override void Context()
      {
         _runResultDTOMapper = A.Fake<IParameterIdentificationRunResultToRunResultDTOMapper>();
         _view = A.Fake<IMultipleParameterIdentificationResultsView>();
         _transferOptimizedParametersToSimulationTask = A.Fake<ITransferOptimizedParametersToSimulationsTask>();
         _commandCollector = A.Fake<ICommandCollector>();

         sut = new MultipleParameterIdentificationResultsPresenter(_view, _runResultDTOMapper, _transferOptimizedParametersToSimulationTask);
         sut.InitializeWith(_commandCollector);


         _parameterIdentification = new ParameterIdentification();
         _runResult1 = A.Fake<ParameterIdentificationRunResult>();
         _runResult2 = A.Fake<ParameterIdentificationRunResult>();
         _parameterIdentification.AddResult(_runResult1);
         _parameterIdentification.AddResult(_runResult2);

         _dto1 = new ParameterIdentificationRunResultDTO(_runResult1);
         _dto2 = new ParameterIdentificationRunResultDTO(_runResult2);

         A.CallTo(() => _runResultDTOMapper.MapFrom(_parameterIdentification, _runResult1)).Returns(_dto1);
         A.CallTo(() => _runResultDTOMapper.MapFrom(_parameterIdentification, _runResult2)).Returns(_dto2);

         A.CallTo(() => _view.BindTo(A<IEnumerable<ParameterIdentificationRunResultDTO>>._))
            .Invokes(x => _allParameterIdentificationRunResultDTO = x.GetArgument<IEnumerable<ParameterIdentificationRunResultDTO>>(0).ToList());
      }
   }

   public class When_editing_the_multiple_run_results_of_a_parameter_identification_presenter : concern_for_MultipleParameterIdentificationResultsPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_bind_one_result_dto_item_per_available_run_results_to_the_view()
      {
         _allParameterIdentificationRunResultDTO.ShouldOnlyContainInOrder(_dto1, _dto2);
      }
   }

   public class When_transferring_the_result_of_a_given_parameter_identification_run_to_simulations : concern_for_MultipleParameterIdentificationResultsPresenter
   {
      private ICommand _command;

      protected override void Context()
      {
         base.Context();
         _command = A.Fake<ICommand>();

         sut.EditParameterIdentification(_parameterIdentification);

         A.CallTo(() => _transferOptimizedParametersToSimulationTask.TransferParametersFrom(_parameterIdentification, _runResult1)).Returns(_command);
      }

      protected override void Because()
      {
         sut.TransferToSimulation(_dto1);
      }

      [Observation]
      public void should_update_the_parameters_and_add_the_corresponding_command_to_the_history()
      {
         A.CallTo(() => _commandCollector.AddCommand(_command)).MustHaveHappened();
      }
   }
}