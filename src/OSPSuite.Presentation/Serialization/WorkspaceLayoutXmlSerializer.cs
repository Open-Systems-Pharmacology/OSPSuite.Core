using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Serialization
{
   public class WorkspaceLayoutXmlSerializer : PresentationXmlSerializer<WorkspaceLayout>
   {
      public override void PerformMapping()
      {
         MapEnumerable(x => x.LayoutItems, x => x.AddLayoutItem);
      }
   }
}