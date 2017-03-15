using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Serialization
{
   public class WorkspaceLayoutItemXmlSerializer : PresentationXmlSerializer<WorkspaceLayoutItem>
   {
      public override void PerformMapping()
      {
         Map(x => x.SubjectId);
         Map(x => x.PresentationSettings);
         Map(x => x.PresentationKey);
         Map(x => x.WasOpenOnSave);
      }
   }
}