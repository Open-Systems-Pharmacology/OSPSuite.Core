using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class MultipleObservedDataFolderContextMenuFactory : IContextMenuSpecificationFactory<IReadOnlyList<ITreeNode>>
   {
      private readonly IContainer _container;

      public MultipleObservedDataFolderContextMenuFactory(IContainer container)
      {
         _container = container;
      }
      public bool IsSatisfiedBy(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return treeNodes.All(x => x is ClassificationNode classificationNode && classificationNode.Tag.ClassificationType == ClassificationType.ObservedData);
      }

      public IContextMenu CreateFor(IReadOnlyList<ITreeNode> objectRequestingContextMenu, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         var explorerPresenter = presenter.DowncastTo<IExplorerPresenter>();
         return new MultipleObservedDataFolderContextMenu(objectRequestingContextMenu.Select(x => x as ClassificationNode).ToList(), explorerPresenter, _container);
      }
   }

   public class MultipleObservedDataFolderContextMenu : ContextMenu<IReadOnlyList<ClassificationNode>, IExplorerPresenter>
   {

      public MultipleObservedDataFolderContextMenu(IReadOnlyList<ClassificationNode> treeNodes, IExplorerPresenter explorerPresenter, IContainer container) : base(treeNodes, explorerPresenter, container)
      {
         
      }

      private IMenuBarSubMenu createGroupingSubMenu(IReadOnlyList<ClassificationNode> treeNodes, IExplorerPresenter presenter)
      {
         var groupMenu = CreateSubMenu.WithCaption(MenuNames.GroupBy);

         var classificationsSupportedByEachNode = treeNodes.Select(treeNode => (treeNode, templates: presenter.AvailableClassificationCategories(treeNode))).ToList();

         intersectionByClassificationName(classificationsSupportedByEachNode).Each(classification => groupMenu.AddItem(classifyButtonFor(treeNodes, classification, presenter)));

         return groupMenu;
      }

      private IMenuBarButton classifyButtonFor(IReadOnlyList<ITreeNode<IClassification>> treeNodes, ClassificationTemplate classification, IExplorerPresenter presenter)
      {
         return CreateMenuButton.WithCaption(classification.ClassificationName)
            .WithIcon(classification.Icon)
            .WithActionCommand(() => classifyTreeNodes(treeNodes, classification, presenter));
      }

      private IReadOnlyList<ClassificationTemplate> intersectionByClassificationName(List<(ClassificationNode treeNode, IEnumerable<ClassificationTemplate> templates)> classifications)
      {
         return (classifications.Any() ? classifications.First().templates.Where(template => templateIsCommonToAll(classifications, template)) : Enumerable.Empty<ClassificationTemplate>()).ToList();
      }

      private bool templateIsCommonToAll(List<(ClassificationNode treeNode, IEnumerable<ClassificationTemplate> templates)> classifications, ClassificationTemplate classification)
      {
         return classifications.All(x => x.templates.Any(c => c.ClassificationName.Equals(classification.ClassificationName)));
      }

      private void classifyTreeNodes(IReadOnlyList<ITreeNode<IClassification>> treeNodes, ClassificationTemplate classification, IExplorerPresenter presenter)
      {
         treeNodes.Each(treeNode => { presenter.AddToClassificationTree(treeNode, classification.ClassificationName); });
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<ClassificationNode> objectRequestingContextMenu, IExplorerPresenter presenter)
      {
         var groupMenu = createGroupingSubMenu(objectRequestingContextMenu, presenter);
         if (groupMenu.AllItems().Any())
            yield return groupMenu;
      }
   }
}