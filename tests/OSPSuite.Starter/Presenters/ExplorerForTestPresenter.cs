using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IExplorerForTestPresenter : IPresenter<IExplorerForTestView>, IExplorerPresenter
   {

   }

   public class ExplorerForTestPresenter : AbstractExplorerPresenter<IExplorerForTestView, IExplorerForTestPresenter>, IExplorerForTestPresenter
   {
      public ExplorerForTestPresenter(IExplorerForTestView view, IRegionResolver regionResolver, IClassificationPresenter classificationPresenter, IToolTipPartCreator toolTipPartCreator, IProjectRetriever projectRetriever)
         : base(view, regionResolver, classificationPresenter, toolTipPartCreator, RegionNames.Explorer, projectRetriever)
      {
      }
      public override bool CanDrag(ITreeNode node)
      {
         return true;
      }

      public override void NodeDoubleClicked(ITreeNode node)
      {
      }

      public override IEnumerable<ClassificationTemplate> AvailableClassificationCategories(ITreeNode<IClassification> parentClassificationNode)
      {
         return Enumerable.Empty<ClassificationTemplate>();
      }

      public override void AddToClassificationTree(ITreeNode<IClassification> parentNode, string category)
      {
      }

      public override bool RemoveDataUnderClassification(ITreeNode<IClassification> classificationNode)
      {
         return true;
      }

      protected override bool CanDrop(ITreeNode<IClassifiable> classifiableNode, ITreeNode<IClassification> classificationNode)
      {
         return true;
      }

      protected override void AddProjectToTree(IProject project)
      {
      }

      public override void ShowContextMenu(ITreeNode node, Point popupLocation)
      {
      }

      public override void ShowContextMenu(IReadOnlyList<ITreeNode> treeNodes, Point popupLocation)
      {
      }

      
   }
}