using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class PathAndValueEntityXmlSerializer<T> : EntityXmlSerializer<T> where T : PathAndValueEntity
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension);
         Map(x => x.ContainerPath);
         MapReference(x => x.Formula);
         Map(x => x.Value);
      }
   }
}