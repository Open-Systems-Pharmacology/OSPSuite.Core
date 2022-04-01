using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IObservedDataInExplorerPresenter :
      IListener<ObservedDataAddedEvent>,
      IListener<ObservedDataRemovedEvent>
   {
      /// <summary>
      ///    Returns all possible <see cref="ClassificationTemplate" /> defined for observed data under the given
      ///    <paramref name="parentClassificationNode" />
      /// </summary>
      IEnumerable<ClassificationTemplate> AvailableObservedDataCategoriesIn(ITreeNode<IClassification> parentClassificationNode);

      /// <summary>
      ///    Intializes the current object with the required references to perform its tasks. This should be done only once
      /// </summary>
      /// <param name="explorerPresenter">Explorer Presenter for which observed data should be managed</param>
      /// <param name="classificationPresenter">
      ///    The Classification Presenter defined in the <paramref name="explorerPresenter" />
      ///    .
      /// </param>
      /// <param name="observedDataFolder">
      ///    The application specific <see cref="RootNodeType" /> representing the observed data
      ///    folder
      /// </param>
      void InitializeWith(IExplorerPresenter explorerPresenter, IClassificationPresenter classificationPresenter, RootNodeType observedDataFolder);

      /// <summary>
      ///    Add all observed data defined in <paramref name="project" /> to the explorer
      /// </summary>
      void AddObservedDataToTree(IProject project);

      /// <summary>
      ///    Groups all observed data classified under <paramref name="parentNode" /> having the given
      ///    <paramref name="category" /> as extended properties
      /// </summary>
      void GroupObservedDataByCategory(ITreeNode<IClassification> parentNode, string category);

      /// <summary>
      ///    Returns true if the <paramref name="node" /> represents an observed data node that can be dragged otherwise false
      /// </summary>
      bool CanDrag(ITreeNode node);

      /// <summary>
      ///    Returns <c>true</c> if the observed data defined unter the <paramref name="classificationNode" /> where removed
      ///    otherwise <c>false</c>
      /// </summary>
      bool RemoveObservedDataUnder(ITreeNode<IClassification> classificationNode);
   }

   public class ObservedDataInExplorerPresenter : IObservedDataInExplorerPresenter
   {
      private readonly IProjectRetriever _projectRetriever;
      private readonly ITreeNodeFactory _treeNodeFactory;
      private readonly IObservedDataTask _observedDataTask;
      private IExplorerPresenter _explorerPresenter;
      private IClassificationPresenter _classificationPresenter;
      private RootNodeType _observedDataFolder;

      public ObservedDataInExplorerPresenter(IProjectRetriever projectRetriever, ITreeNodeFactory treeNodeFactory, IObservedDataTask observedDataTask)
      {
         _projectRetriever = projectRetriever;
         _treeNodeFactory = treeNodeFactory;
         _observedDataTask = observedDataTask;
      }

      public void InitializeWith(IExplorerPresenter explorerPresenter, IClassificationPresenter classificationPresenter, RootNodeType observedDataFolder)
      {
         _explorerPresenter = explorerPresenter;
         _classificationPresenter = classificationPresenter;
         _observedDataFolder = observedDataFolder;
      }

      public void AddObservedDataToTree(IProject project)
      {
         _classificationPresenter.AddClassificationsToTree(project.AllClassificationsByType(ClassificationType.ObservedData));
         project.AllClassifiablesByType<ClassifiableObservedData>().Each(data => addObservedDataClassificationToTree(data));
      }

      public void GroupObservedDataByCategory(ITreeNode<IClassification> parentNode, string category)
      {
         _classificationPresenter.GroupClassificationsByCategory<ClassifiableObservedData>(parentNode,
            category, x => x.ExtendedPropertyValueFor(category));
      }

      public bool CanDrag(ITreeNode node)
      {
         if (node.IsAnImplementationOf<ObservedDataNode>())
            return true;

         var rootNode = node.TagAsObject as RootNodeType;
         if (rootNode == null)
            return false;

         return rootNode == _observedDataFolder;
      }

      public bool RemoveObservedDataUnder(ITreeNode<IClassification> classificationNode)
      {
         var allObservedData = classificationNode.AllNodes<ObservedDataNode>().Select(x => x.Tag.Repository);
         return _observedDataTask.Delete(allObservedData);
      }

      private ITreeNode addObservedDataClassificationToTree(ClassifiableObservedData classifiable)
      {
         var parent = classifiable.Parent ?? _observedDataFolder;
         var parentNode = _explorerPresenter.NodeFor(parent).DowncastTo<ITreeNode<IClassification>>();
         return addClassifiableObservedDataToTree(parentNode, classifiable);
      }

      private ITreeNode addClassifiableObservedDataToTree(ITreeNode<IClassification> classificationNode, ClassifiableObservedData classifiableObservedData)
      {
         var leafNode = _treeNodeFactory
            .CreateFor(classifiableObservedData)
            .WithIcon(ApplicationIcons.ObservedData)
            .Under(classificationNode);

         view.AddNode(classificationNode);

         return leafNode;
      }

      public void Handle(ObservedDataAddedEvent eventToHandle)
      {
         var node = addObservedDataToTree(eventToHandle.DataRepository);
         if (node == null) return;
         view.ExpandNode(node.ParentNode);
      }

      public void Handle(ObservedDataRemovedEvent eventToHandle)
      {
         _explorerPresenter.RemoveNodeFor(eventToHandle.DataRepository);
      }

      public IEnumerable<ClassificationTemplate> AvailableObservedDataCategoriesIn(ITreeNode<IClassification> parentClassificationNode)
      {
         return parentClassificationNode.Children<ObservedDataNode>().SelectMany(node => node.Tag.Repository.ExtendedProperties)
            .Select(property => property.Name)
            .Distinct()
            .Select(property => new ClassificationTemplate(property));
      }

      private ITreeNode addObservedDataToTree(DataRepository dataRepository)
      {
         var project = _projectRetriever.CurrentProject;

         var observedDataFolderNode = _explorerPresenter.NodeByType(_observedDataFolder);
         if (observedDataFolderNode == null)
            return null;

         var classifiableObservedData = project.GetOrCreateClassifiableFor<ClassifiableObservedData, DataRepository>(dataRepository);
         return addClassifiableObservedDataToTree(observedDataFolderNode, classifiableObservedData);
      }

      private IExplorerView view => _explorerPresenter.BaseView.DowncastTo<IExplorerView>();
   }
}