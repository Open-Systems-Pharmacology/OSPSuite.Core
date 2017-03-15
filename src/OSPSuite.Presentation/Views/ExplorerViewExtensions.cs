using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Views
{
   public static class ExplorerViewExtensions
   {
      public static void DestroyNodes(this IExplorerView explorerView)
      {
         explorerView.TreeView.DestroyAllNodes();
      }

      /// <summary>
      ///    Determines if the node is already being shown by the view
      /// </summary>
      /// <param name="explorerView">The view being searched</param>
      /// <param name="node">The node to search for in the view</param>
      /// <returns>If the view contains this node, returns true, otherwise false</returns>
      public static bool ContainsNode(this IExplorerView explorerView, ITreeNode node)
      {
         return explorerView.NodeById(node.Id) != null;
      }

      /// <summary>
      ///    This removes the node from the view without clearing any of its sub nodes or tag. That way the node can be
      ///    attached under another node
      /// </summary>
      public static void RemoveNode(this IExplorerView explorerView, ITreeNode nodeToRemove)
      {
         explorerView.TreeView.RemoveNode(nodeToRemove);
      }

      /// <summary>
      ///    This removes the node from the view and delete all sub nodes. Reference to tag is also removed.
      /// </summary>
      public static void DestroyNode(this IExplorerView explorerView, ITreeNode nodeToRemove)
      {
         explorerView.TreeView.DestroyNode(nodeToRemove);
      }

      /// <summary>
      ///    This removes the node from the view and delete all sub nodes. Reference to tag is also removed.
      /// </summary>
      public static void DestroyNode(this IExplorerView explorerView, string nodeId)
      {
         DestroyNode(explorerView, NodeById(explorerView, nodeId));
      }

      public static ITreeNode NodeByType(this IExplorerView explorerView, RootNodeType rootNodeType)
      {
         return NodeById(explorerView, rootNodeType.Id);
      }

      public static ITreeNode NodeById(this IExplorerView explorerView, string id)
      {
         return explorerView.TreeView.NodeById(id);
      }

      public static void ExpandNode(this IExplorerView explorerView, ITreeNode nodeToExpand)
      {
         explorerView.TreeView.ExpandNode(nodeToExpand);
      }

      public static void CollapseNode(this IExplorerView explorerView, ITreeNode nodeToCollapse)
      {
         explorerView.TreeView.CollapseNode(nodeToCollapse);
      }

      public static void ExpandNodeIfRequired(this IExplorerView explorerView, ITreeNode nodeToExpand, bool shouldExpand)
      {
         if (shouldExpand)
            ExpandNode(explorerView, nodeToExpand);
      }

      public static void ExpandAllNodes(this IExplorerView explorerView)
      {
         explorerView.TreeView.ExpandAllNodes();
      }

      public static void SelectNode(this IExplorerView explorerView, ITreeNode treeNode)
      {
         explorerView.TreeView.SelectNode(treeNode);
      }

      public static void SelectFocusedNodeOrFirst(this IExplorerView explorerView)
      {
         explorerView.TreeView.SelectFocusedNodeOrFirst();
      }

      public static bool IsNodeExpanded(this IExplorerView explorerView, ITreeNode treeNode)
      {
         return explorerView.TreeView.IsNodeExpanded(treeNode);
      }

      public static void ToggleExpandState(this IExplorerView explorerView, ITreeNode node)
      {
         explorerView.TreeView.ToggleExpandState(node);
      }
   }
}