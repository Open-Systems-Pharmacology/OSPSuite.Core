using System.Collections.Generic;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///   Group of related events in model
   /// </summary>
   public interface IEventGroup : IContainer
   {
      /// <summary>
      ///   List of (single) events of the group
      /// </summary>
      IEnumerable<IEvent> Events { get; }

      /// <summary>
      ///   Type of the event. Typically this is the type of the application for an event group application
      ///   otherwise simply
      /// </summary>
      string EventGroupType { get; set; }
   }

   public class EventGroup : Container, IEventGroup
   {
      public string EventGroupType { get; set; }

      public IEnumerable<IEvent> Events
      {
         get { return GetChildren<IEvent>(); }
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceEventGroup = source as IEventGroup;
         if (sourceEventGroup == null) return;
         EventGroupType = sourceEventGroup.EventGroupType;
      }
   }
}