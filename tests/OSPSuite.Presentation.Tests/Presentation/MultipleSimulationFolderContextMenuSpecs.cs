using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Views.ContextMenus;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_MultipleSimulationFolderContextMenuFactory : ContextSpecification<MultipleSimulationFolderContextMenuFactory>
   {
      protected IContainer _container;
      protected IContextMenuView _contextMenuView;
      protected IExplorerPresenter _explorerPresenter;
      protected ClassificationNode _simulationFolder1;
      protected ClassificationNode _simulationFolder2;

      protected override void Context()
      {
         _container = A.Fake<IContainer>();
         _contextMenuView = A.Fake<IContextMenuView>();
         _explorerPresenter = A.Fake<IExplorerPresenter>();
         A.CallTo(() => _container.Resolve<IContextMenuView>()).Returns(_contextMenuView);

         _simulationFolder1 = FolderFor(ClassificationType.Simulation);
         _simulationFolder2 = FolderFor(ClassificationType.Simulation);

         sut = new MultipleSimulationFolderContextMenuFactory(_container);
      }

      protected ClassificationNode FolderFor(ClassificationType classificationType) => new ClassificationNode(new Classification {ClassificationType = classificationType});
   }

   public class When_checking_if_a_context_menu_can_be_created_for_multiple_simulation_folders : concern_for_MultipleSimulationFolderContextMenuFactory
   {
      [Observation]
      public void should_return_true_for_simulation_folders_selected_in_an_explorer()
      {
         sut.IsSatisfiedBy(new[] {_simulationFolder1, _simulationFolder2}, _explorerPresenter).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_one_folder_is_not_a_simulation_folder()
      {
         sut.IsSatisfiedBy(new[] {_simulationFolder1, FolderFor(ClassificationType.ObservedData)}, _explorerPresenter).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_one_node_is_not_a_folder()
      {
         sut.IsSatisfiedBy(new[] {_simulationFolder1, A.Fake<ITreeNode>()}, _explorerPresenter).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_for_a_presenter_that_is_not_an_explorer()
      {
         sut.IsSatisfiedBy(new[] {_simulationFolder1, _simulationFolder2}, A.Fake<IPresenterWithContextMenu<IReadOnlyList<ITreeNode>>>()).ShouldBeFalse();
      }
   }

   public class When_creating_the_context_menu_for_multiple_simulation_folders : concern_for_MultipleSimulationFolderContextMenuFactory
   {
      private List<IMenuBarItem> _allMenuItems;
      private IReadOnlyList<ITreeNode<IClassification>> _removedClassificationNodes;
      private bool _dataRemoved;

      protected override void Context()
      {
         base.Context();
         _allMenuItems = new List<IMenuBarItem>();
         A.CallTo(() => _contextMenuView.AddMenuItem(A<IMenuBarItem>._))
            .Invokes(x => _allMenuItems.Add(x.GetArgument<IMenuBarItem>(0)));

         A.CallTo(() => _explorerPresenter.RemoveClassifications(A<IReadOnlyList<ITreeNode<IClassification>>>._, A<bool>._))
            .Invokes(x =>
            {
               _removedClassificationNodes = x.GetArgument<IReadOnlyList<ITreeNode<IClassification>>>(0);
               _dataRemoved = x.GetArgument<bool>(1);
            });
      }

      protected override void Because()
      {
         sut.CreateFor(new[] {_simulationFolder1, _simulationFolder2}, _explorerPresenter);
      }

      [Observation]
      public void should_create_a_delete_menu_to_delete_the_folders_with_or_without_their_data()
      {
         _allMenuItems.Select(x => x.Caption).ShouldOnlyContain(MenuNames.DeleteSubMenu);
         allDeleteMenuButtons().Select(x => x.Caption).ShouldOnlyContainInOrder(MenuNames.DeleteFoldersAndData, MenuNames.DeleteFoldersAndKeepData);
      }

      [Observation]
      public void clicking_the_delete_folders_and_data_menu_should_remove_the_folders_and_their_data()
      {
         deleteMenuButtonWithCaption(MenuNames.DeleteFoldersAndData).Command.Execute();
         _removedClassificationNodes.ShouldOnlyContainInOrder(_simulationFolder1, _simulationFolder2);
         _dataRemoved.ShouldBeTrue();
      }

      [Observation]
      public void clicking_the_delete_folders_and_keep_data_menu_should_remove_the_folders_only()
      {
         deleteMenuButtonWithCaption(MenuNames.DeleteFoldersAndKeepData).Command.Execute();
         _removedClassificationNodes.ShouldOnlyContainInOrder(_simulationFolder1, _simulationFolder2);
         _dataRemoved.ShouldBeFalse();
      }

      private IReadOnlyList<IMenuBarButton> allDeleteMenuButtons()
      {
         return _allMenuItems.OfType<IMenuBarSubMenu>().Single().AllItems().OfType<IMenuBarButton>().ToList();
      }

      private IMenuBarButton deleteMenuButtonWithCaption(string caption)
      {
         return allDeleteMenuButtons().First(x => string.Equals(x.Caption, caption));
      }
   }
}
