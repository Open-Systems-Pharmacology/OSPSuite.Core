using System.Xml.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class GroupXmlSerializer : OSPSuiteXmlSerializer<Group>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Name);
         Map(x => x.Sequence);
         Map(x => x.Description);
         Map(x => x.DisplayName);
         Map(x => x.IconName);
         Map(x => x.Visible);
         Map(x => x.IsAdvanced);
         Map(x => x.PopDisplayName);
         Map(x => x.FullName);
         MapReference(x => x.Parent);
      }

      protected override void TypedDeserialize(Group group, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         //register group to enable reference resolve to parent mapping
         base.TypedDeserialize(group, outputToDeserialize,serializationContext);
         serializationContext.Register(group);
      }
   }

   public class GroupRepositoryXmlSerializer : OSPSuiteXmlSerializer<IGroupRepository>
   {
      public GroupRepositoryXmlSerializer():base(Constants.Serialization.GROUP_REPOSITORY)
      {
      }

      public override void PerformMapping()
      {
         MapEnumerable(x => x.All(), x => x.AddGroup);
      }

      public override IGroupRepository CreateObject(XElement element, SerializationContext serializationContext)
      {
         return IoC.Resolve<IGroupRepository>();
      }
   }
}