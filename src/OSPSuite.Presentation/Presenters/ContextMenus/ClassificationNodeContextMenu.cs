using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public abstract class ClassificationNodeContextMenu<TPresenter> : ContextMenu<ClassificationNode, TPresenter> where TPresenter : IExplorerPresenter
   {
      protected ClassificationNodeContextMenu(ClassificationNode objectRequestingContextMenu, TPresenter presenter)
         : base(objectRequestingContextMenu, presenter)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(ClassificationNode classificationNode, TPresenter presenter)
      {
         yield return RenameGroupMenuFor(classificationNode, presenter);
         yield return CreateGroupMenuFor(classificationNode, presenter);

         var groupMenu = AddClassificationMenu(classificationNode, presenter);

         if (groupMenu.AllItems().Any())
            yield return groupMenu;

         yield return ClassificationCommonContextMenuItems.RemoveClassificationMainMenu(classificationNode, presenter);
      }

      protected IMenuBarButton CreateGroupMenuFor(ClassificationNode classificationNode, TPresenter presenter)
      {
         return ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(classificationNode, presenter);
      }

      protected IMenuBarButton RenameGroupMenuFor(ClassificationNode classificationNode, TPresenter presenter)
      {
         return CreateMenuButton.WithCaption(MenuNames.Rename)
            .WithActionCommand(() => presenter.RenameClassification(classificationNode))
            .WithIcon(ApplicationIcons.Rename);
      }
    
      protected static IMenuBarSubMenu AddClassificationMenu(ClassificationNode classificationNode, TPresenter presenter)
      {
         var groupMenu = CreateSubMenu.WithCaption(MenuNames.GroupBy);
         presenter.AvailableClassificationCategories(classificationNode)
            .Each(classification => groupMenu.AddItem(
               CreateMenuButton.WithCaption(classification.ClassificationName)
                  .WithIcon(classification.Icon)
                  .WithActionCommand(() => presenter.AddToClassificationTree(classificationNode, classification.ClassificationName))));

         return groupMenu;
      }
   }

   public abstract class ClassificationNodeContextMenuFactory :
      IContextMenuSpecificationFactory<ITreeNode>,
      IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly ClassificationType _classificationType;

      protected ClassificationNodeContextMenuFactory(ClassificationType classificationType)
      {
         _classificationType = classificationType;
      }

      public IContextMenu CreateFor(ITreeNode treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return CreateFor(treeNode.DowncastTo<ClassificationNode>(), presenter.DowncastTo<IExplorerPresenter>());
      }

      public bool IsSatisfiedBy(ITreeNode treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return isSatisifiedBy(treeNode, presenter);
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return CreateFor(viewItem.DowncastTo<ClassificationNode>(), presenter.DowncastTo<IExplorerPresenter>());
      }

      protected abstract IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter);

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return isSatisifiedBy(viewItem,presenter);
      }

      private bool isSatisifiedBy(object obj, IPresenter presenter)
      {
         if (!obj.IsAnImplementationOf<ClassificationNode>()) return false;

         var node = obj.DowncastTo<ClassificationNode>();
         return node.Tag.ClassificationType == _classificationType && presenter.IsAnImplementationOf<IExplorerPresenter>();
      }
   }


}