using System.Xml.Linq;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class NeighborhoodBuilderXmlSerializer : ContainerXmlSerializer<NeighborhoodBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         //this is the real mapping
         Map(x => x.FirstNeighborPath);
         Map(x => x.SecondNeighborPath);

         //Compatibility with older projects
         MapReference(x => x.FirstNeighbor);
         MapReference(x => x.SecondNeighbor);
      }

      protected override XElement TypedSerialize(NeighborhoodBuilder neighborhoodBuilder, SerializationContext context)
      {
         var element = base.TypedSerialize(neighborhoodBuilder, context);

         //We need to remove the reference to the first and second neighbor as they are not serialized anymore if defined;
         element.Attribute("firstNeighbor")?.Remove();
         element.Attribute("secondNeighbor")?.Remove();

         return element;
      }
   }
}