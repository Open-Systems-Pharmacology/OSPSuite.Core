using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Group of related events in model
   /// </summary>
   public class EventGroup : Container
   {
      /// <summary>
      ///    Type of the event. Typically this is the type of the application for an event group application
      ///    otherwise simply
      /// </summary>
      public string EventGroupType { get; set; }

      /// <summary>
      ///    List of (single) events of the group
      /// </summary>
      public IEnumerable<Event> Events => GetChildren<Event>();

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceEventGroup = source as EventGroup;
         if (sourceEventGroup == null) return;
         EventGroupType = sourceEventGroup.EventGroupType;
      }
   }
}