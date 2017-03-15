using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ClassificationCommonContextMenuItems
   {
      /// <summary>
      ///    Returns the main menu entry for the remove section of a classification Node
      /// </summary>
      public static IMenuBarItem RemoveClassificationMainMenu(ITreeNode<IClassification> classificationNode, IExplorerPresenter presenter)
      {
         var groupMenu = CreateSubMenu.WithCaption(MenuNames.DeleteSubMenu)
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();

         groupMenu.AddItem(DeleteClassificationAndDataMenu(classificationNode, presenter));
         groupMenu.AddItem(DeleteClassificationAndKeepDataMenu(classificationNode, presenter));

         return groupMenu;
      }

      public static IMenuBarButton RemoveEmptyClassificationsMenu(IExplorerPresenter presenter)
      {
         return CreateMenuButton.WithCaption(MenuNames.RemoveEmptyGroups)
            .WithActionCommand(presenter.RemoveEmptyClassifcations)
            .WithIcon(ApplicationIcons.Delete);
      }

      public static IMenuBarButton DeleteChildrenClassificationsAndKeepDataMenu(ITreeNode<IClassification> classificationNode, IExplorerPresenter presenter)
      {
         return CreateMenuButton.WithCaption(MenuNames.DeleteSubFoldersAndKeepData)
            .WithIcon(ApplicationIcons.DeleteFolderOnly)
            .WithActionCommand(() => presenter.RemoveChildrenClassifications(classificationNode, removeParent: false));
      }

      public static IMenuBarButton DeleteClassificationAndKeepDataMenu(ITreeNode<IClassification> classificationNode, IExplorerPresenter presenter)
      {
         return CreateMenuButton.WithCaption(MenuNames.DeleteFolderAndKeepData)
            .WithIcon(ApplicationIcons.DeleteFolderOnly)
            .WithActionCommand(() => presenter.RemoveChildrenClassifications(classificationNode, removeParent: true));
      }

      public static IMenuBarButton DeleteClassificationAndDataMenu(ITreeNode<IClassification> classificationNode, IExplorerPresenter presenter)
      {
         return CreateMenuButton.WithCaption(MenuNames.DeleteFolderAndData)
            .WithIcon(ApplicationIcons.Delete)
            .WithActionCommand(() => presenter.RemoveChildrenClassifications(classificationNode, removeParent: true, removeData: true));
      }

      public static IMenuBarButton DeleteChildrenClassificationsAndDateMenu(ITreeNode<IClassification> classificationNode, IExplorerPresenter presenter)
      {
         return CreateMenuButton.WithCaption(MenuNames.DeleteSubFoldersAndData)
            .WithIcon(ApplicationIcons.Delete)
            .WithActionCommand(() => presenter.RemoveChildrenClassifications(classificationNode, removeParent: false, removeData: true));
      }

      public static IMenuBarButton CreateClassificationUnderMenu(ITreeNode<IClassification> classificationNode, IExplorerPresenter presenter)
      {
         return CreateMenuButton.WithCaption(MenuNames.CreateGroup)
            .WithActionCommand(() => presenter.CreateClassificationUnder(classificationNode))
            .WithIcon(ApplicationIcons.Create);
      }

      public static IMenuBarItem RemoveClassificationFolderMainMenu(ITreeNode<RootNodeType> classificationFolderRootNode, IExplorerPresenter presenter)
      {
         var groupMenu = CreateSubMenu.WithCaption(MenuNames.DeleteSubMenu)
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();

         groupMenu.AddItem(DeleteChildrenClassificationsAndDateMenu(classificationFolderRootNode, presenter));
         groupMenu.AddItem(DeleteChildrenClassificationsAndKeepDataMenu(classificationFolderRootNode, presenter));
         return groupMenu;
      }
   }
}