using OSPSuite.Core.Domain.PKAnalyses;

namespace OSPSuite.Core.Serialization.Xml
{
   public class PKParameterXmlSerializer : OSPSuiteXmlSerializer<PKParameter>
   {
      public override void PerformMapping()
      {  
         Map(x => x.Name);
         Map(x => x.Mode);
         Map(x => x.DisplayName);
         Map(x => x.Dimension);
         Map(x => x.Description);
      }
   }
}