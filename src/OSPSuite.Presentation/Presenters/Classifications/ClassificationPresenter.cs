using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters.Classifications
{
   /// <summary>
   ///    Represents a presenter that acts on behalf of one <see cref="IExplorerPresenter" /> and manages the classification
   ///    states
   /// </summary>
   public interface IClassificationPresenter
   {
      /// <summary>
      ///    Passes the <paramref name="explorerPresenter" /> whose classification will be managed.
      /// </summary>
      /// <param name="explorerPresenter"></param>
      void InitializeWith(IExplorerPresenter explorerPresenter);

      /// <summary>
      ///    Removes the classification node from the <see cref="IExplorerPresenter" /> and synchronizes the project by removing
      ///    the corresponding <see cref="IClassification" />
      /// </summary>
      /// <param name="classificationNodeToRemove">Node representing the classification to remove</param>
      void RemoveClassification(ITreeNode<IClassification> classificationNodeToRemove);

      void RemoveChildrenClassifications(ITreeNode<IClassification> parentClassificationNode);

      /// <summary>
      ///    Adds the given <paramref name="classifications" /> to the <see cref="IExplorerPresenter" />. The project will not be
      ///    affected by this changes
      /// </summary>
      /// <param name="classifications">Classificaitons to add to the <see cref="IExplorerPresenter" /></param>
      void AddClassificationsToTree(IReadOnlyCollection<IClassification> classifications);

      /// <summary>
      ///    Removes all classification nodes defined in the <see cref="IExplorerPresenter" />  that do not have any
      ///    <see cref="IClassifiable" /> children (e.g. empty folder structure).
      ///    The corresponding  <see cref="IClassification" /> will be removed from the <see cref="IProject" />
      /// </summary>
      void RemoveEmptyClassifcations();

      void CreateClassificationFolderUnder(ITreeNode<IClassification> parentClassificatonNode);

      /// <summary>
      ///    Renames the <see cref="IClassification" /> associated to the <paramref name="classificationNode" />.
      /// </summary>
      /// <param name="classificationNode">The node representing the  <see cref="IClassification" /> to be renamed</param>
      void RenameClassification(ITreeNode<IClassification> classificationNode);

      /// <summary>
      ///    Takes all the <see cref="IClassifiable" /> of type <typeparamref name="TClassifiable" /> defined under the parent
      ///    <see cref="IClassification" /> represented by the <paramref name="parentNode" />
      ///    and categorizes them according to their value for the <paramref name="category" />.
      /// </summary>
      /// <typeparam name="TClassifiable">Type of classifiable to categorize</typeparam>
      /// <param name="parentNode">
      ///    Classification node under which the <see cref="IClassifiable" /> to categorize are directly
      ///    attached. If <c>null</c> the orphan <see cref="IClassifiable" /> defined in the project will be used instead
      /// </param>
      /// <param name="category">Category used to create classification (e.g. Individual, Compound)</param>
      /// <param name="retrieveClassificationNameFromCategory">
      ///    Function retrieving the value correspondign to the
      ///    <paramref name="category" /> for one specific <see cref="IClassifiable" /> instance (e.g. For category Individual,
      ///    Ind1)
      /// </param>
      void GroupClassificationsByCategory<TClassifiable>(
         ITreeNode<IClassification> parentNode,
         string category,
         Func<TClassifiable, string> retrieveClassificationNameFromCategory) where TClassifiable : class, IClassifiable;

      /// <summary>
      ///    Moves the <see cref="IClassifiable" /> represented by <paramref name="classifiableNode" /> under the
      ///    <see cref="IClassification" /> represented by
      ///    <paramref name="classificationNode" />
      /// </summary>
      void MoveNode(ITreeNode<IClassifiable> classifiableNode, ITreeNode<IClassification> classificationNode);

      /// <summary>
      ///    Returns true if <paramref name="classificationNodeToBeMoved" /> can be moved under
      ///    <paramref name="targetClassificationNode" /> otherwise false
      /// </summary>
      bool CanMove(ITreeNode<IClassification> classificationNodeToBeMoved, ITreeNode<IClassification> targetClassificationNode);
   }

   public class ClassificationPresenter : IClassificationPresenter
   {
      private readonly IClassificationTypeToRootNodeTypeMapper _rootNodeTypeMapper;
      private readonly IApplicationController _applicationController;
      private readonly IProjectRetriever _projectRetriever;
      private IExplorerPresenter _explorerPresenter;

      public ClassificationPresenter(IClassificationTypeToRootNodeTypeMapper rootNodeTypeMapper,
         IApplicationController applicationController, IProjectRetriever projectRetriever)
      {
         _rootNodeTypeMapper = rootNodeTypeMapper;
         _applicationController = applicationController;
         _projectRetriever = projectRetriever;
      }

      public void InitializeWith(IExplorerPresenter explorerPresenter)
      {
         _explorerPresenter = explorerPresenter;
      }

      public virtual void RemoveChildrenClassifications(ITreeNode<IClassification> parentClassificationNode)
      {
         parentClassificationNode.Children<ITreeNode<IClassification>>().Each(c => removeClassification(c, recursive: true));
      }

      public virtual void RemoveClassification(ITreeNode<IClassification> classificationNodeToRemove)
      {
         removeClassification(classificationNodeToRemove, recursive: false);
      }

      private void removeClassification(ITreeNode<IClassification> classificationNodeToRemove, bool recursive)
      {
         if (classificationNodeToRemove == null) return;

         if (recursive)
            classificationNodeToRemove.NodesWithTags<IClassification>().Each(c => removeClassification(c, recursive: true));

         var project = _projectRetriever.CurrentProject;
         var classificationToBeRemoved = classificationNodeToRemove.Tag;
         _explorerPresenter.RemoveNode(classificationNodeToRemove);
         project.RemoveClassification(classificationToBeRemoved);

         // Promote all children nodes
         classificationNodeToRemove.NodesWithTags<IClassifiable>().Each(childNode => promoteNode(childNode, classificationNodeToRemove));

         var parentNode = classificationNodeToRemove.ParentNode;
         if (parentNode != null)
            parentNode.RemoveChild(classificationNodeToRemove);

         classificationNodeToRemove.Delete();
      }

      private void promoteNode(ITreeNode<IClassifiable> nodeToPromote, ITreeNode<IClassification> nodeToRemove)
      {
         var parentClassification = nodeToRemove.Tag.Parent;
         var classifiableToPromote = nodeToPromote.Tag;
         var parentNode = nodeToRemove.ParentNode;
         SetParentClassificationIfRequired(parentClassification, classifiableToPromote);

         // In case removing this node will result in the newly promoted nodes having the same path as an existing node, we will move children and delete instead
         moveChildrenToEquivalentNode(nodeToPromote, nodeToRemove);

         // If the item being promoted is a classification type but has nothing within to classify, we can remove the tag from the project
         if (classifiableToPromote.IsAnImplementationOf<IClassification>() && !nodeToPromote.HasChildren)
         {
            _projectRetriever.CurrentProject.RemoveClassification(classifiableToPromote.DowncastTo<IClassification>());
            return;
         }

         nodeToRemove.RemoveChild(nodeToPromote);

         if (parentNode != null)
            parentNode.AddChild(nodeToPromote);

         _explorerPresenter.AddNode(nodeToPromote);
      }

      protected static void SetParentClassificationIfRequired<T>(IClassification parentClassification, T classifiableToPromote) where T : IClassifiable
      {
         classifiableToPromote.Parent = parentClassification.IsAnImplementationOf<RootNodeType>() ? null : parentClassification;
      }

      private RootNode rootNodeFor(IClassification classification)
      {
         return _explorerPresenter.NodeByType(_rootNodeTypeMapper.MapFrom(classification.ClassificationType));
      }

      public void AddClassificationsToTree(IReadOnlyCollection<IClassification> classifications)
      {
         var allParents = classifications.Select(x => x.Parent).ToList();
         var allLeafs = classifications.Where(x => !allParents.Contains(x));

         //Loading a classification structure: We need to add the root node as well
         allLeafs.Each(n => addClassificationToTree(n, alsoAddRoot: true));
      }

      private ITreeNode addClassificationToTree(IClassification classification, bool alsoAddRoot = true)
      {
         var classificationNode = createClassificationNodeFor(classification);
         if (alsoAddRoot)
            _explorerPresenter.AddNode(classificationNode.RootNode);

         _explorerPresenter.AddNode(classificationNode);

         return classificationNode;
      }

      public void RemoveEmptyClassifcations()
      {
         List<ITreeNode<IClassification>> emptyClassificationNodes;
         do
         {
            emptyClassificationNodes = _projectRetriever.CurrentProject.AllClassifications
               .Select(classificationNodeFor)
               .Where(c => c != null && !c.HasChildren).ToList();

            emptyClassificationNodes.Each(eraseFromProject);
         } while (emptyClassificationNodes.Count != 0);
      }

      private void eraseFromProject(ITreeNode<IClassification> classificationNode)
      {
         _projectRetriever.CurrentProject.RemoveClassification(classificationNode.Tag);
         _explorerPresenter.DestroyNode(classificationNode);
      }

      public virtual void GroupClassificationsByCategory<TClassifiable>(ITreeNode<IClassification> parentNode, string category,
         Func<TClassifiable, string> retrieveClassificationNameFromCategory)
         where TClassifiable : class, IClassifiable
      {
         var classifiablesToClassify = parentNode.NodesWithTags<TClassifiable>().Select(x => x.Tag).ToList();
         var parentClassification = parentNode.Tag;

         classifiablesToClassify.Each(classifiable =>
         {
            var classificationName = retrieveClassificationNameFromCategory(classifiable);
            if (string.IsNullOrEmpty(classificationName)) return;

            //remove the classifiable node from the view
            var classifiableNode = removeClassifiableNodeFromTree(classifiable);

            //creates the classification
            var classification = createClassificationUnder(parentClassification, classificationName);
            var classificationNode = createClassificationNodeFor(classification);

            //move the classifiable under the new classification
            classifiable.Parent = classification;
            classificationNode.AddChild(classifiableNode);
            _explorerPresenter.AddNode(classificationNode);
         });
      }

      public void MoveNode(ITreeNode<IClassifiable> classifiableNode, ITreeNode<IClassification> classificationNode)
      {
         if (classifiableNode == null || classificationNode == null) return;

         var parentClassificationNode = classifiableNode.ParentNode;
         SetParentClassificationIfRequired(classificationNode.Tag, classifiableNode.Tag);

         if (parentClassificationNode != null)
            parentClassificationNode.RemoveChild(classifiableNode);

         _explorerPresenter.RemoveNode(classifiableNode);
         classificationNode.AddChild(classifiableNode);
         _explorerPresenter.AddNode(classifiableNode);
      }

      public bool CanMove(ITreeNode<IClassification> classificationNodeToBeMoved, ITreeNode<IClassification> targetClassificationNode)
      {
         if (Equals(classificationNodeToBeMoved, targetClassificationNode))
            return false;

         if (targetClassificationNode.HasAncestor(classificationNodeToBeMoved))
            return false;

         //source and target are classification node. 
         if (targetClassificationNode.Children.Select(x => x.Text).Contains(classificationNodeToBeMoved.Text))
            return false;

         return targetClassificationNode.Tag.ClassificationType == classificationNodeToBeMoved.Tag.ClassificationType;
      }

      private ITreeNode removeClassifiableNodeFromTree<TClassifiable>(TClassifiable classifiable) where TClassifiable : class, IClassifiable
      {
         var classifiableNode = _explorerPresenter.NodeFor(classifiable);
         _explorerPresenter.RemoveNode(classifiableNode);
         var parent = classifiableNode.ParentNode;
         if (parent != null)
            parent.RemoveChild(classifiableNode);

         return classifiableNode;
      }

      private IClassification createClassificationUnder(IClassification parentClassification, string name)
      {
         var project = _projectRetriever.CurrentProject;
         var classification = project.GetOrCreateByPath(parentClassification, name, parentClassification.ClassificationType);
         SetParentClassificationIfRequired(parentClassification, classification);
         return classification;
      }

      /// <summary>
      ///    Recurisively creates a node up to the top most ancestor of  for the given <paramref name="classification" />.
      /// </summary>
      /// <remarks>The created nodes will not be added to the view! This is the responsability of the caller</remarks>
      private ITreeNode<IClassification> createClassificationNodeFor(IClassification classification)
      {
         if (classification == null)
            return null;

         var parentNode = createClassificationNodeFor(classification.Parent);
         parentNode = parentNode ?? rootNodeFor(classification);

         return createFor(classification)
            .Under(parentNode);
      }

      public void CreateClassificationFolderUnder(ITreeNode<IClassification> parentClassificatonNode)
      {
         var parentClassification = parentClassificatonNode.Tag;

         using (var nameClassificationPresenter = _applicationController.Start<INameClassificationPresenter>())
         {
            if (!nameClassificationPresenter.NewName(parentClassification))
               return;

            var classification = createClassificationUnder(parentClassification, nameClassificationPresenter.Name);
            //just adding a node to an existing node: no need to add the whole treee hierarchy
            var classificationNode = addClassificationToTree(classification, alsoAddRoot: false);
            _explorerPresenter.EnsureNodeVisible(classificationNode);
         }
      }

      public void RenameClassification(ITreeNode<IClassification> classificationNode)
      {
         var classification = classificationNode.Tag;
         using (var nameClassificationPresenter = _applicationController.Start<IRenameClassificationPresenter>())
         {
            if (!nameClassificationPresenter.Rename(classification))
               return;

            classification.Name = nameClassificationPresenter.Name;
         }
      }

      /// <summary>
      ///    Moves children from one node to another
      /// </summary>
      /// <param name="nodeToHaveChildrenRemoved">The node that will have all children removed</param>
      /// <param name="nodeBeingRemoved">The node being removed when the method is being called</param>
      /// <returns>An List of all the child nodes that were moved during the operation</returns>
      private void moveChildrenToEquivalentNode(ITreeNode nodeToHaveChildrenRemoved, ITreeNode<IClassification> nodeBeingRemoved)
      {
         var classifiableToPromote = nodeToHaveChildrenRemoved.TagAsObject as IClassification;
         if (classifiableToPromote == null)
            return;

         //this is the node under which the sub nodes will be moved 
         var nodeToHaveChildrenAdded = equivalentNodes(classifiableToPromote)
            .FirstOrDefault(x => !x.Id.Equals(classifiableToPromote.Id));

         //No equivalent node found or the only one found is the one being removed. 
         if (nodeToHaveChildrenAdded == null || Equals(nodeToHaveChildrenAdded, nodeBeingRemoved))
            return;

         nodeToHaveChildrenRemoved.NodesWithTags<IClassifiable>().Each(child => MoveNode(child, nodeToHaveChildrenAdded));

         //now that all children haven been removed, we can safely remove the parent node
         _explorerPresenter.RemoveNode(nodeToHaveChildrenRemoved);
      }

      /// <summary>
      ///    Creates a classification node for the given <paramref name="classification" />
      /// </summary>
      /// <returns>A <see cref="ClassificationNode" /> that can be added to the tree</returns>
      private ITreeNode<IClassification> createFor(IClassification classification)
      {
         var classificationNode = existingEquivalent(classification);

         if (classificationNode != null)
            return classificationNode;

         return new ClassificationNode(classification) { Icon = ApplicationIcons.Folder };
      }

      /// <summary>
      ///    Lists all nodes that currently have the same classification path as the parameter.
      /// </summary>
      /// <param name="classification">The comparison classification</param>
      /// <returns>A Enumeration of all the current nodes with the same path as the classification argument</returns>
      private IEnumerable<ITreeNode<IClassification>> equivalentNodes(IClassification classification)
      {
         var project = _projectRetriever.CurrentProject;
         var equivalentClassifications = project.AllClassifications
            .Where(c => c.HasEquivalentClassification(classification));

         return equivalentClassifications.Select(classificationNodeFor).Where(n => n != null);
      }

      private ITreeNode<IClassification> existingEquivalent(IClassification classification)
      {
         return equivalentNodes(classification).FirstOrDefault();
      }

      /// <summary>
      ///    Returns the defined node for the <paramref name="classification" /> or null no done was defined for the given
      ///    <paramref name="classification" />
      /// </summary>
      private ITreeNode<IClassification> classificationNodeFor(IClassification classification)
      {
         return _explorerPresenter.NodeFor(classification) as ITreeNode<IClassification>;
      }
   }
}