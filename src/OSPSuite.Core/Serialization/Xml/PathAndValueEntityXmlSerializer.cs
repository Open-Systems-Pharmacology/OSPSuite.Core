using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PathAndValueEntityXmlSerializer<T> : EntityXmlSerializer<T> where T : PathAndValueEntity
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension);
         Map(x => x.ContainerPath);
         MapReference(x => x.Formula);
      }
   }
}