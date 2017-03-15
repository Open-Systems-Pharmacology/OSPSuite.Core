using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Diagram;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.UI.Diagram.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Diagram.Managers
{
   public class JournalDiagramManager : BaseDiagramManager<SimpleContainerNode, SimpleNeighborhoodNode, JournalDiagram>, ILatchable
   {
      private readonly IDiagramToolTipCreator _toolTipCreator;
      public bool IsLatched { get; set; }

      public JournalDiagramManager(IDiagramToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
         CurrentInsertLocation = new PointF(JournalPageNode.NewNodeSize.Width, JournalPageNode.NewNodeSize.Height);
      }

      protected override void UpdateDiagramModel(JournalDiagram journalDiagram, IDiagramModel diagramModel, bool coupleAll)
      {
         var journal = journalDiagram.Journal;
         journalDiagram.Journal.JournalPages.OrderBy(page => page.UniqueIndex).Each(addJournalItem);

         removeJournalPageNodesWherePageWasRemoved(journal);

         removeRelatedItemNodesWherePageWasRemoved(journal);

         addAllRelatedItems(journal);

         addParentRelationLinks(journal);
      }

      private void addAllRelatedItems(Journal journal)
      {
         journal.JournalPages.Each(addRelatedItems);
      }

      private void addParentRelationLinks(Journal journal)
      {
         journal.JournalPages.Each(item =>
         {
            if (string.IsNullOrEmpty(item.ParentId))
               return;

            var childNode = DiagramModel.GetNode<JournalPageNode>(item.Id);
            var parentNode = DiagramModel.GetNode<JournalPageNode>(item.ParentId);
            if (parentNode != null)
               drawLinks(childNode, parentNode);
         });
      }

      private void removeJournalPageNodesWherePageWasRemoved(Journal journal)
      {
         findJournalPageNodesWherePageIsRemoved(journal).Each(node => { DiagramModel.RemoveNode(node.Id); });
      }

      private void removeRelatedItemNodesWherePageWasRemoved(Journal journal)
      {
         findRelatedItemNodesWhereItemIsRemoved(journal).Each(node => { DiagramModel.RemoveNode(node.Id); });
      }

      private IEnumerable<JournalPageNode> findJournalPageNodesWherePageIsRemoved(Journal journal)
      {
         return DiagramModel.GetAllChildren<JournalPageNode>().Where(node => journal.JournalPageById(node.Id) == null);
      }

      private IEnumerable<RelatedItemNode> findRelatedItemNodesWhereItemIsRemoved(Journal journal)
      {
         return DiagramModel.GetAllChildren<RelatedItemNode>().Where(node => journal.RelatedItemdById(node.Id) == null);
      }

      private void drawLinks(JournalPageNode childNode, JournalPageNode parentNode)
      {
         this.DoWithinLatch(() => { redrawLinks(childNode, parentNode); });
      }

      protected override void DecoupleModel()
      {
      }

      protected override bool MustHandleNew(IObjectBase obj)
      {
         return false;
      }

      private void addLink(JournalPageNode parentNode, JournalPageNode childNode)
      {
         var link = new JournalPageLink();
         link.Initialize(childNode, parentNode);
      }

      /// <summary>
      /// Returns a point which is to the immediate left of the <paramref name="lastInsertedNode"/>
      /// </summary>
      public PointF NextInsertLocationRelativeTo(JournalPageNode lastInsertedNode)
      {
         // Some magic constants here. 7 and 3 are derived by trial and error
         return new PointF(lastInsertedNode.Center.X - 7 + 2 * lastInsertedNode.Width, lastInsertedNode.Center.Y - 3);
      }

      private void addRelatedItems(JournalPage page)
      {
         var journalPageNode = DiagramModel.GetNode<JournalPageNode>(page.Id);

         page.RelatedItems.Each(relatedItem => addRelatedItem(journalPageNode, relatedItem, findLowestRelatedItem(page)));
         if(!journalPageNode.IsExpanded)
            journalPageNode.Collapse();
      }

      private RelatedItemNode findLowestRelatedItem(JournalPage page)
      {
         var nodes = page.RelatedItems.Select(item => DiagramModel.GetNode<RelatedItemNode>(item.Id)).Where(node => node != null).ToList();
         return !nodes.Any() ? null : nodes.Aggregate((a, b) => a.Location.Y > b.Location.Y ? a : b);
      }

      private void addRelatedItem(JournalPageNode journalPageNode, RelatedItem item, RelatedItemNode lowestRelatedItemNode)
      {
         var relatedItemNode = DiagramModel.GetNode<RelatedItemNode>(item.Id);
         if (relatedItemNode == null)
         {
            relatedItemNode = DiagramModel.CreateNode<RelatedItemNode>(item.Id, journalPageNode.GetNextRelatedItemLocation(lowestRelatedItemNode), DiagramModel);
         }
         relatedItemNode.SetColorFrom(DiagramOptions.DiagramColors);
         linkRelatedNodes(journalPageNode, relatedItemNode);
         relatedItemNode.UpdateAttributesFromItem(item);
         relatedItemNode.ToolTipText = _toolTipCreator.GetToolTipFor(item);
      }

      private void linkRelatedNodes(ElementBaseNode linkedToNode, RelatedItemNode relatedItemNode)
      {
         var link = new RelatedItemLink();
         link.Initialize(linkedToNode, relatedItemNode);
      }

      private void addJournalItem(JournalPage page)
      {
         var journalPageNode = DiagramModel.GetNode<JournalPageNode>(page.Id) ?? addNewPageNodeFor(page);

         journalPageNode.ToolTipText = _toolTipCreator.GetToolTipFor(page);
         journalPageNode.UpdateAttributesFrom(page);
      }

      private JournalPageNode addNewPageNodeFor(JournalPage page)
      {
         var lastNode = getLastJournalNode();
         var insertLocation = lastNode != null ? NextInsertLocationRelativeTo(lastNode) : GetNextInsertLocation();
         var journalPageNode = DiagramModel.CreateNode<JournalPageNode>(page.Id, insertLocation, DiagramModel);
         return journalPageNode;
      }

      private void redrawLinks(JournalPageNode childNode, JournalPageNode parentNode)
      {
         DiagramModel.BeginUpdate();
         try
         {
            childNode.ClearParentLinks();
            addLink(childNode, parentNode);
         }
         finally
         {
            DiagramModel.EndUpdate();
         }
      }

      private JournalPageNode getLastJournalNode()
      {
         var allChildren = DiagramModel.GetAllChildren<JournalPageNode>().ToList();

         return !allChildren.Any() ? null : allChildren.OrderBy(x => x.UniqueIndex).Last();
      }

      public override IDiagramManager<JournalDiagram> Create()
      {
         return  new JournalDiagramManager(_toolTipCreator);
      }
   }
}