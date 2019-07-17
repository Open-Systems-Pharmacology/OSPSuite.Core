using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ParameterIdentificationSimulationPathUpdater : ContextSpecification<ParameterIdentificationSimulationPathUpdater>
   {
      protected IOSPSuiteExecutionContext _executionContext;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         sut = new ParameterIdentificationSimulationPathUpdater(_executionContext);
      }
   }

   public class When_the_parameter_identification_task_is_updating_paths_for_a_simulation_name_change : concern_for_ParameterIdentificationSimulationPathUpdater
   {
      private ISimulation _simulation;
      private IProject _project;
      private ParameterIdentification _parameterIdentification;
      private DataRepository _simulationDataRepository;
      private IObjectPath _initialObjectPath;
      private DataRepository _observationDataRepository;
      private ResidualsResult _residualsResult;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
         _project = A.Fake<IProject>();
         _parameterIdentification = new ParameterIdentification();
         
         A.CallTo(() => _executionContext.Project).Returns(_project);
         A.CallTo(() => _project.AllParameterIdentifications).Returns(new[] { _parameterIdentification });

         _parameterIdentification.AddSimulation(_simulation);

         _initialObjectPath = new ObjectPath("oldName", "path");
         var parameterIdentificationRunResult = new ParameterIdentificationRunResult { BestResult = new OptimizationRunResult() };
         _simulationDataRepository = DomainHelperForSpecs.SimulationDataRepositoryFor("oldName");
         _observationDataRepository = DomainHelperForSpecs.ObservedData();

         _observationDataRepository.AllButBaseGrid().Each(x => x.QuantityInfo.Path = _initialObjectPath);
         _simulationDataRepository.AllButBaseGrid().Each(x => x.QuantityInfo.Path = _initialObjectPath);

         parameterIdentificationRunResult.BestResult.AddResult(_simulationDataRepository);
         _residualsResult = new ResidualsResult();
         _residualsResult.AddOutputResiduals(_initialObjectPath.PathAsString, _observationDataRepository, new List<Residual> { new Residual(0, 1, 1) });

         parameterIdentificationRunResult.BestResult.ResidualsResult = _residualsResult;
         _parameterIdentification.AddResult(parameterIdentificationRunResult);

         var outputMapping = new OutputMapping
         {
            WeightedObservedData = new WeightedObservedData(_observationDataRepository),
            OutputSelection = new ParameterSelection(_simulation, _initialObjectPath.PathAsString)
         };

         _parameterIdentification.AddOutputMapping(outputMapping);
      }

      protected override void Because()
      {
         sut.UpdatePathsForRenamedSimulation(_simulation, "oldName", "newName");
      }

      [Observation]
      public void should_load_the_identification_before_modification()
      {
         A.CallTo(() => _executionContext.Load(_parameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void output_paths_should_be_updated()
      {
         _residualsResult.AllOutputResiduals.First().FullOutputPath.ShouldBeEqualTo("newName|path");
         _observationDataRepository.FirstDataColumn().PathAsString.ShouldBeEqualTo("newName|path");
         _simulationDataRepository.FirstDataColumn().PathAsString.ShouldBeEqualTo("newName|path");
      }
   }
}