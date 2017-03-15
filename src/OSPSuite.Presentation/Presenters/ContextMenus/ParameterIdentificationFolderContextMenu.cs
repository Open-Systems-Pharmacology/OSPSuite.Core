using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Repositories;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class ParameterIdentificationFolderContextMenu : ContextMenu
   {
      public ParameterIdentificationFolderContextMenu(ITreeNode<RootNodeType> treeNode, IExplorerPresenter presenter)
      {
         _view.AddMenuItem(ParameterIdentificationContextMenuItems.CreateParameterIdentification());
         _view.AddMenuItem(ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(treeNode, presenter).AsGroupStarter());
         _view.AddMenuItem(ClassificationCommonContextMenuItems.RemoveClassificationFolderMainMenu(treeNode, presenter));
      }
   }

   public class ParameterIdentificationFolderTreeNodeContextMenuFactory : RootNodeContextMenuFactory
   {
      public ParameterIdentificationFolderTreeNodeContextMenuFactory(IMenuBarItemRepository repository) : base(RootNodeTypes.ParameterIdentificationFolder, repository)
      {
      }

      public override IContextMenu CreateFor(ITreeNode<RootNodeType> treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return new ParameterIdentificationFolderContextMenu(treeNode, presenter.DowncastTo<IExplorerPresenter>());
      }
   }
}