using OSPSuite.Core.Import;
namespace OSPSuite.Core.Serialization.Xml
{
   public class UnitDescriptionXmlSerializer : OSPSuiteXmlSerializer<UnitDescription>
   {
      public override void PerformMapping()
      {
         Map(x => x.ColumnName);
         Map(x => x.SelectedUnit);
      }
   }
}
