using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Nodes
{
   public interface ITreeNode
   {
      /// <summary>
      /// id of the node
      /// </summary>
      string Id { get; }

      /// <summary>
      /// The text displayed in the node
      /// </summary>
      string Text { get; set; }

      /// <summary>
      /// The color of the text displayed in the node. If not set, the default color will be used
      /// </summary>
      Color ForeColor { get; set; } 

      /// <summary>
      /// Returns the children node of the current node
      /// </summary>
      IEnumerable<ITreeNode> Children { get;  }

      /// <summary>
      /// Add a child node. If the node already exists, it won't be added to the parent again
      /// </summary>
      /// <param name="childNode">Node to add as a child</param>
      void AddChild(ITreeNode childNode);

      /// <summary>
      /// Remove the node given as parameter from the children
      /// </summary>
      /// <param name="childNode">Child node to remove</param>
      void RemoveChild(ITreeNode childNode);

      /// <summary>
      /// Return the parent node of the current node
      /// Set should only be used in the AddChild Method
      /// </summary>
      ITreeNode ParentNode { get; set; }

      /// <summary>
      /// Returns the top node of the hierarchy
      /// </summary>
      ITreeNode RootNode { get; }

      /// <summary>
      /// Icon for the node
      /// </summary>
      ApplicationIcon Icon { get; set; }

      /// <summary>
      /// Event is fired whenever the text was set
      /// </summary>
      event Action<ITreeNode> TextChanged;

      /// <summary>
      /// Event is fired whenever the icon was set
      /// </summary>
      event Action<ITreeNode> IconChanged;

      /// <summary>
      /// Event is fired whenever the text color was set
      /// </summary>
      event Action<ITreeNode> ForeColorChanged;

      /// <summary>
      /// Delete the node (remove the node and all its sub nodes recursively)
      /// </summary>
      void Delete();

      /// <summary>
      /// Returnts the underlying tag 
      /// </summary>
      object TagAsObject { get; }

      /// <summary>
      /// Returns all leaf nodes starting from the current node. If the node is a leaf itself,
      /// only the nod will be returned. Hence he returned enumerable always contains at least one element
      /// </summary>
      IEnumerable<ITreeNode> AllLeafNodes { get; }

      /// <summary>
      /// Returns all nodes starting from the current node (node and leaf)
      /// The returned enumerable will always contains at least one element (the node itself)
      /// </summary>
      IEnumerable<ITreeNode> AllNodes { get; }

      /// <summary>
      /// Returns true if the node has children otherwise false
      /// </summary>
      bool HasChildren { get; }

      /// <summary>
      /// Returns the full path of the current node in the hierarchy using the "\" delimiter
      /// </summary>
      string FullPath();

      /// <summary>
      /// Returns the full path of the current node in the hierarchy using the given <paramref name="delimiter"/>
      /// </summary>
      string FullPath(string delimiter);

      /// <summary>
      /// Hint that will be displayed on mouse over
      /// </summary>
      IList<ToolTipPart> ToolTip { get; set; }

      /// <summary>
      /// Add a tool tip aprt to the tool tip
      /// </summary>
      /// <param name="toolTipPart"></param>
      void AddToolTipPart(ToolTipPart toolTipPart);
   }

   public interface ITreeNode<out TObjectType> : ITreeNode
   {
      TObjectType Tag { get; }
   }
}