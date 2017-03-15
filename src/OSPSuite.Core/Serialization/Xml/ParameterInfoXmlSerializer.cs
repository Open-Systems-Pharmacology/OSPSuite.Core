using System.Xml.Linq;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ParameterInfoXmlSerializer : OSPSuiteXmlSerializer<ParameterInfo>
   {
      private readonly IGroupRepository _groupRepository;

      public ParameterInfoXmlSerializer()
      {
         _groupRepository = IoC.Resolve<IGroupRepository>();
      }

      public override void PerformMapping()
      {
         Map(x => x.MinValue).WithMappingName(Constants.Serialization.Attribute.MIN_VALUE);
         Map(x => x.MaxValue).WithMappingName(Constants.Serialization.Attribute.MAX_VALUE);
         Map(x => x.ParameterFlag).WithMappingName(Constants.Serialization.Attribute.PARAMETER_FLAG);
         Map(x => x.Sequence).WithMappingName(Constants.Serialization.Attribute.SEQUENCE);
         Map(x => x.ReferenceId).WithMappingName(Constants.Serialization.Attribute.REFERENCE_ID);
         Map(x => x.BuildingBlockType).WithMappingName(Constants.Serialization.Attribute.BUILDING_BLOCK);
      }

      protected override void TypedDeserialize(ParameterInfo parameterInfo, XElement parameterInfoNode, SerializationContext serializationContext)
      {
         base.TypedDeserialize(parameterInfo, parameterInfoNode, serializationContext);
         var groupId = parameterInfoNode.GetAttribute(Constants.Serialization.Attribute.GROUP_ID);
         var group = _groupRepository.GroupById(groupId);
         parameterInfo.GroupName = group.Name;
      }

      protected override XElement TypedSerialize(ParameterInfo parameterInfo, SerializationContext serializationContext)
      {
         var parameterInfoNode = base.TypedSerialize(parameterInfo, serializationContext);
         var group = _groupRepository.GroupByName(parameterInfo.GroupName);
         parameterInfoNode.AddAttribute(Constants.Serialization.Attribute.GROUP_ID, group.Id);
         return parameterInfoNode;
      }
   }
}