using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ProcessXmlSerializer<T> : ContainerXmlSerializer<T> where T : class, IProcess
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Dimension);
         MapReference(x => x.Formula); 
      }
   }

   public class ProcessXmlSerializer : ProcessXmlSerializer<Process>
   {
   }
}