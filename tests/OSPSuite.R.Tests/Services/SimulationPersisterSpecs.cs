using System;
using System.IO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SimulationPersister : ContextSpecification<ISimulationPersister>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         Api.InitializeOnce(new ApiConfig {DimensionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Files.DIMENSIONS_FILE_NAME)});
      }

      protected override void Context()
      {
         sut = IoC.Resolve<ISimulationPersister>();
      }
   }

   public class When_loading_a_simulation_from_file : concern_for_SimulationPersister
   {
      private string _simulationFile;
      private IModelCoreSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "S1_concentrBased.pkml");
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
   }


   public class When_saving_a_simulation_to_file : concern_for_SimulationPersister
   {
      private string _simulationFile;
      private IModelCoreSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulationFile = FileHelper.GenerateTemporaryFileName();
         var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "S1_concentrBased.pkml");
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