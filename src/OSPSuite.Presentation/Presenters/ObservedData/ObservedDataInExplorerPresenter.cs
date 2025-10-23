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
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters.ObservedData
{ 
   public interface IObservedDataInExplorerPresenter : IClassificationInExplorerPresenter,
      IListener<ObservedDataAddedEvent>,
      IListener<ObservedDataRemovedEvent>
   {
      /// <summary>
      ///    Returns all possible <see cref="ClassificationTemplate" /> defined for observed data under the given
      ///    <paramref name="parentClassificationNode" />
      /// </summary>
      IEnumerable<ClassificationTemplate> AvailableObservedDataCategoriesIn(ITreeNode<IClassification> parentClassificationNode);

      /// <summary>
      ///    Groups all observed data classified under <paramref name="parentNode" /> having the given
      ///    <paramref name="category" /> as extended properties
      /// </summary>
      void GroupObservedDataByCategory(ITreeNode<IClassification> parentNode, string category);

      /// <summary>
      ///    Returns <c>true</c> if the observed data defined under the <paramref name="classificationNode" /> were removed
      ///    otherwise <c>false</c>
      /// </summary>
      bool RemoveObservedDataUnder(ITreeNode<IClassification> classificationNode);
   }

   public class ObservedDataInExplorerPresenter : ClassificationInExplorerPresenter<ClassifiableObservedData>, IObservedDataInExplorerPresenter
   {
      private readonly IProjectRetriever _projectRetriever;
      private readonly IObservedDataTask _observedDataTask;


      public ObservedDataInExplorerPresenter(IProjectRetriever projectRetriever, ITreeNodeFactory treeNodeFactory, IObservedDataTask observedDataTask) : base(treeNodeFactory)
      {
         _projectRetriever = projectRetriever;
         _observedDataTask = observedDataTask;
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

         return rootNode == _rootNodeType;
      }

      public bool RemoveObservedDataUnder(ITreeNode<IClassification> classificationNode)
      {
         var allObservedData = classificationNode.AllNodes<ObservedDataNode>().Select(x => x.Tag.Repository);
         return _observedDataTask.Delete(allObservedData);
      }

      public void Handle(ObservedDataAddedEvent eventToHandle)
      {
         eventToHandle.DataRepositories.Each(x =>
         {
            var node = addObservedDataToTree(x);
            if (node == null) 
               return;
            view.ExpandNode(node.ParentNode);
         });
      }

      public void Handle(ObservedDataRemovedEvent eventToHandle)
      {
         eventToHandle.DataRepositories.Each(x => _explorerPresenter.RemoveNodeFor(x));
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

         var observedDataFolderNode = _explorerPresenter.NodeByType(_rootNodeType);
         if (observedDataFolderNode == null)
            return null;

         var classifiableObservedData = project.GetOrCreateClassifiableFor<ClassifiableObservedData, DataRepository>(dataRepository);
         return AddClassifiableToTree(observedDataFolderNode, classifiableObservedData);
      }

      protected override ClassificationType ClassificationType => ClassificationType.ObservedData;

      protected override ITreeNode CreateNodeFor(ClassifiableObservedData classifiable)
      {
         return _treeNodeFactory
            .CreateFor(classifiable)
            .WithIcon(ApplicationIcons.ObservedData);
      }

      private IExplorerView view => _explorerPresenter.BaseView.DowncastTo<IExplorerView>();
   }
}