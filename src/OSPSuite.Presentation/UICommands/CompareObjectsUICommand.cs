using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.UICommands
{
   public class CompareObjectsUICommand : ObjectUICommand<IReadOnlyList<IObjectBase>>
   {
      private readonly IEventPublisher _eventPublisher;
      public IReadOnlyList<string> ObjectNames { get; set; }

      public CompareObjectsUICommand(IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
      }

      protected override void PerformExecute()
      {
         if (Subject.Count != 2)
            throw new OSPSuiteException(Error.CanOnlyCompareTwoObjectsAtATime);

         if (ObjectNames == null || ObjectNames.Count != 2)
            ObjectNames = new List<string> {Subject[0].Name, Subject[1].Name};

         _eventPublisher.PublishEvent(new StartComparisonEvent(
            leftObject: Subject[0],
            rightObject: Subject[1],
            leftCaption: ObjectNames[0],
            rightCaption: ObjectNames[1]));
      }
   }
}