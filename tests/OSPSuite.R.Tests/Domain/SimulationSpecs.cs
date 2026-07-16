using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.R.Domain
{
   public abstract class concern_for_Simulation : ContextSpecification<Simulation>
   {
      protected IModelCoreSimulation _coreSimulation;
      protected SimulationConfiguration _configuration;

      protected override void Context()
      {
         _coreSimulation = A.Fake<IModelCoreSimulation>();
         _configuration = new SimulationConfiguration();
         A.CallTo(() => _coreSimulation.Configuration).Returns(_configuration);
         sut = new Simulation(_coreSimulation);
      }
   }

   public class When_getting_calculation_method_for_molecule_and_category : concern_for_Simulation
   {
      private string _result;

      protected override void Context()
      {
         base.Context();
         _configuration.AddCalculationMethodsOverridesFor("Caffeine", new List<UsedCalculationMethod>
         {
            new("PartitionCoefficient", "PK-Sim Standard"),
            new("CellularPermeabilities", "PK-Sim Standard")
         });
      }

      protected override void Because()
      {
         _result = sut.CalculationMethodFor("Caffeine", "PartitionCoefficient");
      }

      [Observation]
      public void should_return_the_calculation_method_name()
      {
         _result.ShouldBeEqualTo("PK-Sim Standard");
      }
   }

   public class When_getting_calculation_method_for_molecule_with_non_existing_category : concern_for_Simulation
   {
      private string _result;

      protected override void Context()
      {
         base.Context();
         _configuration.AddCalculationMethodsOverridesFor("Caffeine", new List<UsedCalculationMethod>
         {
            new("PartitionCoefficient", "PK-Sim Standard")
         });
      }

      protected override void Because()
      {
         _result = sut.CalculationMethodFor("Caffeine", "NonExistingCategory");
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_getting_calculation_method_for_non_existing_molecule : concern_for_Simulation
   {
      private string _result;

      protected override void Because()
      {
         _result = sut.CalculationMethodFor("NonExistingMolecule", "SomeCategory");
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }
}