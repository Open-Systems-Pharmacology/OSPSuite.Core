using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ParameterValueXmlSerializer<T> : PathAndValueEntityXmlSerializer<T> where T : ParameterValue
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Origin);
         Map(x => x.Info);
         Map(x => x.IsDefault);
      }
   }

   public class ParameterValueXmlSerializer : ParameterValueXmlSerializer<ParameterValue>
   {
   }

   public class IndividualParameterXmlSerializer : ParameterValueXmlSerializer<IndividualParameter>
   {
   }

   public class ExpressionParameterXmlSerializer : ParameterValueXmlSerializer<ExpressionParameter>
   {
   }
}