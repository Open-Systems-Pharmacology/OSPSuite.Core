using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SimulationSettings : ContextSpecification<ISimulationSettings>
   {
      protected override void Context()
      {
         sut = new SimulationSettings();
      }
   }

   public class When_the_simulation_settings_is_being_visited : concern_for_SimulationSettings
   {
      private IVisitor _visitor;
      private OutputSchema _simulationOutput;

      protected override void Context()
      {
         base.Context();
         _visitor = A.Fake<IVisitor>();
         _simulationOutput = A.Fake<OutputSchema>();
         sut.OutputSchema = _simulationOutput;
      }

      protected override void Because()
      {
         sut.AcceptVisitor(_visitor);
      }

      [Observation]
      public void should_also_visit_the_simulation_outputs()
      {
         A.CallTo(() => _simulationOutput.AcceptVisitor(_visitor)).MustHaveHappened();
      }
   }
}