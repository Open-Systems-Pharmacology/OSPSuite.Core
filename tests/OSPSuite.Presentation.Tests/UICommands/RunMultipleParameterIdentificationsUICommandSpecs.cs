using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;
using System.Collections.Generic;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class concern_for_RunMultipleParameterIdentificationsUICommand : ContextSpecification<RunMultipleParameterIdentificationsUICommand>
   {
      protected IObservedDataMetaDataTask _task = A.Fake<IObservedDataMetaDataTask>();
      protected IReadOnlyList<ParameterIdentification> _repository;
      protected IParameterIdentificationRunner _parameterIdentificationRunner;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected ParameterIdentification _parameterIdentification1;
      protected ParameterIdentification _parameterIdentification2;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification1 = A.Fake<ParameterIdentification>();
         _parameterIdentification2 = A.Fake<ParameterIdentification>();
         _repository = new List<ParameterIdentification> { _parameterIdentification1, _parameterIdentification2 };
         _parameterIdentificationRunner = A.Fake<IParameterIdentificationRunner>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         sut = new RunMultipleParameterIdentificationsUICommand(_parameterIdentificationRunner, _activeSubjectRetriever);

         sut.For(_repository);
      }
   }

   public class When_running_multiple_Parameter_Identifications : concern_for_RunMultipleParameterIdentificationsUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_run_each_parameter_identification_concurrently_in_runner()
      {
         A.CallTo(() => _parameterIdentificationRunner.Run(_parameterIdentification1)).MustHaveHappened();
         A.CallTo(() => _parameterIdentificationRunner.Run(_parameterIdentification2)).MustHaveHappened();
      }
   }
}