using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views.Diagram;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Journal
{
   public partial class JournalDiagramView : BaseDiagramView, IJournalDiagramView
   {
      private IJournalDiagramPresenter _journalDiagramPresenter;

      public JournalDiagramView(IImageListRetriever imageListRetriever)
         : base(imageListRetriever)
      {
         InitializeComponent();
      }

      protected override void OnContextClicked(GoObjectEventArgs e)
      {
         var nodes = _goView.Selection.All().ToList();
         if (nodes.All(x => x.IsAnImplementationOf<IBaseNode>()))
            _journalDiagramPresenter.ShowContextMenu(nodes.Cast<IBaseNode>().ToList(), e.ViewPoint);
         else
            base.OnContextClicked(e);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _goView.ObjectDoubleClicked += (o, e) => OnEvent(onDoubleClicked, e);
         _goView.LinkCreated += (o, e) => OnEvent(onLinkCreated, e);
         _goView.SelectionDeleting += (o, e) => OnEvent(onSelectionDeleting, e);
         _goView.BackgroundContextClicked += (o, e) => OnEvent(onBackgroundContextClicked, e);
      }

      private void onDoubleClicked(GoObjectEventArgs e)
      {
         var journalPageNode = e.GoObject?.ParentNode as JournalPageNode;
         if (journalPageNode == null)
            return;

         _journalDiagramPresenter.EditJournalPage(journalPageNode);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         _goView.GridCellSize = new SizeF(10, 10);
         _goView.ReplaceMouseTool(typeof(GoToolDragging), new JournalPageDraggingTool(_goView));
      }

      private void onBackgroundContextClicked(GoInputEventArgs goInputEventArgs)
      {
         _journalDiagramPresenter.ShowContextMenu(new JournalDiagramBackground(), goInputEventArgs.ViewPoint);
      }

      private void onLinkCreated(GoSelectionEventArgs e)
      {
         var link = (NewLink) e.GoObject;
         var node1 = (IBaseNode) link.FromNode;
         var node2 = (IBaseNode) link.ToNode;

         var parentNode = getParentNodeFromLink(link);
         var childNode = parentNode == node1 ? node2 : node1;

         _journalDiagramPresenter.AddParentLink(childNode, parentNode);
         _goView.Document.Remove(link);
      }

      private JournalPageNode getParentNodeFromLink(IGoLink goLink)
      {
         if (goLink.FromPort == null || goLink.ToPort == null)
            return null;

         // connecting to the child port of the other node (means the other node is the parent)
         if (goLink.FromPort is JournalChildPort)
            return (JournalPageNode) goLink.FromPort.Node;

         return (JournalPageNode) goLink.ToPort.Node;
      }

      private void onSelectionDeleting(CancelEventArgs e)
      {
         _journalDiagramPresenter.DeleteSelection();
         // Cancel the event because decision to delete is made by user
         // but further event processing removes the node in all cases.
         e.Cancel = true;
      }

      public List<IBaseObject> GetSelection()
      {
         return _goView.Selection.OfType<IBaseObject>().ToList();
      }

      public void AttachPresenter(IJournalDiagramPresenter presenter)
      {
         base.AttachPresenter(presenter);
         _journalDiagramPresenter = presenter;
      }

      public void RefreshDiagram()
      {
         _goView.Refresh();
      }

      public void RemoveSelectionHandles()
      {
         _goView.Selection.RemoveAllSelectionHandles();
      }
   }
}