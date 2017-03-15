using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OutputMappingsXmlSerializer: OSPSuiteXmlSerializer<OutputMappings>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.All, x => x.Add);
      }
   }
}