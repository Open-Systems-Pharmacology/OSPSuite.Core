using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;

namespace OSPSuite.UI.Extensions
{
   public static class TreeListMultiSelectionExtension
   {
      /// <summary>
      /// Get the currently selected TreeListNode(s) as a list
      /// </summary>
      /// <param name="treeList">The TreeListMultiSelection representing the currently selected nodes</param>
      /// <returns>The same TreeListNode as an IList</returns>
      public static IList<TreeListNode> ToList(this TreeListMultiSelection treeList)
      {
         return (from object item in treeList select item as TreeListNode).ToList();
      } 
   }
}