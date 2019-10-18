using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class concern_for_CloneParameterIdentificationUICommand : ContextSpecification<CloneParameterIdentificationUICommand>
   {
      protected ParameterIdentification _parameterIdentification;
      protected ISingleStartPresenterTask _singleStartPresenterTask;
      protected IParameterIdentificationTask _parameterIdentificationTask;
      protected ParameterIdentification _clonedParameterIdentification;

      protected override void Context()
      {
         _parameterIdentificationTask = A.Fake<IParameterIdentificationTask>();
         _singleStartPresenterTask = A.Fake<ISingleStartPresenterTask>();
         _clonedParameterIdentification = A.Fake<ParameterIdentification>();

         sut = new CloneParameterIdentificationUICommand(_singleStartPresenterTask, _parameterIdentificationTask);
         _parameterIdentification = A.Fake<ParameterIdentification>();
         sut.For(_parameterIdentification);
      }
   }

   public class When_cloning_a_parameter_identification_and_the_rename_is_canceled : concern_for_CloneParameterIdentificationUICommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentificationTask.Clone(_parameterIdentification)).Returns(null);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void the_editor_must_not_be_started_for_the_cloned_parameter_identification()
      {
         A.CallTo(() => _singleStartPresenterTask.StartForSubject(_clonedParameterIdentification)).MustNotHaveHappened();
      }

      [Observation]
      public void the_new_parameter_identification_must_not_be_added_to_the_project()
      {
         A.CallTo(() => _parameterIdentificationTask.AddToProject(_clonedParameterIdentification)).MustNotHaveHappened();
      }
   }

   public class When_cloning_a_parameter_identification : concern_for_CloneParameterIdentificationUICommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentificationTask.Clone(_parameterIdentification)).Returns(_clonedParameterIdentification);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void the_editor_must_be_started_for_the_cloned_parameter_identification()
      {
         A.CallTo(() => _singleStartPresenterTask.StartForSubject(_clonedParameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void the_new_parameter_identification_must_be_added_to_the_project()
      {
         A.CallTo(() => _parameterIdentificationTask.AddToProject(_clonedParameterIdentification)).MustHaveHappened();
      }
   }
}