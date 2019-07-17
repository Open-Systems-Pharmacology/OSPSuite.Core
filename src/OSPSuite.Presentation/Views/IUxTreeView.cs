using System;
using System.Collections.Generic;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Views
{
   public class NodeChangingEventArgs : EventArgs
   {
      public NodeChangingEventArgs(ITreeNode node, ITreeNode oldNode)
      {
         Node = node;
         OldNode = oldNode;
      }

      public ITreeNode Node { get; }
      public ITreeNode OldNode { get; }
      public bool Cancel { get; set; }
   }

   public interface IUxTreeView
   {
      /// <summary>
      ///    Event occurs when a node is doubles click
      /// </summary>
      event Action<ITreeNode> NodeDoubleClick;

      /// <summary>
      ///    Event occurs when the selected node has changed, either programatically, with a keystroke or with a mouse click
      /// </summary>
      event Action<ITreeNode> SelectedNodeChanged;

      /// <summary>
      ///    Event occurs before the selected is changed, either programatically, with a keystroke or with a mouse click, thus
      ///    allowing to cancel
      ///    the action if necessary.
      ///    The Event SelectedNodeChanged is fired if the action was not canceled.
      /// </summary>
      event Action<NodeChangingEventArgs> SelectedNodeChanging;

      /// <summary>
      ///    Remove all nodes from the tree and delete all nodes as well
      /// </summary>
      void DestroyAllNodes();

      /// <summary>
      ///    Remove all nodes from the tree. The nodes will not be deleted.
      /// </summary>
      void Clear();

      /// <summary>
      ///    Add the node and all its sub node to the tree
      /// </summary>
      /// <param name="nodeToAdd">node to add</param>
      void AddNode(ITreeNode nodeToAdd);

      /// <summary>
      ///    Retrieve the node with the given id
      /// </summary>
      /// <param name="id">if of the node to retrieve</param>
      ITreeNode NodeById(string id);

      /// <summary>
      ///    Remove node from the tree and delete all sub nodes and reference to tag
      /// </summary>
      /// <param name="treeNode">node to destroy</param>
      void DestroyNode(ITreeNode treeNode);

      /// <summary>
      ///    Destroy node by id
      /// </summary>
      /// <param name="id">id of the node to destroy</param>
      void DestroyNode(string id);

      /// <summary>
      ///    Remove a node from the tree. The node will not be destroyed. Simply removed from the tree
      /// </summary>
      /// <param name="treeNode">The node to be removed</param>
      void RemoveNode(ITreeNode treeNode);

      /// <summary>
      ///    Expand the node given as parameter
      /// </summary>
      void ExpandNode(ITreeNode nodeToExpand);

      /// <summary>
      ///    Collapse the node given as parameter
      /// </summary>
      void CollapseNode(ITreeNode nodeToCollapse);

      /// <summary>
      ///    Collapse the <paramref name="node" /> if it is expanded or expand it if is collapsed
      /// </summary>
      /// <param name="node"></param>
      void ToggleExpandState(ITreeNode node);

      /// <summary>
      ///    Selects the first node of the tree if none was selected otherwise reselect the former selected node (in order to
      ///    raise events)
      /// </summary>
      void SelectFocusedNodeOrFirst();

      /// <summary>
      ///    Selects the given node of the tree if the node exists, otherwise call the default select node method
      /// </summary>
      void SelectNode(ITreeNode nodeToSelect);

      /// <summary>
      ///    returns the selected node if one node was selected, or the first node of a multi selection if more than one node is
      ///    selected
      ///    returns null if no node is selected
      /// </summary>
      ITreeNode SelectedNode { get; }

      /// <summary>
      ///    should the nodes be expanded when created?
      /// </summary>
      bool ShouldExpandAddedNode { set; }

      /// <summary>
      ///    Gets or sets a value indicating whether to use lazy loading.
      /// </summary>
      /// <value>
      ///    <c>true</c> if using lazy loading; otherwise, <c>false</c>.
      ///    default: <c>false</c>
      /// </value>
      /// <remarks> Ensure that the used <see cref="ITreeNode" /> also supports lazy loading</remarks>
      bool UseLazyLoading { get; set; }

      /// <summary>
      ///    Expand all nodes
      /// </summary>
      void ExpandAllNodes();

      /// <summary>
      ///    Function handler that retrieves the tool tip for a given node. Per default the handler returns
      ///    node.ToolTip. Settings this method is a great way to generate node toop til dynamically
      /// </summary>
      Func<ITreeNode, IEnumerable<ToolTipPart>> ToolTipForNode { get; set; }

      /// <summary>
      ///    Returns <c>true</c> if <paramref name="treeNode" /> is expanded otherwise <c>false</c>
      /// </summary>
      bool IsNodeExpanded(ITreeNode treeNode);
   }
}