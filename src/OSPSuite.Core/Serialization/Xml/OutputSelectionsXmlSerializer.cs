using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OutputSelectionsXmlSerializer : OSPSuiteXmlSerializer<OutputSelections>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.AllOutputs, x => x.AddOutput);
      }
   }
}