using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class ContainerXmlSerializer<T> : EntityXmlSerializer<T> where T : IContainer 
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Mode);
         Map(x => x.ContainerType);
         MapEnumerable(x => x.Children,x => x.Add);
      }
   }

   public class ContainerXmlSerializer : ContainerXmlSerializer<Container>
   {
   }
}