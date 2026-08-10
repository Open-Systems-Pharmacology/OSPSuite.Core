using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class SimulationAnalysisPresenterContextMenu : ContextMenu<ISimulationAnalysisPresenter, IPresenterWithAnalyses>
   {
      public SimulationAnalysisPresenterContextMenu(ISimulationAnalysisPresenter simulationAnalysisPresenter, IPresenterWithAnalyses presenterWithAnalyses, IContainer container)
         : base(simulationAnalysisPresenter, presenterWithAnalyses, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(ISimulationAnalysisPresenter simulationAnalysisPresenter, IPresenterWithAnalyses presenterWithAnalyses)
      {
         yield return CreateMenuButton.WithCaption(MenuNames.Clone)
            .WithActionCommand(() => presenterWithAnalyses.CloneAnalysis(simulationAnalysisPresenter.Analysis))
            .WithIcon(ApplicationIcons.Clone);

         yield return CreateMenuButton.WithCaption(MenuNames.RemoveAnalysis)
            .WithActionCommand(() => presenterWithAnalyses.RemoveAnalysis(simulationAnalysisPresenter))
            .WithIcon(ApplicationIcons.Close);

         yield return CreateMenuButton.WithCaption(MenuNames.RemoveAllAnalyses)
            .WithActionCommand(presenterWithAnalyses.RemoveAllAnalyses);

         yield return CreateMenuButton.WithCaption(MenuNames.Rename)
            .WithCommandFor<RenameSimulationAnalysisUICommand, ISimulationAnalysis>(simulationAnalysisPresenter.Analysis, _container)
            .WithIcon(ApplicationIcons.Rename)
            .AsGroupStarter();
      }
   }

   public class SimulationAnalysisPresenterContextMenuSpecificationFactory : IContextMenuSpecificationFactory<ISimulationAnalysisPresenter>
   {
      private readonly IContainer _container;

      public SimulationAnalysisPresenterContextMenuSpecificationFactory(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(ISimulationAnalysisPresenter simulationAnalysisPresenter, IPresenterWithContextMenu<ISimulationAnalysisPresenter> presenter)
      {
         return new SimulationAnalysisPresenterContextMenu(simulationAnalysisPresenter, presenter.DowncastTo<IPresenterWithAnalyses>(), _container);
      }

      public bool IsSatisfiedBy(ISimulationAnalysisPresenter simulationAnalysisPresenter, IPresenterWithContextMenu<ISimulationAnalysisPresenter> presenter)
      {
         return presenter.IsAnImplementationOf<IPresenterWithAnalyses>();
      }
   }
}
