using System.Collections.Generic;
using System.Drawing;

namespace OSPSuite.Core.Diagram
{
   public interface IDiagramModel : IContainerBase
   {
      /// <summary>
      ///    Gets a node based on its id
      /// </summary>
      /// <param name="id">The id of the node being searched for</param>
      /// <returns>A node with the <paramref name="id" /> if it's found.</returns>
      IBaseNode GetNode(string id);

      T GetNode<T>(string id) where T : class, IBaseNode;
      T CreateNode<T>(string id, PointF location, IContainerBase parentContainerBase) where T : class, IBaseNode, new();

      /// <summary>
      ///    Removes a node from the model
      /// </summary>
      /// <param name="id">The id of the node to be removed</param>
      void RemoveNode(string id);

      /// <summary>
      ///    Renames the node to <paramref name="name" /> if the node with <paramref name="id" /> can be found
      /// </summary>
      /// <param name="id">The id of the node being renamed</param>
      /// <param name="name">The new name</param>
      void RenameNode(string id, string name);

      /// <summary>
      ///    Create a copy of the given <see cref="IDiagramModel" /> if the parameter <paramref name="containerId" /> is not
      ///    defined
      ///    (default). Otherwise, create a new <see cref="IDiagramModel" /> containing a copy of the container with id
      ///    <paramref name="containerId" />
      /// </summary>
      IDiagramModel CreateCopy(string containerId = null);

      /// <summary>
      ///    Changes node id's when found in the model
      /// </summary>
      /// <param name="changedIds">
      ///    The dictionary contains the existing node id as key and the new node id as value. If the node
      ///    for key id is found, it's id is changed to value id
      /// </param>
      void ReplaceNodeIds(IDictionary<string, string> changedIds);

      bool IsEmpty();

      /// <summary>
      ///    Set if the diagram has already been layed out once. Prevents layout from moving all the nodes when layout is called
      /// </summary>
      bool IsLayouted { get; set; }

      IDiagramOptions DiagramOptions { get; set; }
      void Clear();
      void SetDefaultExpansion();
      void RefreshSize();
      void Undo();
      void ClearUndoStack();
      void BeginUpdate();
      void EndUpdate();
      bool StartTransaction();
      bool FinishTransaction(string layoutrecursivedone);
      void ShowDefaultExpansion();
      IBaseNode FindByName(string name);
      void AddNodeId(IBaseNode baseNode);

      /// <summary>
      /// Returns a new instance of the diagram model
      /// </summary>
      /// <returns></returns>
      IDiagramModel Create();
   }
}