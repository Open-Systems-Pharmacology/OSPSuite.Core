using System.Collections.Generic;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Consists of one or more <see cref="EventBuilder" /> which are usually related
   ///    <para></para>
   ///    to each other in terms of modeled problem.
   ///    <para></para>
   ///    E.g. {Start of infusion; Stop of Infusion}
   /// </summary>
   public class EventGroupBuilder : Container, IBuilder
   {
      /// <summary>
      ///    Defines in which containers the event should be created
      /// </summary>
      public DescriptorCriteria SourceCriteria { get; set; }

      /// <summary>
      ///    Type of event groups
      /// </summary>
      public string EventGroupType { get; set; }

      public IBuildingBlock BuildingBlock { get; set; }

      public EventGroupBuilder()
      {
         SourceCriteria = new DescriptorCriteria();
         ContainerType = ContainerType.EventGroup;
      }

      /// <summary>
      ///    List of events in the group
      /// </summary>
      public IEnumerable<EventBuilder> Events => GetChildren<EventBuilder>();

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcEventGroupBuilder = source as EventGroupBuilder;
         if (srcEventGroupBuilder == null) return;
         EventGroupType = srcEventGroupBuilder.EventGroupType;
         SourceCriteria = srcEventGroupBuilder.SourceCriteria.Clone();
      }
   }
}