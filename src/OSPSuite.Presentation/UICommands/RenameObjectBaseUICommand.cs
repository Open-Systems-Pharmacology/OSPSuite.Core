using System.Collections.Generic;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class RenameObjectBaseUICommand<T> : ObjectUICommand<T> where T : class, IWithName
   {
      protected readonly IOSPSuiteExecutionContext _context;
      private readonly IEventPublisher _eventPublisher;
      private readonly IApplicationController _applicationController;

      protected RenameObjectBaseUICommand(IOSPSuiteExecutionContext context, IEventPublisher eventPublisher, IApplicationController applicationController)
      {
         _context = context;
         _eventPublisher = eventPublisher;
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         using (var renameObjectPresenter = _applicationController.Start<IRenameObjectPresenter>())
         {
            var newName = renameObjectPresenter.NewNameFrom(Subject, ForbiddenNamesFor(Subject));
            if (string.IsNullOrEmpty(newName)) return;

            loadSubjectBeforeRenaming();
            Subject.Name = newName;
            _eventPublisher.PublishEvent(new RenamedEvent(Subject));
         }

      }

      private void loadSubjectBeforeRenaming()
      {
         var objectBase = Subject as IObjectBase;
         if (objectBase == null) return;
         _context.Load(objectBase);
      }

      protected abstract IEnumerable<string> ForbiddenNamesFor(T subject);
   }
}