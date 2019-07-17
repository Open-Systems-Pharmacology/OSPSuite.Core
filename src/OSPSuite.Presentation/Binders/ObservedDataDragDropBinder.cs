using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Binders
{
   public class ObservedDataDragDropBinder
   {
      private readonly Type[] _conditionalDraggableTypes = {typeof(IEnumerable<ITreeNode>)};

      public void PrepareDrag(IDragEvent e)
      {
         e.Effect = _conditionalDraggableTypes.Any(e.TypeBeingDraggedIs) ? dragEffectForConditionalType(e) : DragEffect.None;
      }

      public IReadOnlyList<DataRepository> DroppedObservedDataFrom(IDragEvent e)
      {
         return getObservedDataNodesFrom(e).Select(observedDataNode => observedDataNode.Tag.Subject).ToList();
      }

      private DragEffect dragEffectForConditionalType(IDragEvent e)
      {
         return isDraggedTypeObservedDataRelated(e.Data<IEnumerable<ITreeNode>>()) ? DragEffect.Move : DragEffect.None;
      }

      private bool isDraggedTypeObservedDataRelated(IEnumerable<ITreeNode> multipleNodes)
      {
         if (multipleNodes == null) return false;

         var treeNodes = multipleNodes as IList<ITreeNode> ?? multipleNodes.ToList();
         return areAllObservedDataClassificationNodes(treeNodes) || areAllObservedDataNodes(treeNodes) || areAllRootNodes(treeNodes);
      }

      private bool areAllRootNodes(IList<ITreeNode> treeNodes)
      {
         return treeNodes.OfType<RootNode>().Count(rootNode => rootNode.Tag.ClassificationType == ClassificationType.ObservedData) == treeNodes.Count();
      }

      private bool areAllObservedDataNodes(IEnumerable<ITreeNode> treeNodes)
      {
         return treeNodes.All(node => node.IsAnImplementationOf<ObservedDataNode>());
      }

      private bool areAllObservedDataClassificationNodes(IList<ITreeNode> treeNodes)
      {
         return treeNodes.OfType<ClassificationNode>().Count(classificationNode => (classificationNode).Tag.ClassificationType == ClassificationType.ObservedData) == treeNodes.Count();
      }

      private IReadOnlyList<ObservedDataNode> getObservedDataNodesFrom(IDragEvent e)
      {
         var dataNodes = e.Data<IEnumerable<ITreeNode>>();
         if (dataNodes == null)
            return new List<ObservedDataNode>();

         var treeNodes = dataNodes as IList<ITreeNode> ?? dataNodes.ToList();
         if (areAllObservedDataNodes(treeNodes))
            return observedDataNodesFromObservedDataNodes(treeNodes);

         if (areAllObservedDataClassificationNodes(treeNodes) || areAllRootNodes(treeNodes))
            return observedDataNodesFromClassificationNodes(treeNodes);

         return new List<ObservedDataNode>();
      }

      private IReadOnlyList<ObservedDataNode> observedDataNodesFromClassificationNodes(IEnumerable<ITreeNode> treeNodes)
      {
         var observedDataList = new List<ObservedDataNode>();
         treeNodes.Each(treeNode =>
         {
            var classificationNode = getClassificationNodeFrom(treeNode);
            if (classificationNode != null)
               observedDataList.AddRange(classificationNode.AllLeafNodes.OfType<ObservedDataNode>());
         });
         return observedDataList;
      }

      private static IReadOnlyList<ObservedDataNode> observedDataNodesFromObservedDataNodes(IEnumerable<ITreeNode> treeNodes)
      {
         var observedDataList = new List<ObservedDataNode>();
         treeNodes.Each(node =>
         {
            var observedDataNode = node as ObservedDataNode;
            if (observedDataNode != null)
               observedDataList.Add(observedDataNode);
         });
         return observedDataList;
      }

      private ITreeNode<IClassification> getClassificationNodeFrom(ITreeNode treeNode)
      {
         var classificationNode = treeNode as ClassificationNode;
         if (classificationNode != null)
            return classificationNode;

         var rootNode = treeNode as RootNode;
         return rootNode;
      }
   }
}