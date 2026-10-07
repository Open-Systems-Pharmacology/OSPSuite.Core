using System;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Diagram.Services;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_JournalDiagramManager : ContextSpecification<JournalDiagramManager>
   {
      protected IDiagramToolTipCreator _toolTipCreator;
      protected Journal _journal;
      protected JournalDiagram _journalDiagram;
      protected IDiagramModel _diagramModel;
      protected DiagramOptions _diagramOptions;
      protected DiagramColors _colors;
      protected JournalPage _page1;
      protected JournalPage _page2;
      protected JournalPage _page3;
      protected RelatedItem _item1;
      protected RelatedItem _item2;

      protected override void Context()
      {
         _toolTipCreator = A.Fake<IDiagramToolTipCreator>();
         A.CallTo(() => _toolTipCreator.GetToolTipFor(A<JournalPage>._)).ReturnsLazily((JournalPage page) => $"tooltip for {page.Title}");
         A.CallTo(() => _toolTipCreator.GetToolTipFor(A<RelatedItem>._)).ReturnsLazily((RelatedItem item) => $"tooltip for {item.Name}");

         _journal = new Journal();
         _journalDiagram = new JournalDiagram {DiagramModel = new DiagramModel()};
         _journal.AddDiagram(_journalDiagram);
         _diagramModel = _journalDiagram.DiagramModel;

         _colors = new DiagramColors
         {
            JournalPageNode = Color.Red,
            JournalPagePort = Color.Blue,
            JournalPageLink = Color.Green,
            RelatedItemNode = Color.Yellow,
            RelatedItemLink = Color.Purple,
            BorderUnfixed = Color.Gray
         };
         _diagramOptions = new DiagramOptions {DiagramColors = _colors};

         _page2 = createPage("page-2", 2, "Second page", parentId: "page-1");
         _page1 = createPage("page-1", 1, "First page");
         _page3 = createPage("page-3", 3, "Third page", parentId: "page-2");
         _item1 = addRelatedItem(_page1, "item-1", "Sim 1", "Simulation");
         _item2 = addRelatedItem(_page1, "item-2", "Ind 1", "Individual");

         sut = new JournalDiagramManager(_toolTipCreator);
         sut.InitializeWith(_journalDiagram, _diagramOptions);
      }

      protected JournalPage createPage(string id, int uniqueIndex, string title, string parentId = null)
      {
         var page = new JournalPage
         {
            Id = id,
            UniqueIndex = uniqueIndex,
            Title = title,
            ParentId = parentId,
            CreatedAt = new DateTime(2024, 3, 5, 10, 30, 0, DateTimeKind.Utc)
         };
         _journal.AddJournalPage(page);
         return page;
      }

      protected static RelatedItem addRelatedItem(JournalPage page, string id, string name, string itemType)
      {
         var item = new RelatedItem {Id = id, Name = name, ItemType = itemType};
         page.AddRelatedItem(item);
         return item;
      }

      protected JournalPageNode nodeFor(JournalPage page) => _diagramModel.GetNode<JournalPageNode>(page.Id);

      protected RelatedItemNode nodeFor(RelatedItem item) => _diagramModel.GetNode<RelatedItemNode>(item.Id);

      protected static JournalPageLink parentLinkOf(JournalPageNode childNode) => childNode.Links.OfType<JournalPageLink>().SingleOrDefault(link => link.ChildNode == childNode);
   }

   public class When_initializing_the_journal_diagram_manager : concern_for_JournalDiagramManager
   {
      [Observation]
      public void should_be_initialized_with_the_diagram_and_the_options()
      {
         sut.IsInitialized.ShouldBeTrue();
         sut.PkModel.ShouldBeEqualTo(_journalDiagram);
         sut.DiagramOptions.ShouldBeEqualTo(_diagramOptions);
         _diagramModel.DiagramOptions.ShouldBeEqualTo(_diagramOptions);
      }

      [Observation]
      public void should_create_one_page_node_per_page_in_unique_index_order_at_the_next_insert_locations()
      {
         _diagramModel.GetAllChildren<JournalPageNode>().Count().ShouldBeEqualTo(3);
         nodeFor(_page1).Location.ShouldBeEqualTo(new PointF(120, 65));
         nodeFor(_page2).Location.ShouldBeEqualTo(new PointF(353, 62));
         nodeFor(_page3).Location.ShouldBeEqualTo(new PointF(586, 59));
         nodeFor(_page1).GetParent().ShouldBeEqualTo(_diagramModel);
      }

      [Observation]
      public void should_compute_the_next_insert_location_to_the_right_of_the_last_page()
      {
         sut.NextInsertLocationRelativeTo(nodeFor(_page1)).ShouldBeEqualTo(new PointF(120 - 7 + 240, 65 - 3));
      }

      [Observation]
      public void should_update_the_page_nodes_from_the_pages()
      {
         var node = nodeFor(_page1);
         node.UniqueIndex.ShouldBeEqualTo(1);
         node.Name.ShouldBeEqualTo("First page");
         node.Description.ShouldBeEqualTo("tooltip for First page");
         var date = new DateTimeFormatter().Format(_page1.CreatedAt);
         node.Text.ShouldBeEqualTo($"First page{Environment.NewLine}{Environment.NewLine}(1)             {date}{Environment.NewLine}");
      }

      [Observation]
      public void should_create_the_related_item_nodes_below_the_page()
      {
         nodeFor(_item1).Location.ShouldBeEqualTo(new PointF(80, 127.5F));
         nodeFor(_item2).Location.ShouldBeEqualTo(new PointF(80, 167.5F));
         nodeFor(_item1).GetParent().ShouldBeEqualTo(_diagramModel);
      }

      [Observation]
      public void should_update_the_related_item_nodes_from_the_items()
      {
         nodeFor(_item1).Name.ShouldBeEqualTo("  Sim 1 (Simulation)");
         nodeFor(_item1).Description.ShouldBeEqualTo("tooltip for Sim 1");
      }

      [Observation]
      public void should_link_each_related_item_once_to_its_page()
      {
         nodeFor(_page1).RelatedItemNodes.ShouldOnlyContain(nodeFor(_item1), nodeFor(_item2));
         nodeFor(_item1).PageNode.ShouldBeEqualTo(nodeFor(_page1));
         nodeFor(_item1).Links.Count.ShouldBeEqualTo(1);
         nodeFor(_page2).HasRelatedItems.ShouldBeFalse();
      }

      [Observation]
      public void should_link_the_child_pages_to_their_parents_from_parent_to_child()
      {
         nodeFor(_page1).ParentPageNode.ShouldBeNull();
         nodeFor(_page2).ParentPageNode.ShouldBeEqualTo(nodeFor(_page1));
         nodeFor(_page3).ParentPageNode.ShouldBeEqualTo(nodeFor(_page2));

         var link = parentLinkOf(nodeFor(_page2));
         link.GetFromNode().ShouldBeEqualTo(nodeFor(_page1));
         link.GetToNode().ShouldBeEqualTo(nodeFor(_page2));
         link.ParentNode.ShouldBeEqualTo(nodeFor(_page1));
         link.ChildNode.ShouldBeEqualTo(nodeFor(_page2));
         _diagramModel.GetAllChildren<JournalPageLink>().Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_color_the_nodes_and_links_from_the_diagram_options()
      {
         nodeFor(_page1).FillColor.ShouldBeEqualTo(_colors.JournalPageNode);
         nodeFor(_page1).PortColor.ShouldBeEqualTo(_colors.JournalPagePort);
         nodeFor(_page1).BorderColor.ShouldBeEqualTo(_colors.BorderUnfixed);
         nodeFor(_page1).BorderWidth.ShouldBeEqualTo(1F);
         nodeFor(_item1).FillColor.ShouldBeEqualTo(_colors.RelatedItemNode);
         nodeFor(_item1).PortColor.ShouldBeEqualTo(_colors.RelatedItemNode);
         parentLinkOf(nodeFor(_page2)).Color.ShouldBeEqualTo(_colors.JournalPageLink);
         nodeFor(_item1).Links.OfType<RelatedItemLink>().Single().Color.ShouldBeEqualTo(_colors.RelatedItemLink);
      }

      [Observation]
      public void should_not_be_latched_after_the_links_were_redrawn()
      {
         sut.IsLatched.ShouldBeFalse();
      }
   }

   public class When_refreshing_the_journal_diagram_without_changes : concern_for_JournalDiagramManager
   {
      private JournalPageNode _page1Node;
      private RelatedItemNode _item1Node;

      protected override void Context()
      {
         base.Context();
         _page1Node = nodeFor(_page1);
         _item1Node = nodeFor(_item1);
         _page1Node.Location = new PointF(500, 500);
      }

      protected override void Because()
      {
         sut.RefreshDiagramFromModel();
      }

      [Observation]
      public void should_keep_the_existing_nodes_and_their_locations()
      {
         nodeFor(_page1).ShouldBeEqualTo(_page1Node);
         nodeFor(_item1).ShouldBeEqualTo(_item1Node);
         _page1Node.Location.ShouldBeEqualTo(new PointF(500, 500));
         _diagramModel.GetAllChildren<IBaseNode>().Count().ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_not_duplicate_the_related_item_links()
      {
         _page1Node.Links.OfType<RelatedItemLink>().Count().ShouldBeEqualTo(2);
         _item1Node.Links.Count.ShouldBeEqualTo(1);
         _diagramModel.GetAllChildren<RelatedItemLink>().Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_redraw_the_parent_links_without_duplicates()
      {
         _diagramModel.GetAllChildren<JournalPageLink>().Count().ShouldBeEqualTo(2);
         nodeFor(_page2).Links.OfType<JournalPageLink>().Count().ShouldBeEqualTo(2);
         nodeFor(_page3).Links.OfType<JournalPageLink>().Count().ShouldBeEqualTo(1);
         nodeFor(_page2).ParentPageNode.ShouldBeEqualTo(_page1Node);
      }
   }

   public class When_refreshing_the_journal_diagram_after_adding_a_page_and_a_related_item : concern_for_JournalDiagramManager
   {
      private JournalPage _page4;
      private RelatedItem _item3;

      protected override void Context()
      {
         base.Context();
         nodeFor(_page3).Location = new PointF(1000, 500);
      }

      protected override void Because()
      {
         _page4 = createPage("page-4", 4, "Fourth page", parentId: "page-3");
         _item3 = addRelatedItem(_page2, "item-3", "Pop 1", "Population");
         sut.RefreshDiagramFromModel();
      }

      [Observation]
      public void should_place_the_new_page_relative_to_the_page_with_the_highest_unique_index()
      {
         nodeFor(_page4).Location.ShouldBeEqualTo(new PointF(1000 - 7 + 240, 500 - 3));
         nodeFor(_page4).Name.ShouldBeEqualTo("Fourth page");
         nodeFor(_page4).ParentPageNode.ShouldBeEqualTo(nodeFor(_page3));
      }

      [Observation]
      public void should_place_the_new_related_item_below_its_page()
      {
         nodeFor(_item3).Location.ShouldBeEqualTo(new PointF(313, 124.5F));
         nodeFor(_page2).RelatedItemNodes.ShouldOnlyContain(nodeFor(_item3));
         nodeFor(_item3).FillColor.ShouldBeEqualTo(_colors.RelatedItemNode);
      }
   }

   public class When_refreshing_the_journal_diagram_after_removing_a_page_and_a_related_item : concern_for_JournalDiagramManager
   {
      protected override void Because()
      {
         _journal.Remove(_page3);
         _page1.RemoveRelatedItem(_item1);
         sut.RefreshDiagramFromModel();
      }

      [Observation]
      public void should_remove_the_node_and_the_links_of_the_removed_page()
      {
         _diagramModel.GetNode(_page3.Id).ShouldBeNull();
         nodeFor(_page2).Links.OfType<JournalPageLink>().Count().ShouldBeEqualTo(1);
         _diagramModel.GetAllChildren<JournalPageLink>().Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_remove_the_node_and_the_link_of_the_removed_item()
      {
         _diagramModel.GetNode(_item1.Id).ShouldBeNull();
         nodeFor(_page1).RelatedItemNodes.ShouldOnlyContain(nodeFor(_item2));
         nodeFor(_item2).Location.ShouldBeEqualTo(new PointF(80, 167.5F));
      }
   }

   public class When_refreshing_the_journal_diagram_after_the_parent_of_a_page_changed : concern_for_JournalDiagramManager
   {
      protected override void Because()
      {
         _page3.ParentId = _page1.Id;
         sut.RefreshDiagramFromModel();
      }

      [Observation]
      public void should_replace_the_parent_link_of_the_page()
      {
         nodeFor(_page3).ParentPageNode.ShouldBeEqualTo(nodeFor(_page1));
         nodeFor(_page3).Links.OfType<JournalPageLink>().Count().ShouldBeEqualTo(1);
         nodeFor(_page2).Links.OfType<JournalPageLink>().Count().ShouldBeEqualTo(1);
         _diagramModel.GetAllChildren<JournalPageLink>().Count().ShouldBeEqualTo(2);
      }
   }

   public class When_refreshing_the_journal_diagram_with_a_page_whose_parent_is_unknown : concern_for_JournalDiagramManager
   {
      private JournalPage _orphan;

      protected override void Because()
      {
         _orphan = createPage("page-orphan", 5, "Orphan", parentId: "missing");
         sut.RefreshDiagramFromModel();
      }

      [Observation]
      public void should_create_the_page_node_without_a_parent_link()
      {
         nodeFor(_orphan).ShouldNotBeNull();
         nodeFor(_orphan).ParentPageNode.ShouldBeNull();
         _diagramModel.GetAllChildren<JournalPageLink>().Count().ShouldBeEqualTo(2);
      }
   }

   public class When_refreshing_the_journal_diagram_after_adding_an_item_to_a_collapsed_page : concern_for_JournalDiagramManager
   {
      private RelatedItem _item3;

      protected override void Context()
      {
         base.Context();
         nodeFor(_page1).CollapseRelatedItems();
      }

      protected override void Because()
      {
         _item3 = addRelatedItem(_page1, "item-3", "Pop 1", "Population");
         sut.RefreshDiagramFromModel();
      }

      [Observation]
      public void should_keep_the_page_collapsed_and_hide_the_new_item()
      {
         nodeFor(_page1).IsExpanded.ShouldBeFalse();
         nodeFor(_item3).Visible.ShouldBeFalse();
         nodeFor(_item1).Visible.ShouldBeFalse();
         nodeFor(_item3).Location.ShouldBeEqualTo(new PointF(80, 207.5F));
      }
   }

   public class When_creating_a_new_journal_diagram_manager : concern_for_JournalDiagramManager
   {
      private IDiagramManager<JournalDiagram> _newManager;

      protected override void Because()
      {
         _newManager = sut.Create();
      }

      [Observation]
      public void should_return_a_new_uninitialized_manager_starting_at_the_first_page_location()
      {
         _newManager.ShouldBeAnInstanceOf<JournalDiagramManager>();
         ReferenceEquals(_newManager, sut).ShouldBeFalse();
         _newManager.IsInitialized.ShouldBeFalse();
         _newManager.PkModel.ShouldBeNull();
         ((JournalDiagramManager) _newManager).CurrentInsertLocation.ShouldBeEqualTo(new PointF(120, 65));
      }
   }
}
