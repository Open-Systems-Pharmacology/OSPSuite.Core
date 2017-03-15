using System.Collections;
using System.Linq;
using Northwoods.Go;

namespace OSPSuite.UI.Diagram.Elements
{
   public class JournalPageDraggingTool : GoToolDragging
   {
      public JournalPageDraggingTool(GoView view)
         : base(view)
      {
         CopiesEffectiveSelection = true;
      }

      public override GoSelection ComputeEffectiveSelection(IGoCollection selection, bool move)
      {
         var result = base.ComputeEffectiveSelection(selection, move);
         if (move || CopiesEffectiveSelection)
         {
            AddSubtrees(result);
         }
         return result;
      }

      public static void AddSubtrees(IGoCollection selection)
      {
         var collection = new Hashtable();
         foreach (var obj in selection)
         {
            addReachable(collection, obj as IGoNode);
         }
         foreach (GoObject obj in collection.Keys)
         {
            selection.Add(obj);
         }
      }

      private static void addReachable(Hashtable collection, IGoNode node)
      {
         if (node == null) return;
         var goObject = node.GoObject;
         if (collection.ContainsKey(goObject)) return;
         collection.Add(goObject, goObject);
         foreach (var goLink in node.DestinationLinks.OfType<RelatedItemLink>())
         {
            var link = goLink.GoObject;
            if (!collection.ContainsKey(link))
               collection.Add(link, link);
            addReachable(collection, goLink.ToNode);
         }
      }
   }
}
