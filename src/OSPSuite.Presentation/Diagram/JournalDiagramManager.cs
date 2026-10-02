using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Diagram.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram
{
   public class JournalDiagramManager : BaseDiagramManager<ContainerNode, NeighborhoodNode, JournalDiagram>, ILatchable
   {
      private const float NEXT_PAGE_OFFSET_X = -7F;
      private const float NEXT_PAGE_OFFSET_Y = -3F;
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
         journal.JournalPages.OrderBy(page => page.UniqueIndex).Each(addJournalItem);
         removeJournalPageNodesWherePageWasRemoved(journal);
         removeRelatedItemNodesWhereItemWasRemoved(journal);
         journal.JournalPages.Each(addRelatedItems);
         addParentRelationLinks(journal);
      }

      private void addParentRelationLinks(Journal journal)
      {
         journal.JournalPages.Where(page => !string.IsNullOrEmpty(page.ParentId)).Each(page =>
         {
            var childNode = DiagramModel.GetNode<JournalPageNode>(page.Id);
            var parentNode = DiagramModel.GetNode<JournalPageNode>(page.ParentId);
            if (parentNode != null)
               this.DoWithinLatch(() => redrawLinks(childNode, parentNode));
         });
      }

      private void removeJournalPageNodesWherePageWasRemoved(Journal journal)
      {
         DiagramModel.GetAllChildren<JournalPageNode>().Where(node => journal.JournalPageById(node.Id) == null).ToList().Each(node => DiagramModel.RemoveNode(node.Id));
      }

      private void removeRelatedItemNodesWhereItemWasRemoved(Journal journal)
      {
         DiagramModel.GetAllChildren<RelatedItemNode>().Where(node => journal.RelatedItemdById(node.Id) == null).ToList().Each(node => DiagramModel.RemoveNode(node.Id));
      }

      protected override void DecoupleModel()
      {
      }

      protected override bool MustHandleNew(IObjectBase obj) => false;

      public PointF NextInsertLocationRelativeTo(JournalPageNode lastInsertedNode)
      {
         return new PointF(lastInsertedNode.Center.X + NEXT_PAGE_OFFSET_X + 2 * lastInsertedNode.Size.Width, lastInsertedNode.Center.Y + NEXT_PAGE_OFFSET_Y);
      }

      private void addRelatedItems(JournalPage page)
      {
         var journalPageNode = DiagramModel.GetNode<JournalPageNode>(page.Id);
         page.RelatedItems.Each(relatedItem => addRelatedItem(journalPageNode, relatedItem, findLowestRelatedItem(page)));
      }

      private RelatedItemNode findLowestRelatedItem(JournalPage page)
      {
         var nodes = page.RelatedItems.Select(item => DiagramModel.GetNode<RelatedItemNode>(item.Id)).Where(node => node != null).ToList();
         return !nodes.Any() ? null : nodes.Aggregate((a, b) => a.Location.Y > b.Location.Y ? a : b);
      }

      private void addRelatedItem(JournalPageNode journalPageNode, RelatedItem item, RelatedItemNode lowestRelatedItemNode)
      {
         var relatedItemNode = DiagramModel.GetNode<RelatedItemNode>(item.Id) ?? DiagramModel.CreateNode<RelatedItemNode>(item.Id, journalPageNode.GetNextRelatedItemLocation(lowestRelatedItemNode), DiagramModel);
         if (!journalPageNode.RelatedItemNodes.Contains(relatedItemNode))
            new RelatedItemLink().Initialize(journalPageNode, relatedItemNode);

         relatedItemNode.SetColorFrom(DiagramOptions.DiagramColors);
         relatedItemNode.UpdateAttributesFromItem(item);
         relatedItemNode.Description = _toolTipCreator.GetToolTipFor(item);
      }

      private void addJournalItem(JournalPage page)
      {
         var journalPageNode = DiagramModel.GetNode<JournalPageNode>(page.Id) ?? addNewPageNodeFor(page);
         journalPageNode.Description = _toolTipCreator.GetToolTipFor(page);
         journalPageNode.UpdateAttributesFrom(page);
         journalPageNode.SetColorFrom(DiagramOptions.DiagramColors);
      }

      private JournalPageNode addNewPageNodeFor(JournalPage page)
      {
         var lastNode = getLastJournalNode();
         var insertLocation = lastNode != null ? NextInsertLocationRelativeTo(lastNode) : GetNextInsertLocation();
         return DiagramModel.CreateNode<JournalPageNode>(page.Id, insertLocation, DiagramModel);
      }

      private void redrawLinks(JournalPageNode childNode, JournalPageNode parentNode)
      {
         DiagramModel.BeginUpdate();
         try
         {
            childNode.ClearParentLinks();
            var link = new JournalPageLink();
            link.Initialize(parentNode, childNode);
            link.SetColorFrom(DiagramOptions.DiagramColors);
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

      public override IDiagramManager<JournalDiagram> Create() => new JournalDiagramManager(_toolTipCreator);
   }
}
