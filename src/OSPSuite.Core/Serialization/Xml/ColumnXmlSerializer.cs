using OSPSuite.Core.Import;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ColumnXmlSerializer : OSPSuiteXmlSerializer<Column>
   {
      public override void PerformMapping()
      {
         Map(x => x.ErrorStdDev);
         Map(x => x.LloqColumn);
         Map(x => x.Name);
         Map(x => x.Unit);
      }
   }
}
