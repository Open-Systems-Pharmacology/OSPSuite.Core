using Northwoods.Go;

namespace OSPSuite.UI.Views.Diagram
{
   // .NET 8 workaround: GoOverview inherits the broken GoView.DoContextClick and
   // the default GoToolContext in its mouse tools. Apply the same overrides so
   // right-clicks inside an overview don't trigger TypeLoadException.
   public class BaseDiagramOverview : GoOverview
   {
      public BaseDiagramOverview()
      {
         var contextTool = new SafeGoToolContext(this) { SingleSelection = false };
         ReplaceMouseTool(typeof(GoToolContext), contextTool);
      }

      public override bool DoContextClick(GoInputEventArgs evt)
      {
         return DiagramContextClick.Handle(
            this,
            evt,
            RaiseObjectContextClicked,
            RaiseBackgroundContextClicked);
      }
   }
}
