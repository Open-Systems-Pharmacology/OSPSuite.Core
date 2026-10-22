using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class MultipleSimulationFolderContextMenuFactory : IContextMenuSpecificationFactory<IReadOnlyList<ITreeNode>>
   {
      private readonly IContainer _container;

      public MultipleSimulationFolderContextMenuFactory(IContainer container)
      {
         _container = container;
      }

      public bool IsSatisfiedBy(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return presenter.IsAnImplementationOf<IExplorerPresenter>() &&
                treeNodes.All(x => x is ClassificationNode classificationNode && classificationNode.Tag.ClassificationType == ClassificationType.Simulation);
      }

      public IContextMenu CreateFor(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleSimulationFolderContextMenu(treeNodes.Cast<ClassificationNode>().ToList(), presenter.DowncastTo<IExplorerPresenter>(), _container);
      }
   }

   public class MultipleSimulationFolderContextMenu : ContextMenu<IReadOnlyList<ClassificationNode>, IExplorerPresenter>
   {
      public MultipleSimulationFolderContextMenu(IReadOnlyList<ClassificationNode> classificationNodes, IExplorerPresenter presenter, IContainer container)
         : base(classificationNodes, presenter, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<ClassificationNode> classificationNodes, IExplorerPresenter presenter)
      {
         yield return ClassificationCommonContextMenuItems.RemoveClassificationsMainMenu(classificationNodes, presenter);
      }
   }
}
