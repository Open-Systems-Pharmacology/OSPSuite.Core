using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.CLI.Core.MinimalImplementations;
using OSPSuite.Core.Domain;
using OSPSuite.R.Domain;
using OSPSuite.Utility;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SimulationPersister : ContextForIntegration<ISimulationPersister>
   {
      protected override void Context()
      {
         sut = Api.GetSimulationPersister();
      }
   }

   public class When_loading_a_simulation_from_file : concern_for_SimulationPersister
   {
      private string _simulationFile;
      private IModelCoreSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulationFile = HelperForSpecs.DataFile("S1.pkml");
      }

      protected override void Because()
      {
         _simulation = sut.LoadSimulation(_simulationFile);
      }

      [Observation]
      public void should_be_able_to_return_the_expected_simulation()
      {
         _simulation.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_kept_the_grouping_information_on_loaded_parameters()
      {
         var parameter = _simulation.Model.Root.EntityAt<IParameter>(Constants.ORGANISM, Constants.Parameters.WEIGHT);
         parameter.GroupName.ShouldNotBeEqualTo(Constants.Groups.UNDEFINED);
      }

      [Observation]
      public void the_group_repository_is_registered_correctly()
      {
         Api.Container.Resolve<IGroupRepository>().ShouldBeAnInstanceOf<GroupRepository>();
      }
   }

   public class When_saving_a_simulation_to_file : concern_for_SimulationPersister
   {
      private string _simulationFile;
      private Simulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulationFile = FileHelper.GenerateTemporaryFileName();
         var fileName = HelperForSpecs.DataFile("S1.pkml");
         _simulation = sut.LoadSimulation(fileName);
      }

      protected override void Because()
      {
         sut.SaveSimulation(_simulation, _simulationFile);
      }

      [Observation]
      public void should_be_able_to_return_the_expected_simulation()
      {
         FileHelper.FileExists(_simulationFile).ShouldBeTrue();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_simulationFile);
      }
   }
}