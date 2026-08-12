using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Presentation.Views.ContextMenus;
using OSPSuite.Utility.Events;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SimulationAnalysisPresenterContextMenuSpecificationFactory : ContextSpecification<SimulationAnalysisPresenterContextMenuSpecificationFactory>
   {
      protected IContainer _container;
      protected IContextMenuView _contextMenuView;
      protected ISimulationAnalysisPresenter _simulationAnalysisPresenter;
      protected IPresenterWithAnalyses _presenterWithAnalyses;

      protected override void Context()
      {
         _container = A.Fake<IContainer>();
         _contextMenuView = A.Fake<IContextMenuView>();
         _simulationAnalysisPresenter = A.Fake<ISimulationAnalysisPresenter>();
         _presenterWithAnalyses = A.Fake<IPresenterWithAnalyses>();

         A.CallTo(() => _container.Resolve<IContextMenuView>()).Returns(_contextMenuView);
         A.CallTo(() => _container.Resolve<RenameSimulationAnalysisUICommand>()).Returns(
            new RenameSimulationAnalysisUICommand(A.Fake<IOSPSuiteExecutionContext>(), A.Fake<IEventPublisher>(), A.Fake<IApplicationController>()));

         sut = new SimulationAnalysisPresenterContextMenuSpecificationFactory(_container);
      }
   }

   public class When_checking_if_a_context_menu_can_be_created_for_a_presenter_managing_analyses : concern_for_SimulationAnalysisPresenterContextMenuSpecificationFactory
   {
      [Observation]
      public void should_return_true_for_a_presenter_implementing_the_presenter_with_analyses_interface()
      {
         sut.IsSatisfiedBy(_simulationAnalysisPresenter, _presenterWithAnalyses).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_for_an_edit_analyzable_presenter()
      {
         sut.IsSatisfiedBy(_simulationAnalysisPresenter, A.Fake<IEditAnalyzablePresenter>()).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_for_any_other_presenter()
      {
         sut.IsSatisfiedBy(_simulationAnalysisPresenter, A.Fake<IPresenterWithContextMenu<ISimulationAnalysisPresenter>>()).ShouldBeFalse();
      }
   }

   public class When_creating_the_context_menu_for_a_simulation_analysis_presenter : concern_for_SimulationAnalysisPresenterContextMenuSpecificationFactory
   {
      private List<IMenuBarItem> _allMenuItems;
      private ISimulationAnalysis _analysis;

      protected override void Context()
      {
         base.Context();
         _allMenuItems = new List<IMenuBarItem>();
         _analysis = A.Fake<ISimulationAnalysis>();
         A.CallTo(() => _simulationAnalysisPresenter.Analysis).Returns(_analysis);
         A.CallTo(() => _contextMenuView.AddMenuItem(A<IMenuBarItem>._))
            .Invokes(x => _allMenuItems.Add(x.GetArgument<IMenuBarItem>(0)));
      }

      protected override void Because()
      {
         sut.CreateFor(_simulationAnalysisPresenter, _presenterWithAnalyses);
      }

      [Observation]
      public void should_create_a_menu_containing_clone_remove_remove_all_and_rename()
      {
         _allMenuItems.Select(x => x.Caption).ShouldOnlyContainInOrder(MenuNames.Clone, MenuNames.RemoveAnalysis, MenuNames.RemoveAllAnalyses, MenuNames.Rename);
      }

      [Observation]
      public void clicking_the_clone_menu_should_clone_the_analysis()
      {
         menuButtonWithCaption(MenuNames.Clone).Command.Execute();
         A.CallTo(() => _presenterWithAnalyses.CloneAnalysis(_analysis)).MustHaveHappened();
      }

      [Observation]
      public void clicking_the_remove_menu_should_remove_the_analysis()
      {
         menuButtonWithCaption(MenuNames.RemoveAnalysis).Command.Execute();
         A.CallTo(() => _presenterWithAnalyses.RemoveAnalysis(_simulationAnalysisPresenter)).MustHaveHappened();
      }

      [Observation]
      public void clicking_the_remove_all_menu_should_remove_all_analyses()
      {
         menuButtonWithCaption(MenuNames.RemoveAllAnalyses).Command.Execute();
         A.CallTo(() => _presenterWithAnalyses.RemoveAllAnalyses()).MustHaveHappened();
      }

      [Observation]
      public void the_rename_menu_should_start_a_new_group()
      {
         menuButtonWithCaption(MenuNames.Rename).BeginGroup.ShouldBeTrue();
      }

      private IMenuBarButton menuButtonWithCaption(string caption)
      {
         return _allMenuItems.OfType<IMenuBarButton>().First(x => string.Equals(x.Caption, caption));
      }
   }
}
