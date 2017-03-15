using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;

namespace OSPSuite.Core
{
   public abstract class concern_for_JournalDiagramFactory : ContextSpecification<IJournalDiagramFactory>
   {
      protected IDiagramModelFactory _diagramModelFactory;
      protected IJournalDiagramManagerFactory _journalDiagramManagerFactory;

      protected override void Context()
      {
         _diagramModelFactory = A.Fake<IDiagramModelFactory>();
         _journalDiagramManagerFactory = A.Fake<IJournalDiagramManagerFactory>();
         sut = new JournalDiagramFactory(_diagramModelFactory, _journalDiagramManagerFactory);
      }
   }

   public class When_creating_a_new_named_journal_diagram : concern_for_JournalDiagramFactory
   {
      private JournalDiagram _result;
      private IDiagramModel _diagramModel;
      private IDiagramManager<JournalDiagram> _diagramManager;

      protected override void Context()
      {
         base.Context();
         _diagramModel = A.Fake<IDiagramModel>();
         _diagramManager = A.Fake<IDiagramManager<JournalDiagram>>();
         A.CallTo(() => _diagramModelFactory.Create()).Returns(_diagramModel);
         A.CallTo(() => _journalDiagramManagerFactory.Create()).Returns(_diagramManager);
      }

      protected override void Because()
      {
         _result = sut.Create("XX");
      }

      [Observation]
      public void should_return_a_journal_diagram_with_the_given_name()
      {
         _result.Name.ShouldBeEqualTo("XX");
      }

      [Observation]
      public void should_create_a_default_diagram_model()
      {
         _result.DiagramModel.ShouldBeEqualTo(_diagramModel);
      }

      [Observation]
      public void should_create_a_default_diagram_manager()
      {
         _result.DiagramManager.ShouldBeEqualTo(_diagramManager);
      }
   }
}