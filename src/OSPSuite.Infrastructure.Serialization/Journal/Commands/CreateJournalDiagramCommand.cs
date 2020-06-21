using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Serialization.Journal.Commands
{
   public class CreateJournalDiagram
   {
      public JournalDiagram Diagram { get; set; }
   }

   public class CreateJournalDiagramCommand : JournalDatabaseCommand<CreateJournalDiagram>
   {
      private readonly IIdGenerator _idGenerator;
      private readonly IOSPSuiteExecutionContext _context;

      public CreateJournalDiagramCommand(IIdGenerator idGenerator, IJournalSession journalSession, IOSPSuiteExecutionContext context) : base(journalSession)
      {
         _idGenerator = idGenerator;
         _context = context;
      }

      public override void Execute(CreateJournalDiagram payload)
      {
         var diagram = payload.Diagram;

         if (!diagram.IsTransient)
            throw new CannotCreateNewItemForPersitedEntity(diagram.Id);

         diagram.Id = _idGenerator.NewId();
         diagram.Data = _context.Serialize(diagram.DiagramModel);

         Db.Diagrams.Insert(new
         {
            diagram.Id,
            diagram.Name,
            diagram.Data,
         });
      }
   }
}