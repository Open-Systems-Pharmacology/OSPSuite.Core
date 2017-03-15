using OSPSuite.Serializer;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DescriptorCriteriaXmlSerializer : OSPSuiteXmlSerializer<DescriptorCriteria>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x, x => x.Add).WithMappingName(Constants.Serialization.DESCRIPTOR_CONDITIONS);
      }
   }

   public class MatchTagConditionXmlSerializer : OSPSuiteXmlSerializer<MatchTagCondition>
   {
      public override void PerformMapping()
      {
         Map(x => x.Tag);
      }
   }

   public class NotMatchTagConditionXmlSerializer : OSPSuiteXmlSerializer<NotMatchTagCondition>
   {
      public override void PerformMapping()
      {
         Map(x => x.Tag);
      }
   }
   public class MatchAllConditionXmlSerializer  : OSPSuiteXmlSerializer<MatchAllCondition>
   {
      public override void PerformMapping()
      {
         /*nothing to do*/
      }
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