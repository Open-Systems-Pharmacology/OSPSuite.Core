using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   //necessary for PkSimNeighborhood
   public class NeighborhoodXmlSerializerBase<TNeighborhood> : ContainerXmlSerializer<TNeighborhood> where TNeighborhood : class, INeighborhood
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         MapReference(x => x.FirstNeighbor);
         MapReference(x => x.SecondNeighbor);
      }
   }

   public class NeighborhoodXmlSerializer : NeighborhoodXmlSerializerBase<Neighborhood>
   {
   }
}