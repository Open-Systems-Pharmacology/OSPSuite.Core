using System.Collections.Generic;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Extensions
{
   public static class BaseDiagramExtensions
   {
      private static void addUnique<T>(IList<T> list, T item)
      {
         if (!list.Contains(item)) list.Add(item);
      }

      public static IEnumerable<IContainerNode> GetParentNodes(this IBaseNode baseNode)
      {
         IList<IContainerNode> parentNodes = new List<IContainerNode>();

         var parent = baseNode.GetParent() as IContainerNode;
         while (parent != null)
         {
            addUnique(parentNodes, parent);
            parent = parent.GetParent() as IContainerNode;
         }

         return parentNodes;
      }

      public static string GetLongName(this IBaseNode baseNode)
      {
         string longName = baseNode.Name;
         var parent = baseNode.GetParent() as IContainerNode;
         while (parent != null)
         {
            longName = parent.Name + "/" + longName;
            parent = parent.GetParent() as IContainerNode;
         }
         return longName;
      }

   }
}
