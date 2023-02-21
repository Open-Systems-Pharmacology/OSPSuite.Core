using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class ParameterIdentificationFolderContextMenu : ContextMenu
   {
      public ParameterIdentificationFolderContextMenu(ITreeNode<RootNodeType> treeNode, IExplorerPresenter presenter, IContainer container) : base(container)
      {
         _view.AddMenuItem(ParameterIdentificationContextMenuItems.CreateParameterIdentification(container));
         _view.AddMenuItem(ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(treeNode, presenter).AsGroupStarter());
         _view.AddMenuItem(ClassificationCommonContextMenuItems.RemoveClassificationFolderMainMenu(treeNode, presenter));
      }
   }

   public class ParameterIdentificationFolderTreeNodeContextMenuFactory : RootNodeContextMenuFactory
   {
      private readonly IContainer _container;

      public ParameterIdentificationFolderTreeNodeContextMenuFactory(IMenuBarItemRepository repository, IContainer container) : base(RootNodeTypes.ParameterIdentificationFolder, repository)
      {
         _container = container;
      }

      public override IContextMenu CreateFor(ITreeNode<RootNodeType> treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return new ParameterIdentificationFolderContextMenu(treeNode, presenter.DowncastTo<IExplorerPresenter>(), _container);
      }
   }
}