using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class ParameterIdentificationSimulationNodeContextMenu : ContextMenu<ISimulation, IParameterIdentificationSimulationSelectionPresenter>
   {
      public ParameterIdentificationSimulationNodeContextMenu(ISimulation simulation, IParameterIdentificationSimulationSelectionPresenter presenter) : base(simulation, presenter)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(ISimulation simulation, IParameterIdentificationSimulationSelectionPresenter presenter)
      {
         yield return CreateMenuButton.WithCaption(MenuNames.ReplaceSimulation.WithEllipsis())
            .WithActionCommand(() => presenter.ReplaceSimulation(simulation))
            .WithIcon(ApplicationIcons.Swap);

         yield return CreateMenuButton.WithCaption(MenuNames.Remove)
            .WithActionCommand(() => presenter.RemoveSimulation(simulation))
            .WithIcon(ApplicationIcons.Remove).AsGroupStarter();
      }
   }

   public class ParameterIdentificationSimulationNodeContextMenuFactory : IContextMenuSpecificationFactory<ITreeNode>
   {
      public IContextMenu CreateFor(ITreeNode treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return new ParameterIdentificationSimulationNodeContextMenu(treeNode.TagAsObject.DowncastTo<ISimulation>(), presenter.DowncastTo<IParameterIdentificationSimulationSelectionPresenter>());
      }

      public bool IsSatisfiedBy(ITreeNode treeNode, IPresenterWithContextMenu<ITreeNode> presenter)
      {
         return treeNode.TagAsObject.IsAnImplementationOf<ISimulation>() &&
                presenter.IsAnImplementationOf<IParameterIdentificationSimulationSelectionPresenter>();
      }
   }
}