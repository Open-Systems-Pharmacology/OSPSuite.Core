using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class EventGroupXmlSerializer : ContainerXmlSerializer<EventGroup>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.EventGroupType);
      }
   }

   public class EventXmlSerializer : ContainerXmlSerializer<Event>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension);
         MapReference(x => x.Formula);
         Map(x => x.OneTime);
      }
   }

   public class EventAssignmentXmlSerializer : EntityXmlSerializer<EventAssignment>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension);
         MapReference(x => x.Formula);
         Map(x => x.UseAsValue);
         //object path is redundant but needs to be saved to allow cloning
         Map(x => x.ObjectPath);
         MapReference(x => x.ChangedEntity);
      }
   }
}