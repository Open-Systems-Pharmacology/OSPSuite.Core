using OSPSuite.Core.Domain.PKAnalyses;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PKParameterRepositoryXmlSerializer : OSPSuiteXmlSerializer<PKParameterRepository>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.All(), x => x.Add);
      }
   }
}