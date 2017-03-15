using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IExplorerPresenter :
      IPresenterWithContextMenu<ITreeNode>,
      IPresenterWithContextMenu<IReadOnlyList<ITreeNode>>,
      IListener<ProjectCreatedEvent>,
      IListener<ProjectClosedEvent>

   {
      void NodeDoubleClicked(ITreeNode node);

      IEnumerable<ClassificationTemplate> AvailableClassificationCategories(ITreeNode<IClassification> parentClassificationNode);

      void AddToClassificationTree(ITreeNode<IClassification> parentNode, string category);

      /// <summary>
      ///    Removes the data defined under the classification. The classification itself is not removed.
      ///    Returns <c>true</c> if the data were indeed deleted otherwise <c>false</c> (for example if user canceled deletion)
      /// </summary>
      bool RemoveDataUnderClassification(ITreeNode<IClassification> classificationNode);

      /// <summary>
      ///    Removes the classification node. The data are not deleted
      /// </summary>
      /// <param name="nodeToRemove"></param>
      void RemoveClassification(ITreeNode<IClassification> nodeToRemove);

      /// <summary>
      ///    Removes all sub-classification nodes. if<paramref name="removeParent" /> is set to true, removes the node passed in
      ///    as well
      /// </summary>
      /// <param name="parentClassificationNode">
      ///    The node who's sub-classification nodes should be removed. Leafs remain and are attached to
      ///    the parent
      /// </param>
      /// <param name="removeParent">Specifes whether the parent node should be remove or not. Default is <c>false</c></param>
      /// <param name="removeData">Specifies whether the data should also be removed. Defaults is <c>false</c></param>
      void RemoveChildrenClassifications(ITreeNode<IClassification> parentClassificationNode, bool removeParent = false, bool removeData = false);

      /// <summary>
      ///    Returns the node whose <c>Id</c> is equal to the <paramref name="objectWithId" /> Id or null a node with this id was
      ///    not found
      /// </summary>
      ITreeNode NodeFor(IWithId objectWithId);

      /// <summary>
      ///    Starts the workflow allowing the user to add an arbitrary <see cref="IClassification" /> under the parent
      ///    <see cref="IClassification" />
      ///    represented by the <paramref name="parentClassificatonNode" />
      /// </summary>
      void CreateClassificationUnder(ITreeNode<IClassification> parentClassificatonNode);

      /// <summary>
      ///    Allows the user to rename the <see cref="IClassification" /> represented by the
      ///    <paramref name="classificationNode" />
      /// </summary>
      /// <param name="classificationNode"></param>
      void RenameClassification(ITreeNode<IClassification> classificationNode);

      /// <summary>
      ///    Removes all classifications defined in the projecct that have no <see cref="IClassifiable" /> children (direct or
      ///    indirect).
      /// </summary>
      void RemoveEmptyClassifcations();

      /// <summary>
      ///    returns true is the <paramref name="node" /> can be dragged otherwise false
      /// </summary>
      /// <param name="node">node to drag</param>
      bool CanDrag(ITreeNode node);

      /// <summary>
      ///    returns true is the <paramref name="dragNode" /> can be dragged under <paramref name="targetNode" /> otherwise false
      /// </summary>
      bool CanDrop(ITreeNode dragNode, ITreeNode targetNode);

      /// <summary>
      ///    Moves the <paramref name="dragNode" /> under the <paramref name="targetNode" />
      /// </summary>
      /// <param name="dragNode">Node being dragged</param>
      /// <param name="targetNode">Node under which the dragged node should be attached</param>
      void MoveNode(ITreeNode dragNode, ITreeNode targetNode);

      /// <summary>
      ///    Removes node from the view. The node won't be destroyed
      /// </summary>
      void RemoveNode(ITreeNode nodeToRemove);

      /// <summary>
      ///    Adds node to the view
      /// </summary>
      /// <param name="nodeToAdd"></param>
      void AddNode(ITreeNode nodeToAdd);

      /// <summary>
      ///    Removes the node from the view and destroy the node
      /// </summary>
      void DestroyNode(ITreeNode nodeToDestroy);

      /// <summary>
      ///    Returns the <see cref="RootNode" /> defined for the given <paramref name="rootNodeType" />
      /// </summary>
      RootNode NodeByType(RootNodeType rootNodeType);

      IEnumerable<ToolTipPart> ToolTipFor(ITreeNode node);

      /// <summary>
      ///    Node to show in the tree view
      /// </summary>
      /// <param name="treeNode"></param>
      void EnsureNodeVisible(ITreeNode treeNode);

      /// <summary>
      ///    Returns <c>true</c> if multiselect can be applied for the current node selection otherwise <c>false</c>
      /// </summary>
      bool AllowMultiSelectFor(IEnumerable<ITreeNode> selectedNodes);

      void RemoveNodeFor(IWithId objectWithId);
      void RemoveNodesFor(IEnumerable<IWithId> removedObjects);

      /// <summary>
      ///    Creates or retrieve a classifiable in the current project for the given <paramref name="subject" /> and adds it to
      ///    the tree
      ///    using the <paramref name="addClassifiableToTree" /> passed as parameter
      /// </summary>
      ITreeNode AddSubjectToClassifyToTree<TSubject, TClassifiable>(TSubject subject, Func<TClassifiable, ITreeNode> addClassifiableToTree)
         where TSubject : IWithId, IWithName
         where TClassifiable : Classifiable<TSubject>, new();

      /// <summary>
      ///    Adds a <see cref="IClassifiable" /> to the tree either under its parent if defined or under the node with type
      ///    <paramref name="rootNodeType" /> oterhwiseExpVoiew
      ///    and use the method <paramref name="addClassifiableToTree" /> to create the node
      /// </summary>
      ITreeNode AddClassifiableToTree<T>(T classifiable, RootNodeType rootNodeType, Func<ITreeNode<IClassification>, T, ITreeNode> addClassifiableToTree) where T : IClassifiable;

      /// <summary>
      ///    Addds the <paramref name="classifiableNode" /> under the <paramref name="classificationNode" /> if defined or
      ///    directly in the view otherwise
      /// </summary>
      void AddClassifiableNodeToView(ITreeNode classifiableNode, ITreeNode<IClassification> classificationNode = null);
   }

   public abstract class AbstractExplorerPresenter<TView, TPresenter> : AbstractPresenter<TView, TPresenter>, IExplorerPresenter
      where TView : IView<TPresenter>, IExplorerView
      where TPresenter : IExplorerPresenter
   {
      protected readonly IRegionResolver _regionResolver;
      private IRegion _region;
      protected readonly IToolTipPartCreator _toolTipPartCreator;
      private readonly RegionName _regionName;
      protected readonly IProjectRetriever _projectRetriever;
      protected readonly IClassificationPresenter _classificationPresenter;
      private bool _initialized;

      protected AbstractExplorerPresenter(TView view, IRegionResolver regionResolver,
         IClassificationPresenter classificationPresenter, IToolTipPartCreator toolTipPartCreator, RegionName regionName, IProjectRetriever projectRetriever)
         : base(view)
      {
         _regionResolver = regionResolver;
         _toolTipPartCreator = toolTipPartCreator;
         _regionName = regionName;
         _projectRetriever = projectRetriever;
         _classificationPresenter = classificationPresenter;
         _classificationPresenter.InitializeWith(this);
      }

      public abstract bool CanDrag(ITreeNode node);
      public abstract IEnumerable<ClassificationTemplate> AvailableClassificationCategories(ITreeNode<IClassification> parentClassificationNode);
      public abstract void AddToClassificationTree(ITreeNode<IClassification> parentNode, string category);
      public abstract bool RemoveDataUnderClassification(ITreeNode<IClassification> classificationNode);

      protected abstract void AddProjectToTree(IProject project);

      public virtual void CreateClassificationUnder(ITreeNode<IClassification> parentClassificatonNode)
      {
         if (parentClassificatonNode == null) return;
         _classificationPresenter.CreateClassificationFolderUnder(parentClassificatonNode);
      }

      public override void Initialize()
      {
         if (_initialized) return;
         _initialized = true;
         _region = _regionResolver.RegionWithName(_regionName);
         _region.Add(_view);
      }

      public virtual void RemoveNodeFor(IWithId objectWithId)
      {
         var node = NodeFor(objectWithId);
         if (node == null) return;
         _view.DestroyNode(node);
      }

      public virtual void RemoveNodesFor(IEnumerable<IWithId> removedObjects)
      {
         removedObjects.Each(RemoveNodeFor);
      }

      public virtual void ToggleVisibility()
      {
         _region.ToggleVisibility();
      }

      protected virtual bool IsFolderNode(ITreeNode node)
      {
         return node.IsAnImplementationOf<RootNode>() || node.IsAnImplementationOf<ClassificationNode>();
      }

      public virtual ITreeNode NodeFor(IWithId objectWithId)
      {
         return objectWithId != null ? _view.NodeById(objectWithId.Id) : null;
      }

      public virtual IEnumerable<ToolTipPart> ToolTipFor(ITreeNode node)
      {
         return _toolTipPartCreator.ToolTipFor(node.TagAsObject);
      }

      public virtual void EnsureNodeVisible(ITreeNode treeNode)
      {
         if (treeNode == null) return;
         _view.ExpandNode(treeNode);
         EnsureNodeVisible(treeNode.ParentNode);
      }

      public virtual bool AllowMultiSelectFor(IEnumerable<ITreeNode> selectedNodes)
      {
         return selectedNodes.Where(x => x != null)
            .Select(x => x.GetType())
            .Distinct()
            .Count() == 1;
      }

      public abstract void ShowContextMenu(ITreeNode node, Point popupLocation);
      public abstract void ShowContextMenu(IReadOnlyList<ITreeNode> treeNodes, Point popupLocation);

      public virtual void NodeDoubleClicked(ITreeNode node)
      {
         if (IsFolderNode(node))
            View.ToggleExpandState(node);
      }

      /// <summary>
      ///    Removes a classification tree node from the view
      /// </summary>
      /// <param name="nodeToRemove">The classification node to remove</param>
      public virtual void RemoveClassification(ITreeNode<IClassification> nodeToRemove)
      {
         _classificationPresenter.RemoveClassification(nodeToRemove);
      }

      public virtual void RemoveChildrenClassifications(ITreeNode<IClassification> parentClassificationNode, bool removeParent = false, bool removeData = false)
      {
         if (removeData)
         {
            if (!RemoveDataUnderClassification(parentClassificationNode))
               return;
         }

         _classificationPresenter.RemoveChildrenClassifications(parentClassificationNode);

         if (removeParent)
            RemoveClassification(parentClassificationNode);
      }
      public virtual ITreeNode AddSubjectToClassifyToTree<TSubject, TClassifiable>(TSubject subject, Func<TClassifiable, ITreeNode> addClassifiableToTree)
         where TSubject : IWithId, IWithName
         where TClassifiable : Classifiable<TSubject>, new()
      {
         var project = _projectRetriever.CurrentProject;
         var classifiableSubject = project.GetOrCreateClassifiableFor<TClassifiable, TSubject>(subject);

         return addClassifiableToTree(classifiableSubject);
      }

      public virtual ITreeNode AddClassifiableToTree<T>(T classifiable, RootNodeType rootNodeType, Func<ITreeNode<IClassification>, T, ITreeNode> addClassifiableToTree) where T : IClassifiable
      {
         var parent = classifiable.Parent ?? rootNodeType;
         var parentNode = NodeFor(parent).DowncastTo<ITreeNode<IClassification>>();
         return addClassifiableToTree(parentNode, classifiable);
      }

      public virtual void AddClassifiableNodeToView(ITreeNode classifiableNode, ITreeNode<IClassification> classificationNode = null)
      {
         if (classificationNode != null)
         {
            classificationNode.AddChild(classifiableNode);
            _view.AddNode(classificationNode);
         }
         else
            _view.AddNode(classifiableNode);
      }

      public virtual void RenameClassification(ITreeNode<IClassification> classificationNode)
      {
         _classificationPresenter.RenameClassification(classificationNode);
      }

      public virtual void RemoveEmptyClassifcations()
      {
         _classificationPresenter.RemoveEmptyClassifcations();
      }

      public virtual void MoveNode(ITreeNode dragNode, ITreeNode targetNode)
      {
         var classificationNode = targetNode as ITreeNode<IClassification>;
         var classifiableNode = dragNode as ITreeNode<IClassifiable>;
         _classificationPresenter.MoveNode(classifiableNode, classificationNode);
      }

      public virtual void RemoveNode(ITreeNode nodeToRemove)
      {
         _view.RemoveNode(nodeToRemove);
      }

      public virtual void AddNode(ITreeNode nodeToAdd)
      {
         _view.AddNode(nodeToAdd);
      }

      public virtual RootNode NodeByType(RootNodeType rootNodeType)
      {
         return _view.NodeByType(rootNodeType).DowncastTo<RootNode>();
      }

      public virtual void DestroyNode(ITreeNode nodeToDestroy)
      {
         _view.DestroyNode(nodeToDestroy);
      }

      public virtual bool CanDrop(ITreeNode dragNode, ITreeNode targetNode)
      {
         var targetClassificationNode = targetNode as ITreeNode<IClassification>;
         if (targetClassificationNode == null)
            return false;

         var classifiableNode = dragNode as ITreeNode<IClassifiable>;
         if (classifiableNode == null)
            return false;

         if (Equals(classifiableNode.ParentNode, targetClassificationNode))
            return false;

         var dragClassifcationNode = classifiableNode as ITreeNode<IClassification>;
         if (dragClassifcationNode == null)
            return CanDrop(classifiableNode, targetClassificationNode);

         return _classificationPresenter.CanMove(dragClassifcationNode, targetClassificationNode);
      }

      protected virtual bool CanDrop(ITreeNode<IClassifiable> classifiableNode, ITreeNode<IClassification> classificationNode)
      {
         return classificationNode.Tag.ClassificationType == classifiableNode.Tag.ClassificationType;
      }

      public virtual void Handle(ProjectCreatedEvent eventToHandle)
      {
         AddProjectToTree(eventToHandle.Project);
      }

      public virtual void Handle(ProjectClosedEvent eventToHandle)
      {
         _view.DestroyNodes();
      }
   }
}