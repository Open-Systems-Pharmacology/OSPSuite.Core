using OSPSuite.Core.Commands;
using OSPSuite.Core.Journal;

namespace OSPSuite.Infrastructure.Journal.Commands
{
   public class UpdateJournalDiagram
   {
      public JournalDiagram Diagram { get; set; }
   }

   public class UpdateJournalDiagramCommand : JournalDatabaseCommand<UpdateJournalDiagram>
   {
      private readonly IOSPSuiteExecutionContext _context;

      public UpdateJournalDiagramCommand(IJournalSession journalSession, IOSPSuiteExecutionContext context) : base(journalSession)
      {
         _context = context;
      }

      public override void Execute(UpdateJournalDiagram payload)
      {
         var diagram = payload.Diagram;
         diagram.Data = _context.Serialize(diagram.DiagramModel);

         Db.Diagrams.Update(diagram.Id, new
         {
            diagram.Name,
            diagram.Data
         });
      }
   }
}