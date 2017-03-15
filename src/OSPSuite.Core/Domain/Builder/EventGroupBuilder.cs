using System.Collections.Generic;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{

   /// <summary>
   ///   Consists of one or more <see cref = "IEventBuilder" /> which are usually related <para></para>
   ///   to each other in terms of modeled problem. <para></para>
   ///   E.g. {Start of infusion; Stop of Infusion}
   /// </summary>
   public interface IEventGroupBuilder : IContainer
   {
      /// <summary>
      ///   Defines in which containers the event should be created
      /// </summary>
      DescriptorCriteria SourceCriteria { get; set; }

      /// <summary>
      ///   List of events in the group
      /// </summary>
      IEnumerable<IEventBuilder> Events { get; }

      /// <summary>
      /// Type of event groups
      /// </summary>
      string EventGroupType { get; set; }
   }

   public class EventGroupBuilder : Container, IEventGroupBuilder
   {
      public DescriptorCriteria SourceCriteria { get; set; }
      public string EventGroupType { get; set; }

      public EventGroupBuilder()
      {
         SourceCriteria = new DescriptorCriteria();
         ContainerType = ContainerType.EventGroup;
      }

      public IEnumerable<IEventBuilder> Events
      {
         get { return GetChildren<IEventBuilder>(); }
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcEventGroupBuilder = source as IEventGroupBuilder;
         if (srcEventGroupBuilder == null) return;
         EventGroupType = srcEventGroupBuilder.EventGroupType;
         SourceCriteria = srcEventGroupBuilder.SourceCriteria.Clone();
      }
   }
}