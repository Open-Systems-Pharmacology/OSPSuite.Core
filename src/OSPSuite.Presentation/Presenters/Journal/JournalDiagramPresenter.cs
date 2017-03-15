using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Events;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Diagram;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using IDialogCreator = OSPSuite.Core.Services.IDialogCreator;

namespace OSPSuite.Presentation.Presenters.Journal
{
   public interface IJournalDiagramPresenter : IPresenter<IJournalDiagramView>,
      IBaseDiagramPresenter,
      IPresenterWithContextMenu<IReadOnlyList<IBaseNode>>,
      IPresenterWithContextMenu<IViewItem>,
      IListener<JournalLoadedEvent>,
      IListener<JournalPageAddedEvent>,
      IListener<JournalPageDeletedEvent>,
      IListener<JournalClosedEvent>,
      IListener<JournalPageUpdatedEvent>,
      IListener<ProjectSavingEvent>
   {
      void SaveDiagram();

      /// <summary>
      /// Loads related items back into the project
      /// </summary>
      /// <param name="relatedItemNodes">The item to load</param>
      void Load(IReadOnlyList<IRelatedItemNode> relatedItemNodes);

      /// <summary>
      /// Starts the editor for a journal page
      /// </summary>
      /// <param name="journalPageNode">The page to be edited</param>
      void EditJournalPage(IJournalPageNode journalPageNode);

      /// <summary>
      /// Compare two related items
      /// </summary>
      /// <param name="relatedItemNodes">Items to be compared. Only the first two items in the list are used</param>
      void Compare(IReadOnlyList<IRelatedItemNode> relatedItemNodes);

      void AddParentLink(IBaseNode child, IBaseNode parent);

      /// <summary>
      /// Removes the current selection of nodes from the view
      /// </summary>
      void DeleteSelection();

      /// <summary>
      /// Places page nodes in chronological order on the diagram. Does not affect links
      /// </summary>
      void RestoreChronologicalOrder();

      /// <summary>
      /// Copy entire diagram to clipboard as bitmap
      /// </summary>
      void CopyDiagramToClipboard();

      void HideRelatedItems();
      void ShowRelatedItems();
   }

   public class JournalDiagramPresenter : BaseDiagramPresenter<IJournalDiagramView, IJournalDiagramPresenter, JournalDiagram>,
      IJournalDiagramPresenter
   
   {
      private readonly IPresentationUserSettings _userSettings;

      private readonly IJournalTask _journalTask;
      private readonly IJournalPageTask _journalPageTask;
      private readonly IReloadRelatedItemTask _reloadRelatedItemTask;
      private readonly IJournalComparisonTask _journalComparisonTask;
      private readonly IMultipleBaseNodeContextMenuFactory _multipleNodeContextMenuFactory;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private JournalDiagram _journalDiagram;

      public JournalDiagramPresenter(
         IJournalDiagramView view, IContainerBaseLayouter layouter, IDialogCreator dialogCreator,
         IDiagramModelFactory diagramModelFactory,
         IPresentationUserSettings userSettings, IJournalTask journalTask,
         IJournalPageTask journalPageTask, IReloadRelatedItemTask reloadRelatedItemTask,
         IJournalComparisonTask journalComparisonTask, IMultipleBaseNodeContextMenuFactory multipleNodeContextMenuFactory, 
         IViewItemContextMenuFactory viewItemContextMenuFactory)
         : base(view, layouter, dialogCreator, diagramModelFactory)
      {
         _userSettings = userSettings;
         _journalTask = journalTask;
         _journalPageTask = journalPageTask;
         _reloadRelatedItemTask = reloadRelatedItemTask;
         _journalComparisonTask = journalComparisonTask;
         _multipleNodeContextMenuFactory = multipleNodeContextMenuFactory;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
      }

      public void ShowContextMenu(IReadOnlyList<IBaseNode> baseNodes, Point popupLocation, PointF locationInDiagramView)
      {
         ShowContextMenu(baseNodes, popupLocation);
      }

      public override void ShowContextMenu(IBaseNode baseNode, Point popupLocation, PointF locationInDiagramView)
      {
         ShowContextMenu(new[] { baseNode }, popupLocation, locationInDiagramView);
      }

      protected override IDiagramOptions GetDiagramOptions()
      {
         return _userSettings.DiagramOptions;
      }

      public void Handle(JournalLoadedEvent eventToHandle)
      {
         _journalDiagram = eventToHandle.Journal.Diagram;
         Edit(_journalDiagram);
      }

      private void removeParentLink(IBaseNode child)
      {
         var journalPage = getJournalPageFor(child);
         if (journalPage != null && !string.IsNullOrEmpty(journalPage.ParentId))
            _journalPageTask.DeleteParentFrom(journalPage);

         _journalTask.SaveJournalDiagram(_journalDiagram);
      }

      private JournalPage getJournalPageFor(IBaseNode node)
      {
         return _journalDiagram.Journal.JournalPageById(node.Id);
      }

      public void AddParentLink(IBaseNode child, IBaseNode parent)
      {
         _journalPageTask.AddAsParentTo(getJournalPageFor(child), getJournalPageFor(parent));
         _journalTask.SaveJournalDiagram(_journalDiagram.Journal.Diagram);
      }

      public void DeleteSelection()
      {
         var baseObjects = _view.GetSelection();
         deleteLinks(baseObjects.OfType<IJournalPageLink>().ToList());
         deleteRelatedItems(baseObjects.OfType<IRelatedItemNode>().ToList());
         deleteJournalPages(baseObjects.OfType<IJournalPageNode>().ToList());
         _view.RemoveSelectionHandles();
      }

      private void deleteLinks(IEnumerable<IJournalPageLink> journalPageLinks)
      {
         journalPageLinks.Each(baseObject =>
         {
            var baseLink = baseObject;
            var child = baseLink.GetFromNode();
            removeParentLink(child);
            baseLink.Unlink();
         });
      }

      public void RestoreChronologicalOrder()
      {
         if (_journalDiagram == null || !confirmRestoreDiagramLayout()) return;
         DiagramModel.Clear();
         refreshAndSave();
      }

      private bool confirmRestoreDiagramLayout()
      {
         return _dialogCreator.MessageBoxYesNo(Captions.ConfirmJournalDiagramRestoreLayout) == ViewResult.Yes;
      }

      public void CopyDiagramToClipboard()
      {
         CopyBitmapToClipboard(DiagramModel);
      }

      public void HideRelatedItems()
      {
         DiagramModel.GetAllChildren<IJournalPageNode>().Each(node => node.CollapseRelatedItems());
      }

      public void ShowRelatedItems()
      {
         DiagramModel.GetAllChildren<IJournalPageNode>().Each(node => node.ExpandRelatedItems());
      }

      private void refreshAndSave()
      {
         Refresh();
         SaveDiagram();
      }

      public void Handle(JournalPageAddedEvent eventToHandle)
      {
         refreshAndSave();
      }

      public void SaveDiagram()
      {
         if (_journalDiagram == null) return;
         _journalTask.SaveJournalDiagram(_journalDiagram);
      }

      private void deleteRelatedItems(IReadOnlyList<IRelatedItemNode> relatedItems)
      {
         _journalPageTask.DeleteRelatedItemsFrom(_journalDiagram.Journal, relatedItems.Select(relatedItemFrom).ToList());
         _journalDiagram.DiagramModel.ClearUndoStack();
         refreshAndSave();
      }

      public void Compare(IReadOnlyList<IRelatedItemNode> relatedItemNode)
      {
         _journalComparisonTask.StartComparison(relatedItemFrom(relatedItemNode[0]), relatedItemFrom(relatedItemNode[1]));
      }

      public void Load(IReadOnlyList<IRelatedItemNode> relatedItemNodes)
      {
         relatedItemNodes.Each(node => _reloadRelatedItemTask.Load(relatedItemFrom(node)));
      }

      private void deleteJournalPages(IReadOnlyList<IJournalPageNode> journalPageNodes)
      {
         _journalTask.DeleteJournalPages(journalPageNodes.Select(journalPageFrom).ToList());
         _journalDiagram.DiagramModel.ClearUndoStack();
      }

      private JournalPage journalPageFrom(IJournalPageNode journalPageNode)
      {
         return _journalDiagram.Journal.JournalPageById(journalPageNode.Id);
      }

      public void EditJournalPage(IJournalPageNode journalPageNode)
      {
         _journalTask.Edit(journalPageFrom(journalPageNode), showEditor: true);
      }

      private RelatedItem relatedItemFrom(IRelatedItemNode relatedItemNode)
      {
         return _journalDiagram.Journal.RelatedItemdById(relatedItemNode.Id);
      }

      public void Handle(JournalPageDeletedEvent eventToHandle)
      {
         refreshAndSave();
         _view.RemoveSelectionHandles();
      }

      public void Handle(JournalClosedEvent eventToHandle)
      {
         if (_journalDiagram == null) return;

         _journalDiagram.DiagramModel.Clear();
         _journalDiagram = null;
      }

      public void Handle(JournalPageUpdatedEvent eventToHandle)
      {
         Refresh();
      }

      public void Handle(ProjectSavingEvent eventToHandle)
      {
         SaveDiagram();
      }

      public void ShowContextMenu(IReadOnlyList<IBaseNode> baseNodes, Point popupLocation)
      {
         if (_journalDiagram == null) return;

         _multipleNodeContextMenuFactory.CreateFor(baseNodes, this).Show(_view, popupLocation);
      }

      public void ShowContextMenu(IViewItem viewItem, Point popupLocation)
      {
         if (_journalDiagram == null) return;

         _viewItemContextMenuFactory.CreateFor(viewItem, this).Show(_view, popupLocation);
      }
   }
}
