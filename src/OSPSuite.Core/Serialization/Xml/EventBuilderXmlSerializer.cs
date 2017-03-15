using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class EventAssignmentBuilderXmlSerializer : EntityXmlSerializer<EventAssignmentBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.UseAsValue);
         Map(x => x.ObjectPath);
         Map(x => x.Dimension);
         MapReference(x => x.Formula);
      }
   }

   public class EventBuilderXmlSerializer : EntityXmlSerializer<EventBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.OneTime);
         Map(x => x.Dimension);
         MapReference(x => x.Formula);
         MapEnumerable(x => x.Parameters, x => x.AddParameter);
         MapEnumerable(x => x.Assignments, x => x.AddAssignment);
      }
   }

   public class EventGroupBuilderBaseXmlSerializer<T> : ContainerXmlSerializer<T> where T : IEventGroupBuilder
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.SourceCriteria);
         Map(x => x.EventGroupType);
      }
   }

   public class EventGroupBuilderXmlSerializer : EventGroupBuilderBaseXmlSerializer<EventGroupBuilder>
   {
   }

   public class ApplicationBuilderXmlSerializer : EventGroupBuilderBaseXmlSerializer<ApplicationBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.MoleculeName);
      }
   }

   public class ApplicationMoleculeBuilderXmlSerializer : EntityXmlSerializer<ApplicationMoleculeBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.RelativeContainerPath);
         MapReference(x => x.Formula);
      }
   }
}