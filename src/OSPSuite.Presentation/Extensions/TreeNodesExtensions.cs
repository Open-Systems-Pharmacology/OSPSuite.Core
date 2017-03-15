using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Extensions
{
   public static class TreeNodeExtensions
   {
      public static IReadOnlyList<T> Children<T>(this ITreeNode treeNode)
      {
         return treeNode.Children.OfType<T>().ToList();
      }

      public static IReadOnlyList<T> AllNodes<T>(this ITreeNode treeNode)
      {
         return treeNode.AllNodes.OfType<T>().ToList();
      }

      public static IReadOnlyList<ITreeNode<T>> NodesWithTags<T>(this ITreeNode treeNode)
      {
         return treeNode.Children<ITreeNode<T>>();
      }

      public static bool HasAncestor(this ITreeNode node, ITreeNode possibleAncestor)
      {
         if (node == null)
            return false;

         if (Equals(node, possibleAncestor))
            return true;

         return HasAncestor(node.ParentNode, possibleAncestor);
      }
   }
}