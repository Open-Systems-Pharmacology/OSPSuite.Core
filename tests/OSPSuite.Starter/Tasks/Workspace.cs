using OSPSuite.Core;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure;
using OSPSuite.Infrastructure.Journal;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.FileLocker;

namespace OSPSuite.Starter.Tasks
{
   public class Workspace : Workspace<TestProject>
   {
      public Workspace(IEventPublisher eventPublisher, IJournalSession journalSession, IFileLocker fileLocker)
         : base(eventPublisher,  fileLocker)
      {
         _project = new TestProject();
      }
   }

   public class WithWorkspaceLayout : IWithWorkspaceLayout
   {
      public IWorkspaceLayout WorkspaceLayout { get; set; } = new WorkspaceLayout();
   }
}