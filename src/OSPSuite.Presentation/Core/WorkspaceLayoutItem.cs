namespace OSPSuite.Presentation.Core
{
   public class WorkspaceLayoutItem
   {
      public IPresentationSettings PresentationSettings { get; set; }
      public string SubjectId { get; set; }
      public string PresentationKey { get; set; }
      public bool WasOpenOnSave { get; set; }
   }
}
