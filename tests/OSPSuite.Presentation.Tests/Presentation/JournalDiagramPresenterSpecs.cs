using System.Collections.Generic;
using System.Drawing;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_JournalDiagramPresenter : ContextSpecification<JournalDiagramPresenter>
   {
      protected IJournalTask _journalTask;
      protected IJournalDiagramView _view;
      protected IMultipleBaseNodeContextMenuFactory _multipleBaseNodeContextMenuFactory;
      protected IJournalPageTask _journalPageTask;
      protected IPresentationUserSettings _userSettings;
      protected IDialogCreator _dialogCreator;
      protected IContainerBaseLayouter _containerBaseLayouter;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IReloadRelatedItemTask _reloadRelatedItemTask;
      protected JournalDiagram _journalDiagram;
      protected Journal _journal;
      private IDiagramModelFactory _diagramModelFactory;

      protected override void Context()
      {
         _journalTask = A.Fake<IJournalTask>();
         _view = A.Fake<IJournalDiagramView>();
         _reloadRelatedItemTask = A.Fake<IReloadRelatedItemTask>();
         var journalComparisonTask = A.Fake<IJournalComparisonTask>();
         _multipleBaseNodeContextMenuFactory = A.Fake<IMultipleBaseNodeContextMenuFactory>();
         _journalPageTask = A.Fake<IJournalPageTask>();
         _userSettings = A.Fake<IPresentationUserSettings>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _containerBaseLayouter = A.Fake<IContainerBaseLayouter>();
         _viewItemContextMenuFactory= A.Fake<IViewItemContextMenuFactory>();
         _diagramModelFactory= A.Fake<IDiagramModelFactory>();
         sut = new JournalDiagramPresenter(_view, _containerBaseLayouter, _dialogCreator, _diagramModelFactory, _userSettings, _journalTask,
            _journalPageTask, _reloadRelatedItemTask, journalComparisonTask, _multipleBaseNodeContextMenuFactory, _viewItemContextMenuFactory);

         _journal = new Journal();
         _journalDiagram = new JournalDiagram().WithName(global::OSPSuite.Assets.Captions.Journal.DefaultDiagramName);
         _journal.AddDiagram(_journalDiagram);
         _journalDiagram.DiagramManager = A.Fake<IDiagramManager<JournalDiagram>>();
         _journalDiagram.DiagramModel = A.Fake<IDiagramModel>();
         sut.Handle(new JournalLoadedEvent(_journal));
      }
   }

   public class When_restoring_the_chronological_order_of_a_diagram : concern_for_JournalDiagramPresenter
   {
      [Observation]
      public void should_confirm_before_delete()
      {
         sut.RestoreChronologicalOrder();
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>.Ignored)).MustHaveHappened();
      }

      [Observation]
      public void should_not_restore_if_not_confirmed()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._)).Returns(ViewResult.No);
         sut.RestoreChronologicalOrder();
         A.CallTo(() => _journalDiagram.DiagramManager.RefreshDiagramFromModel()).MustNotHaveHappened();
      }

      [Observation]
      public void should_restore_if_confirmed()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._)).Returns(ViewResult.Yes);
         sut.RestoreChronologicalOrder();
         A.CallTo(() => _journalDiagram.DiagramManager.RefreshDiagramFromModel()).MustHaveHappened();
      }
   }

   public class When_removing_related_items_from_journal : concern_for_JournalDiagramPresenter
   {
      private JournalPage _journalPage;
      private RelatedItem _relatedItem;
      private IRelatedItemNode _relatedItemNode;
      private IJournalPageNode _journalPageNode;

      protected override void Context()
      {
         base.Context();
         _journalPage = new JournalPage().WithId("journalPage");
         _relatedItem = new RelatedItem().WithId("relatedItem");
         _journalPage.AddRelatedItem(_relatedItem);
         _relatedItemNode = A.Fake<IRelatedItemNode>().WithId("relatedItem");
         _journalPageNode = A.Fake<IJournalPageNode>().WithId("journalPage");

         _journal.AddJournalPage(_journalPage);

         A.CallTo(() => _view.GetSelection()).Returns(new List<IBaseObject> { _relatedItemNode, _journalPageNode });
      }

      protected override void Because()
      {
         sut.DeleteSelection();
      }

      [Observation]
      public void should_use_the_task_to_remove_the_journal_page_from_the_journal()
      {
         A.CallTo(() => _journalTask.DeleteJournalPages(A<IReadOnlyList<JournalPage>>.That.Contains(_journalPage))).MustHaveHappened();
      }

      [Observation]
      public void the_journal_page_task_should_be_used_to_remove_the_related_item_from_the_journal()
      {
         A.CallTo(() => _journalPageTask.DeleteRelatedItemsFrom(_journal, A<IReadOnlyList<RelatedItem>>.That.Contains(_relatedItem))).MustHaveHappened();
      }
   }

   public class When_deleting_journal_pages : concern_for_JournalDiagramPresenter
   {
      private JournalPage _journalPage;
      private IJournalPageNode _journalPageNode;

      protected override void Context()
      {
         base.Context();
         _journalPage = new JournalPage().WithId("journalPage");
         _journal.AddJournalPage(_journalPage);
         _journalPageNode = A.Fake<IJournalPageNode>().WithId("journalPage");

         A.CallTo(() => _view.GetSelection()).Returns(new List<IBaseObject> {_journalPageNode});
      }

      protected override void Because()
      {
         sut.DeleteSelection();
      }

      [Observation]
      public void the_page_should_be_removed()
      {
         A.CallTo(() => _journalTask.DeleteJournalPages(A<IReadOnlyList<JournalPage>>.That.Contains(_journalPage))).MustHaveHappened();
      }
   }

   public class When_loading_related_items_back_into_the_project : concern_for_JournalDiagramPresenter
   {
      private RelatedItem _relatedItem;
      private IRelatedItemNode _relatedItemNode;
      private JournalPage _journalPage;

      protected override void Context()
      {
         base.Context();
         _relatedItem = new RelatedItem().WithId("relatedItem");
         _relatedItemNode = A.Fake<IRelatedItemNode>().WithId("relatedItem");
         _journalPage = new JournalPage();
         _journalPage.AddRelatedItem(_relatedItem);
         _journal.AddJournalPage(_journalPage);
      }

      protected override void Because()
      {
         sut.Load(new[] {_relatedItemNode});
      }

      [Observation]
      public void should_use_the_task_to_reload_the_related_items()
      {
         A.CallTo(() => _reloadRelatedItemTask.Load(_relatedItem)).MustHaveHappened();
      }
   }

   public class When_adding_journal_parent_links_to_journal_items : concern_for_JournalDiagramPresenter
   {
      private IBaseNode _childNode;
      private IBaseNode _parentNode;
      private JournalPage _childPage;
      private JournalPage _parentPage;

      protected override void Context()
      {
         base.Context();
         _childNode = A.Fake<IBaseNode>().WithId("childNode");
         _parentNode = A.Fake<IBaseNode>().WithId("parentNode");

         _childPage = new JournalPage().WithId("childNode");
         _journal.AddJournalPage(_childPage);
         _parentPage = new JournalPage().WithId("parentNode");
         _journal.AddJournalPage(_parentPage);
      }

      protected override void Because()
      {
         sut.AddParentLink(_childNode, _parentNode);
      }

      [Observation]
      public void journal_page_task_should_be_used_to_create_the_relationship()
      {
         A.CallTo(() => _journalPageTask.AddAsParentTo(_childPage, _parentPage)).MustHaveHappened();
      }

      [Observation]
      public void the_journal_diagram_should_be_saved()
      {
         A.CallTo(() => _journalTask.SaveJournalDiagram(_journalDiagram)).MustHaveHappened();
      }
   }

   public class When_showing_context_menu_for_node : concern_for_JournalDiagramPresenter
   {
      protected override void Because()
      {
         sut.ShowContextMenu(A.Fake<IBaseNode>() , new Point(), new PointF());
      }

      [Observation]
      public void should_call_context_menu_factory_when_journal_is_defined()
      {
         A.CallTo(() => _multipleBaseNodeContextMenuFactory.CreateFor(A<IReadOnlyList<IBaseNode>>._, sut)).MustHaveHappened();
      }
   }

   public class When_showing_context_menu_for_background : concern_for_JournalDiagramPresenter
   {
      protected override void Because()
      {
         sut.ShowContextMenu(new JournalDiagramBackground(), new Point());
      }

      [Observation]
      public void should_call_context_menu_factory_when_journal_is_defined()
      {
         A.CallTo(() => _viewItemContextMenuFactory.CreateFor(A<IViewItem>._, sut)).MustHaveHappened();
      }
   }

   public class When_showing_context_menu_for_node_when_journal_is_not_loaded : concern_for_JournalDiagramPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Handle(new JournalClosedEvent());
      }

      protected override void Because()
      {
         sut.ShowContextMenu(A.Fake<IBaseNode>(), new Point(), new PointF());
      }

      [Observation]
      public void should_not_call_context_menu_factory_when_journal_is_not_defined()
      {
         A.CallTo(() => _multipleBaseNodeContextMenuFactory.CreateFor(A<IReadOnlyList<IBaseNode>>._, sut)).MustNotHaveHappened();
      }
   }

   public class When_showing_context_menu_for_background_when_journal_is_not_loaded : concern_for_JournalDiagramPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Handle(new JournalClosedEvent());
      }

      protected override void Because()
      {
         sut.ShowContextMenu(new JournalDiagramBackground(), new Point());
      }

      [Observation]
      public void should_not_call_context_menu_factory_when_journal_is_not_defined()
      {
         A.CallTo(() => _viewItemContextMenuFactory.CreateFor(A<IViewItem>._, sut)).MustNotHaveHappened();
      }
   }
   
   public class When_handling_a_journal_closed_event : concern_for_JournalDiagramPresenter
   {
      protected override void Because()
      {
         sut.Handle(new JournalClosedEvent());
      }

      [Observation]
      public void should_clear_the_view()
      {
         A.CallTo(() => _journalDiagram.DiagramModel.Clear()).MustHaveHappened();
      }
   }

   public class When_handling_a_project_saving_event : concern_for_JournalDiagramPresenter
   {
      protected override void Because()
      {
         sut.Handle(new ProjectSavingEvent(A.Fake<IProject>()));
      }

      [Observation]
      public void should_save_the_journal_to_the_database()
      {
         A.CallTo(() => _journalTask.SaveJournalDiagram(_journalDiagram)).MustHaveHappened();
      }
   }

   public class When_handling_a_journal_item_removed_event : concern_for_JournalDiagramPresenter
   {
      protected override void Because()
      {
         sut.Handle(new JournalPageDeletedEvent(new JournalPage()));
      }

      [Observation]
      public void should_save_the_journal_to_the_database()
      {
         A.CallTo(() => _journalTask.SaveJournalDiagram(_journalDiagram)).MustHaveHappened();
      }
   }

   public class When_handling_a_journal_item_added_event : concern_for_JournalDiagramPresenter
   {
      protected override void Because()
      {
         sut.Handle(new JournalPageAddedEvent(new JournalPage()));
      }

      [Observation]
      public void should_save_the_journal_to_the_database()
      {
         A.CallTo(() => _journalTask.SaveJournalDiagram(_journalDiagram)).MustHaveHappened();
      }
   }
}
