using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Serializer;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DescriptorCriteriaXmlSerializer : OSPSuiteXmlSerializer<DescriptorCriteria>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x, x => x.Add).WithMappingName(Constants.Serialization.DESCRIPTOR_CONDITIONS);
      }
   }

   public abstract class TagConditionXmlSerializer<T> : OSPSuiteXmlSerializer<T> where T : ITagCondition
   {
      public override void PerformMapping()
      {
         Map(x => x.Tag);
      }
   }

   public class MatchTagConditionXmlSerializer : TagConditionXmlSerializer<MatchTagCondition>
   {
   }

   public class NotMatchTagConditionXmlSerializer : TagConditionXmlSerializer<NotMatchTagCondition>
   {
   }

   public class MatchAllConditionXmlSerializer : TagConditionXmlSerializer<MatchAllCondition>
   {
      public override void PerformMapping()
      {
         /*nothing to do*/
      }
   }

   public class InContainerConditionXmlSerializer : TagConditionXmlSerializer<InContainerCondition>
   {
   }

   public class NotInContainerConditionXmlSerializer : TagConditionXmlSerializer<NotInContainerCondition>
   {
   }

   public class ParameterDescriptorXmlSerializer : OSPSuiteXmlSerializer<ParameterDescriptor>
   {
      public override void PerformMapping()
      {
         Map(x => x.ParameterName);
         Map(x => x.ContainerCriteria);
      }
   }
}