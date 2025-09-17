using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IGroupRepositoryPersistor : IFilePersistor<IGroupRepository>
   {
   }

   public class GroupRepositoryPersistor : AbstractFilePersistor<IGroupRepository>, IGroupRepositoryPersistor
   {
      public GroupRepositoryPersistor(IOSPSuiteXmlSerializerRepository serializerRepository, IContainer container)
         : base(serializerRepository, container)
      {
      }

      public override void Load(IGroupRepository groupRepository, string fileName)
      {
         groupRepository.Clear();

         using (var serializationContext = SerializationTransaction.Create(_container, withIdRepository: new WithIdRepository()))
         {
            var serializer = _serializerRepository.SerializerFor(groupRepository);
            var element = XElementSerializer.PermissiveLoad(fileName);
            serializer.Deserialize(groupRepository, element, serializationContext);
         }

         //last step. groups only have reference to parents. We need to add them as child
         foreach (var group in groupRepository.All().Where(x => x.Parent != null))
         {
            group.Parent.AddChild(group);
         }
      }
   }
}