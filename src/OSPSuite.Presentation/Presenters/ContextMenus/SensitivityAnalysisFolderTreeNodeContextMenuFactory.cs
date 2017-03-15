using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Repositories;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class SensitivityAnalysisFolderContextMenu : ContextMenu
   {
      public SensitivityAnalysisFolderContextMenu(ITreeNode<RootNodeType> treeNode, IExplorerPresenter presenter)
      {
         _view.AddMenuItem(SensitivityAnalysisContextMenuItems.CreateSensitivityAnalysis());
         _view.AddMenuItem(ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(treeNode, presenter).AsGroupStarter());
         _view.AddMenuItem(ClassificationCommonContextMenuItems.RemoveClassificationFolderMainMenu(treeNode, presenter));
      }
   }

   public class SensitivityAnalysisFolderTreeNodeContextMenuFactory : RootNodeContextMenuFactory
   {
      public SensitivityAnalysisFolderTreeNodeContextMenuFactory(IMenuBarItemRepository repository) : base(RootNodeTypes.SensitivityAnalysisFolder, repository)
      {
      }

      public override IContextMenu CreateFor(ITreeNode<RootNodeType> treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return new SensitivityAnalysisFolderContextMenu(treeNode, presenter.DowncastTo<IExplorerPresenter>());
      }
   }
}