using OSPSuite.Serializer.Xml;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Serialization
{
   public class GridColumnSettingsXmlSerializer : XmlSerializer<GridColumnSettings, SerializationContext>, IPresentationXmlSerializer
   {
      public override void PerformMapping()
      {
         Map(x => x.ColumnName);
         Map(x => x.Caption);
         Map(x => x.Visible);
         Map(x => x.GroupIndex);
         Map(x => x.VisibleIndex);
         Map(x => x.Width);
         Map(x => x.SortColumnName);
      }
   }
}