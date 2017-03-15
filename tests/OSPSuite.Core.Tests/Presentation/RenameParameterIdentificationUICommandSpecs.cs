using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_RenameParameterIdentificationUICommand : ContextSpecification<RenameParameterIdentificationUICommand>
   {
      protected IOSPSuiteExecutionContext _executionContext;
      private IEventPublisher _eventPublisher;
      private IProject _project;
      protected ParameterIdentification _parameterIdentification;
      private IApplicationController _applicationController;
      protected IRenameObjectPresenter _renameObjectPresenter;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _project = A.Fake<IProject>();
         _parameterIdentification = new ParameterIdentification();
         _applicationController = A.Fake<IApplicationController>();
         _renameObjectPresenter = A.Fake<IRenameObjectPresenter>();

         A.CallTo(() => _applicationController.Start<IRenameObjectPresenter>()).Returns(_renameObjectPresenter);
         A.CallTo(() => _executionContext.Project).Returns(_project);
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_parameterIdentification, A<IEnumerable<string>>._, A<string>._)).Returns("newName");

         A.CallTo(() => _project.AllParameterIdentifications).Returns(new[]
         {
            new ParameterIdentification {Name = "name1"},
            new ParameterIdentification {Name = "name2"}
         });

         sut = new RenameParameterIdentificationUICommand(_executionContext, _eventPublisher, _applicationController);
         sut.For(_parameterIdentification);
      }
   }

   public class When_renaming_a_parameter_identification : concern_for_RenameParameterIdentificationUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void the_forbidden_names_must_include_the_existing_parameter_identification_names()
      {
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_parameterIdentification, A<IEnumerable<string>>.That.Contains("name1"), A<string>._)).MustHaveHappened();
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_parameterIdentification, A<IEnumerable<string>>.That.Contains("name2"), A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void the_parameter_identification_name_should_be_updated()
      {
         _parameterIdentification.Name.ShouldBeEqualTo("newName");
      }

      [Observation]
      public void should_load_the_parameter_identification()
      {
         A.CallTo(() => _executionContext.Load(_parameterIdentification)).MustHaveHappened();
      }
   }
}
