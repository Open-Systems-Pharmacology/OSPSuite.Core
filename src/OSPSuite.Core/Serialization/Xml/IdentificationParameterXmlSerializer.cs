using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class IdentificationParameterXmlSerializer : ContainerXmlSerializer<IdentificationParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Scaling);
         Map(x => x.UseAsFactor);
         MapEnumerable(x => x.AllLinkedParameters, x => x.AddLinkedParameter);
      }
   }
}