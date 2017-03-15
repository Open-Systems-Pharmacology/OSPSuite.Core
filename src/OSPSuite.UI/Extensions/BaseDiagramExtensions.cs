using System;
using System.Collections.Generic;
using Northwoods.Go;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Extensions
{
   public static class BaseDiagramExtensions
   {
      public static void ToFront(this GoObject goObject)
      {
         var goGroup = goObject.Parent;
         if (goGroup != null)
            try { goGroup.InsertAfter(null, goObject); }
            catch (Exception) { } 
         else
            try { goObject.Layer.MoveAfter(null, goObject); }
            catch (Exception) { } 
      }

      public static void ToBack(this GoObject goObject)
      {
         var goGroup = goObject.Parent;
         if (goGroup != null)
            try { goGroup.InsertBefore(null, goObject); }
            catch (Exception) { } 
         else 
            try { goObject.Layer.MoveBefore(null, goObject); }
            catch (Exception) { }
      }

   

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

      public static string GetLongNameId(this IBaseNode baseNode)
      {
         return GetLongName(baseNode) + "[" + baseNode.Id + "]"; ;
      }

   }
}
