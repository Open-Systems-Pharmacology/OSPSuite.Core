using OSPSuite.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class SimulationClassificationCommonContextMenuItems
   {
      public static IMenuBarItem RemoveSimulationFolderMainMenu(ITreeNode<RootNodeType> simulationFolderNode, IExplorerPresenter presenter)
      {
         var groupMenu = CreateSubMenu.WithCaption(MenuNames.DeleteSubMenu)
            .WithIcon(ApplicationIcons.Remove)
            .AsGroupStarter();

         groupMenu.AddItem(ClassificationCommonContextMenuItems.DeleteChildrenClassificationsAndDateMenu(simulationFolderNode, presenter));
         groupMenu.AddItem(ClassificationCommonContextMenuItems.DeleteChildrenClassificationsAndKeepDataMenu(simulationFolderNode, presenter));
         return groupMenu;
      }
   }
}