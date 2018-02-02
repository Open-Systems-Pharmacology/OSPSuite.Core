using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_TransferOptimizedParametersToSimulationsTask : ContextSpecification<ITransferOptimizedParametersToSimulationsTask>
   {
      protected ISetParameterTask _parameterTask;
      protected ParameterIdentificationRunResult _runResult;
      protected ParameterIdentification _parameterIdentification;
      private IdentificationParameter _identificationParameter1;
      private IdentificationParameter _identificationParameter2;
      protected IdentificationParameter _identificationParameter3;
      protected ParameterSelection _linkedParameter1;
      protected ParameterSelection _linkedParameter2;
      protected ParameterSelection _linkedParameter3;
      protected IDialogCreator _dialogCreator;
      protected ParameterSelection _linkedParameter4;
      private IOSPSuiteExecutionContext _context;
      protected List<ICommand> _allValueOriginCommands;

      protected override void Context()
      {
         _parameterTask = A.Fake<ISetParameterTask>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _parameterIdentification = new ParameterIdentification();
         _runResult = new ParameterIdentificationRunResult();

         _runResult.BestResult.AddValue(new OptimizedParameterValue("P1", 10, 20));
         _runResult.BestResult.AddValue(new OptimizedParameterValue("P2", 4, 5));

         _identificationParameter1 = new IdentificationParameter {Name = "P1"};
         _identificationParameter2 = new IdentificationParameter {Name = "P2", UseAsFactor = true};
         _identificationParameter3 = new IdentificationParameter {Name = "P3", IsFixed = true,};
         _identificationParameter3.Add(DomainHelperForSpecs.ConstantParameterWithValue(25).WithName(Constants.Parameters.START_VALUE));

         _linkedParameter1 = A.Fake<ParameterSelection>();
         A.CallTo(() => _linkedParameter1.Parameter).Returns(DomainHelperForSpecs.ConstantParameterWithValue(2));

         _linkedParameter2 = A.Fake<ParameterSelection>();
         A.CallTo(() => _linkedParameter2.Parameter).Returns(DomainHelperForSpecs.ConstantParameterWithValue(3));
         A.CallTo(() => _linkedParameter2.Dimension).Returns(Constants.Dimension.NO_DIMENSION);

         _linkedParameter3 = A.Fake<ParameterSelection>();
         A.CallTo(() => _linkedParameter3.Parameter).Returns(DomainHelperForSpecs.ConstantParameterWithValue(4));
         A.CallTo(() => _linkedParameter3.Dimension).Returns(Constants.Dimension.NO_DIMENSION);

         _linkedParameter4 = A.Fake<ParameterSelection>();
         A.CallTo(() => _linkedParameter4.Parameter).Returns(DomainHelperForSpecs.ConstantParameterWithValue(5));
         A.CallTo(() => _linkedParameter4.Dimension).Returns(Constants.Dimension.NO_DIMENSION);


         _identificationParameter1.AddLinkedParameter(_linkedParameter1);
         _identificationParameter2.AddLinkedParameter(_linkedParameter2);
         _identificationParameter2.AddLinkedParameter(_linkedParameter3);
         _identificationParameter3.AddLinkedParameter(_linkedParameter4);

         _parameterIdentification.AddIdentificationParameter(_identificationParameter1);
         _parameterIdentification.AddIdentificationParameter(_identificationParameter2);
         _parameterIdentification.AddIdentificationParameter(_identificationParameter3);

         _context = A.Fake<IOSPSuiteExecutionContext>();
         sut = new TestTransferOptimizedParametersToSimulationsTask(_parameterTask, _dialogCreator, _context);

         _allValueOriginCommands = new List<ICommand>();

         A.CallTo(() => _parameterTask.SetParameterValue(A<IParameter>._, A<double>._, A<ISimulation>._)).ReturnsLazily(x => A.Fake<IOSPSuiteCommmand<IOSPSuiteExecutionContext>>());

         A.CallTo(() => _parameterTask.UpdateParameterValueOrigin(A<IParameter>._, A<ValueOrigin>._, A<ISimulation>._)).ReturnsLazily(x =>
         {
            var command = A.Fake<IOSPSuiteCommmand<IOSPSuiteExecutionContext>>();
            _allValueOriginCommands.Add(command);
            return command;
         });
      }
   }

   public class TestTransferOptimizedParametersToSimulationsTask : TransferOptimizedParametersToSimulationsTask<IOSPSuiteExecutionContext>
   {
      public TestTransferOptimizedParametersToSimulationsTask(ISetParameterTask parameterTask, IDialogCreator dialogCreator, IOSPSuiteExecutionContext context) : base(parameterTask, dialogCreator, context)
      {
      }
   }

   public class When_transferring_the_optimized_parameters_of_a_completed_parameter_identification_to_the_simulations : concern_for_TransferOptimizedParametersToSimulationsTask
   {
      private ICommand _command;
      private Func<DateTime> _currentNow;
      private DateTime _now;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _now = new DateTime(1979, 05, 24);
         _currentNow = SystemTime.Now;
         SystemTime.Now = () => _now;
      }

      protected override void Context()
      {
         base.Context();
         _runResult.Status = RunStatus.RanToCompletion;
      }

      protected override void Because()
      {
         _command = sut.TransferParametersFrom(_parameterIdentification, _runResult);
      }

      [Observation]
      public void should_create_two_commands_for_each_parameter_to_update()
      {
         var macro = _command.DowncastTo<IMacroCommand>();
         //*2 because we have two commands per parmeter to update
         macro.Count.ShouldBeEqualTo(4 * 2);
      }

      [Observation]
      public void should_hide_all_value_origin_commands()
      {
         _allValueOriginCommands.Each(x => x.Visible.ShouldBeFalse());
      }

      [Observation]
      public void should_use_a_factor_if_the_corresponding_identification_parameter_was_using_a_factor()
      {
         A.CallTo(() => _parameterTask.SetParameterValue(_linkedParameter2.Parameter, 3 * 4, _linkedParameter2.Simulation)).MustHaveHappened();
         A.CallTo(() => _parameterTask.SetParameterValue(_linkedParameter3.Parameter, 4 * 4, _linkedParameter3.Simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_use_the_value_if_the_corresponding_identification_parameter_is_not_using_a_factor()
      {
         A.CallTo(() => _parameterTask.SetParameterValue(_linkedParameter1.Parameter, 10, _linkedParameter1.Simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_use_the_identification_parameter_start_value_if_the_corresponding_identification_parameter_is_fixed()
      {
         A.CallTo(() => _parameterTask.SetParameterValue(_linkedParameter4.Parameter, _identificationParameter3.StartValue, _linkedParameter4.Simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_not_warn_the_user()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(Warning.ImportingParameterIdentificationValuesFromCancelledRun)).MustNotHaveHappened();
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         SystemTime.Now = _currentNow;
      }
   }

   public class When_transferring_the_optimized_parameters_of_a_parameter_identification_to_the_simulations_with_a_parmaeter_that_does_not_exist : concern_for_TransferOptimizedParametersToSimulationsTask
   {
      protected override void Context()
      {
         base.Context();
         _runResult.BestResult = new OptimizationRunResult();
         _runResult.BestResult.AddValue(new OptimizedParameterValue("Does not exist", 1, 1));
         _parameterIdentification = new ParameterIdentification();
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.TransferParametersFrom(_parameterIdentification, _runResult)).ShouldThrowAn<OSPSuiteException>();
      }
   }

   public class When_transferring_the_optimized_parameter_of_a_run_that_was_cancelled : concern_for_TransferOptimizedParametersToSimulationsTask
   {
      protected override void Context()
      {
         base.Context();
         _runResult.Status = RunStatus.Canceled;
      }

      protected override void Because()
      {
         sut.TransferParametersFrom(_parameterIdentification, _runResult);
      }

      [Observation]
      public void should_warn_the_user()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Warning.ImportingParameterIdentificationValuesFromCancelledRun)).MustHaveHappened();
      }
   }

   public class When_transferring_the_optimized_parameter_of_a_run_that_was_cancelled_and_the_user_does_not_want_to_transfer_the_results : concern_for_TransferOptimizedParametersToSimulationsTask
   {
      private ICommand _command;

      protected override void Context()
      {
         base.Context();
         _runResult.Status = RunStatus.Canceled;
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Warning.ImportingParameterIdentificationValuesFromCancelledRun)).Returns(ViewResult.No);
      }

      protected override void Because()
      {
         _command = sut.TransferParametersFrom(_parameterIdentification, _runResult);
      }

      [Observation]
      public void should_warn_the_user()
      {
         _command.IsEmpty().ShouldBeTrue();
      }
   }

   public class When_transferring_the_optimized_parameter_of_a_run_that_was_cancelled_and_the_user_wants_to_transfer_the_results : concern_for_TransferOptimizedParametersToSimulationsTask
   {
      private ICommand _command;

      protected override void Context()
      {
         base.Context();
         _runResult.Status = RunStatus.Canceled;
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Warning.ImportingParameterIdentificationValuesFromCancelledRun)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         _command = sut.TransferParametersFrom(_parameterIdentification, _runResult);
      }

      [Observation]
      public void should_warn_the_user()
      {
         _command.IsEmpty().ShouldBeFalse();
      }
   }

   public class When_transferring_the_optimized_parameter_of_a_categorial_parameter_identification : concern_for_TransferOptimizedParametersToSimulationsTask
   {
      protected override void Context()
      {
         base.Context();
         _parameterIdentification.Configuration.RunMode = new CategorialParameterIdentificationRunMode();
      }

      protected override void Because()
      {
         sut.TransferParametersFrom(_parameterIdentification, _runResult);
      }

      [Observation]
      public void should_warn_the_user()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(Warning.ImportingParameterIdentificationValuesFromCategorialRun)).MustHaveHappened();
      }
   }
}