using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Events
{
   public class RenamedEvent
   {
      public IWithName RenamedObject { get; private set; }

      public RenamedEvent(IWithName renameObject)
      {
         RenamedObject = renameObject;
      }
   }

   public class ExportToPDFEvent
   {
      public object ObjectToExport { get; private set; }

      public ExportToPDFEvent(object objectToExport)
      {
         ObjectToExport = objectToExport;
      }
   }

   public class ChartTemplatesChangedEvent
   {
      public IWithChartTemplates WithChartTemplates { get; private set; }

      public ChartTemplatesChangedEvent(IWithChartTemplates withChartTemplates)
      {
         WithChartTemplates = withChartTemplates;
      }
   }
}