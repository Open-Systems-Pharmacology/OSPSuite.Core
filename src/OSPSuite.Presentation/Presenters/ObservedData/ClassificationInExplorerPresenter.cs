using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IClassificationInExplorerPresenter
   {
      /// <summary>
      ///    Initializes the current object with the required references to perform its tasks. This should be done only once
      /// </summary>
      /// <param name="explorerPresenter">Explorer Presenter for which classification should be managed</param>
      /// <param name="classificationPresenter">
      ///    The Classification Presenter defined in the <paramref name="explorerPresenter" />
      ///    .
      /// </param>
      /// <param name="rootNodeType">
      ///    The application specific <see cref="RootNodeType" /> representing the classifiable
      ///    folder
      /// </param>
      void InitializeWith(IExplorerPresenter explorerPresenter, IClassificationPresenter classificationPresenter, RootNodeType rootNodeType);

      /// <summary>
      ///    Returns true if the <paramref name="node" /> represents a node can be dragged otherwise false
      /// </summary>
      bool CanDrag(ITreeNode node);

      /// <summary>
      ///    Add all classifications defined in <paramref name="project" /> to the explorer
      /// </summary>
      void AddAllClassificationsToTree(IProject project);
   }

   public abstract class ClassificationInExplorerPresenter<TClassifiable> where TClassifiable : class, IClassifiable
   {
      protected IExplorerPresenter _explorerPresenter;
      protected IClassificationPresenter _classificationPresenter;
      protected RootNodeType _rootNodeType;
      protected readonly ITreeNodeFactory _treeNodeFactory;

      protected ClassificationInExplorerPresenter(ITreeNodeFactory treeNodeFactory)
      {
         _treeNodeFactory = treeNodeFactory;
      }

      public void InitializeWith(IExplorerPresenter explorerPresenter, IClassificationPresenter classificationPresenter, RootNodeType rootNodeType)
      {
         _explorerPresenter = explorerPresenter;
         _classificationPresenter = classificationPresenter;
         _rootNodeType = rootNodeType;
      }

      public void AddAllClassificationsToTree(IProject project)
      {
         _classificationPresenter.AddClassificationsToTree(project.AllClassificationsByType(ClassificationType));
         project.AllClassifiablesByType<TClassifiable>().Each(data => addClassificationToTree(data));
      }

      protected abstract ClassificationType ClassificationType { get; }

      private ITreeNode addClassificationToTree(TClassifiable classifiable)
      {
         var parent = classifiable.Parent ?? _rootNodeType;
         var parentNode = _explorerPresenter.NodeFor(parent).DowncastTo<ITreeNode<IClassification>>();
         return AddClassifiableToTree(parentNode, classifiable);
      }

      protected ITreeNode AddClassifiableToTree(ITreeNode<IClassification> classificationNode, TClassifiable classifiable)
      {
         var leafNode = CreateNodeFor(classifiable)
            .Under(classificationNode);

         view.AddNode(classificationNode);

         return leafNode;
      }

      protected abstract ITreeNode CreateNodeFor(TClassifiable classifiable);

      private IExplorerView view => _explorerPresenter.BaseView.DowncastTo<IExplorerView>();
   }
}