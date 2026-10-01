using System;
using System.Drawing;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_JournalPageNode : ContextSpecification<JournalPageNode>
   {
      protected DiagramModel _model;
      protected RelatedItemNode _item1;
      protected RelatedItemNode _item2;
      protected RelatedItemLink _itemLink1;
      protected RelatedItemLink _itemLink2;

      protected override void Context()
      {
         _model = new DiagramModel();
         sut = _model.CreateNode<JournalPageNode>("page", new PointF(120, 65), _model);
         _item1 = _model.CreateNode<RelatedItemNode>("item-1", new PointF(80, 127.5F), _model);
         _item2 = _model.CreateNode<RelatedItemNode>("item-2", new PointF(80, 167.5F), _model);
         _itemLink1 = new RelatedItemLink();
         _itemLink1.Initialize(sut, _item1);
         _itemLink2 = new RelatedItemLink();
         _itemLink2.Initialize(sut, _item2);
      }

      protected static JournalPage pageWith(int uniqueIndex, string title)
      {
         return new JournalPage {Id = "page", UniqueIndex = uniqueIndex, Title = title, CreatedAt = new DateTime(2024, 3, 5, 10, 30, 0, DateTimeKind.Utc)};
      }

      protected JournalPageNode createPage(string id, PointF location) => _model.CreateNode<JournalPageNode>(id, location, _model);

      protected static JournalPageLink linkPages(JournalPageNode parent, JournalPageNode child)
      {
         var link = new JournalPageLink();
         link.Initialize(parent, child);
         return link;
      }
   }

   public class When_creating_a_journal_page_node : concern_for_JournalPageNode
   {
      [Observation]
      public void should_have_the_size_of_a_journal_page_and_hide_the_default_label()
      {
         sut.NodeBaseSize.ShouldBeEqualTo(new SizeF(120, 65));
         sut.NodeSize.ShouldBeEqualTo(NodeSize.Middle);
         sut.Size.ShouldBeEqualTo(new SizeF(120, 65));
         sut.Bounds.ShouldBeEqualTo(new RectangleF(60, 32.5F, 120, 65));
         sut.LabelVisible.ShouldBeFalse();
         sut.CanLink.ShouldBeTrue();
      }

      [Observation]
      public void should_be_expanded_with_an_empty_text()
      {
         sut.IsExpanded.ShouldBeTrue();
         sut.UniqueIndex.ShouldBeEqualTo(0);
         sut.Text.ShouldBeEqualTo(string.Empty);
      }

      [Observation]
      public void should_know_its_related_item_nodes_and_have_no_parent()
      {
         sut.HasRelatedItems.ShouldBeTrue();
         sut.RelatedItemNodes.ShouldOnlyContain(_item1, _item2);
         sut.ParentPageNode.ShouldBeNull();
         createPage("empty", new PointF(0, 0)).HasRelatedItems.ShouldBeFalse();
      }
   }

   public class When_collapsing_the_related_items_of_a_page : concern_for_JournalPageNode
   {
      protected override void Because()
      {
         sut.CollapseRelatedItems();
      }

      [Observation]
      public void should_not_be_expanded_anymore()
      {
         sut.IsExpanded.ShouldBeFalse();
         sut.Visible.ShouldBeTrue();
      }

      [Observation]
      public void should_hide_the_related_item_nodes_without_touching_their_own_visibility_flag()
      {
         _item1.Visible.ShouldBeFalse();
         _item2.Visible.ShouldBeFalse();
         _item1.IsVisible.ShouldBeTrue();
      }

      [Observation]
      public void should_hide_the_related_item_links()
      {
         _itemLink1.Visible.ShouldBeFalse();
         _itemLink2.Visible.ShouldBeFalse();
         _itemLink1.IsVisible.ShouldBeTrue();
      }

      [Observation]
      public void should_keep_the_related_items_linked()
      {
         sut.RelatedItemNodes.ShouldOnlyContain(_item1, _item2);
         sut.HasRelatedItems.ShouldBeTrue();
      }
   }

   public class When_expanding_the_related_items_of_a_collapsed_page : concern_for_JournalPageNode
   {
      protected override void Context()
      {
         base.Context();
         sut.CollapseRelatedItems();
      }

      protected override void Because()
      {
         sut.ExpandRelatedItems();
      }

      [Observation]
      public void should_show_the_related_item_nodes_and_their_links_again()
      {
         sut.IsExpanded.ShouldBeTrue();
         _item1.Visible.ShouldBeTrue();
         _item2.Visible.ShouldBeTrue();
         _itemLink1.Visible.ShouldBeTrue();
         _itemLink2.Visible.ShouldBeTrue();
      }

      [Observation]
      public void should_keep_related_items_hidden_that_were_hidden_on_their_own()
      {
         _item1.Visible = false;
         _item1.Visible.ShouldBeFalse();
         _itemLink1.Visible.ShouldBeFalse();
         _item2.Visible.ShouldBeTrue();
      }
   }

   public class When_updating_a_page_node_from_a_journal_page : concern_for_JournalPageNode
   {
      private JournalPage _page;
      private int _changedCount;

      protected override void Context()
      {
         base.Context();
         _page = pageWith(7, "My title");
         _model.Changed += () => _changedCount++;
      }

      protected override void Because()
      {
         sut.UpdateAttributesFrom(_page);
      }

      [Observation]
      public void should_take_over_the_unique_index_and_use_the_title_as_name()
      {
         sut.UniqueIndex.ShouldBeEqualTo(7);
         sut.Name.ShouldBeEqualTo("My title");
      }

      [Observation]
      public void should_format_the_three_line_text_with_title_index_and_creation_date()
      {
         var date = new DateTimeFormatter().Format(_page.CreatedAt);
         sut.Text.ShouldBeEqualTo($"My title{Environment.NewLine}{Environment.NewLine}(7)             {date}{Environment.NewLine}");
      }

      [Observation]
      public void should_notify_the_model()
      {
         (_changedCount > 0).ShouldBeTrue();
      }
   }

   public class When_retrieving_the_parent_page_node : concern_for_JournalPageNode
   {
      private JournalPageNode _parent;
      private JournalPageNode _child;

      protected override void Context()
      {
         base.Context();
         _parent = createPage("parent", new PointF(0, 0));
         _child = createPage("child", new PointF(600, 0));
         linkPages(_parent, sut);
         linkPages(sut, _child);
      }

      [Observation]
      public void should_return_the_from_node_of_the_link_where_the_page_is_the_child()
      {
         sut.ParentPageNode.ShouldBeEqualTo(_parent);
         _child.ParentPageNode.ShouldBeEqualTo(sut);
         _parent.ParentPageNode.ShouldBeNull();
      }
   }

   public class When_clearing_the_parent_links_of_a_page : concern_for_JournalPageNode
   {
      private JournalPageNode _parent;
      private JournalPageNode _child;
      private JournalPageLink _parentLink;
      private JournalPageLink _childLink;

      protected override void Context()
      {
         base.Context();
         _parent = createPage("parent", new PointF(0, 0));
         _child = createPage("child", new PointF(600, 0));
         _parentLink = linkPages(_parent, sut);
         _childLink = linkPages(sut, _child);
      }

      protected override void Because()
      {
         sut.ClearParentLinks();
      }

      [Observation]
      public void should_remove_the_link_to_the_parent_from_both_nodes()
      {
         sut.ParentPageNode.ShouldBeNull();
         sut.Links.ShouldNotContain(_parentLink);
         _parent.Links.ShouldNotContain(_parentLink);
         _parent.Links.OfType<JournalPageLink>().ShouldBeEmpty();
      }

      [Observation]
      public void should_keep_the_links_to_the_children_and_the_related_items()
      {
         sut.Links.OfType<JournalPageLink>().ShouldOnlyContain(_childLink);
         _child.ParentPageNode.ShouldBeEqualTo(sut);
         sut.RelatedItemNodes.ShouldOnlyContain(_item1, _item2);
      }
   }

   public class When_retrieving_the_next_related_item_location : concern_for_JournalPageNode
   {
      [Observation]
      public void should_place_the_first_item_below_the_page_aligned_with_its_related_item_port()
      {
         sut.GetNextRelatedItemLocation(null).ShouldBeEqualTo(new PointF(80, 127.5F));
      }

      [Observation]
      public void should_place_the_next_item_below_the_lowest_related_item()
      {
         sut.GetNextRelatedItemLocation(_item2).ShouldBeEqualTo(new PointF(80, 207.5F));
      }

      [Observation]
      public void should_place_the_next_item_below_the_page_when_the_lowest_item_is_above_the_page_bottom()
      {
         _item1.Location = new PointF(80, 50);
         sut.GetNextRelatedItemLocation(_item1).ShouldBeEqualTo(new PointF(80, 127.5F));
      }

      [Observation]
      public void should_follow_the_page_location()
      {
         sut.Location = new PointF(353, 62);
         sut.GetNextRelatedItemLocation(null).ShouldBeEqualTo(new PointF(313, 124.5F));
      }
   }

   public class When_setting_the_colors_of_a_page_node : concern_for_JournalPageNode
   {
      private DiagramColors _colors;
      private JournalPageLink _parentLink;

      protected override void Context()
      {
         base.Context();
         _colors = new DiagramColors
         {
            JournalPageNode = Color.Red,
            JournalPagePort = Color.Blue,
            JournalPageLink = Color.Green,
            BorderFixed = Color.Black,
            BorderUnfixed = Color.Gray
         };
         _parentLink = linkPages(createPage("parent", new PointF(0, 0)), sut);
      }

      protected override void Because()
      {
         sut.SetColorFrom(_colors);
      }

      [Observation]
      public void should_use_the_journal_page_colors()
      {
         sut.FillColor.ShouldBeEqualTo(Color.Red);
         sut.PortColor.ShouldBeEqualTo(Color.Blue);
         sut.BorderColor.ShouldBeEqualTo(Color.Gray);
         sut.BorderWidth.ShouldBeEqualTo(1F);
      }

      [Observation]
      public void should_color_the_parent_links_but_not_the_related_item_links()
      {
         _parentLink.Color.ShouldBeEqualTo(Color.Green);
         _itemLink1.Color.ShouldBeEqualTo(Color.Empty);
      }

      [Observation]
      public void should_use_the_fixed_border_for_a_fixed_page()
      {
         sut.LocationFixed = true;
         sut.SetColorFrom(_colors);
         sut.BorderColor.ShouldBeEqualTo(Color.Black);
         sut.BorderWidth.ShouldBeEqualTo(2F);
      }
   }

   public class When_copying_a_journal_page_node : concern_for_JournalPageNode
   {
      private JournalPageNode _copy;

      protected override void Context()
      {
         base.Context();
         sut.UpdateAttributesFrom(pageWith(7, "My title"));
         sut.Description = "tooltip";
         sut.IsExpanded = false;
         sut.LocationFixed = true;
         sut.UserFlags = 3;
         sut.SetColorFrom(new DiagramColors());
      }

      protected override void Because()
      {
         _copy = sut.Copy() as JournalPageNode;
      }

      [Observation]
      public void should_create_a_new_journal_page_node()
      {
         _copy.ShouldNotBeNull();
         ReferenceEquals(_copy, sut).ShouldBeFalse();
      }

      [Observation]
      public void should_copy_the_base_properties()
      {
         _copy.Id.ShouldBeEqualTo("page");
         _copy.Name.ShouldBeEqualTo("My title");
         _copy.Description.ShouldBeEqualTo("tooltip");
         _copy.Location.ShouldBeEqualTo(new PointF(120, 65));
         _copy.LocationFixed.ShouldBeTrue();
         _copy.UserFlags.ShouldBeEqualTo(3);
         _copy.NodeBaseSize.ShouldBeEqualTo(new SizeF(120, 65));
         _copy.FillColor.ShouldBeEqualTo(sut.FillColor);
      }

      [Observation]
      public void should_copy_the_journal_properties()
      {
         _copy.UniqueIndex.ShouldBeEqualTo(7);
         _copy.Text.ShouldBeEqualTo(sut.Text);
         _copy.IsExpanded.ShouldBeFalse();
      }

      [Observation]
      public void should_not_copy_the_links()
      {
         _copy.Links.ShouldBeEmpty();
         _copy.RelatedItemNodes.ShouldBeEmpty();
         _copy.HasRelatedItems.ShouldBeFalse();
      }
   }

   public class When_creating_journal_links : concern_for_JournalPageNode
   {
      private JournalPageNode _child;
      private JournalPageLink _pageLink;

      protected override void Context()
      {
         base.Context();
         _child = createPage("child", new PointF(600, 0));
         _pageLink = linkPages(sut, _child);
      }

      [Observation]
      public void should_curve_page_links_and_draw_related_item_links_straight()
      {
         _pageLink.IsCurved.ShouldBeTrue();
         _pageLink.IsDashed.ShouldBeFalse();
         _itemLink1.IsCurved.ShouldBeFalse();
         _itemLink1.IsDashed.ShouldBeFalse();
      }

      [Observation]
      public void should_expose_the_linked_nodes()
      {
         _pageLink.ParentNode.ShouldBeEqualTo(sut);
         _pageLink.ChildNode.ShouldBeEqualTo(_child);
         _pageLink.GetParent().ShouldBeEqualTo(_model);
         _itemLink1.PageNode.ShouldBeEqualTo(sut);
         _itemLink1.ItemNode.ShouldBeEqualTo(_item1);
      }

      [Observation]
      public void should_be_visible_only_while_both_nodes_are_visible()
      {
         _pageLink.Visible.ShouldBeTrue();
         _child.Hidden = true;
         _pageLink.Visible.ShouldBeFalse();
      }
   }

   public abstract class concern_for_RelatedItemNode : ContextSpecification<RelatedItemNode>
   {
      protected override void Context()
      {
         sut = new RelatedItemNode {Id = "item", Location = new PointF(80, 127.5F)};
      }

      protected static RelatedItem itemWith(string name, string itemType) => new RelatedItem {Id = "item", Name = name, ItemType = itemType};
   }

   public class When_creating_a_related_item_node : concern_for_RelatedItemNode
   {
      [Observation]
      public void should_be_a_small_unlinkable_node_with_a_black_label()
      {
         sut.NodeBaseSize.ShouldBeEqualTo(new SizeF(20, 20));
         sut.Size.ShouldBeEqualTo(new SizeF(20, 20));
         sut.NodeSize.ShouldBeEqualTo(NodeSize.Middle);
         sut.CanLink.ShouldBeFalse();
         sut.LabelVisible.ShouldBeTrue();
         sut.LabelColor.ShouldBeEqualTo(Color.Black);
      }

      [Observation]
      public void should_be_visible_without_a_page()
      {
         sut.PageNode.ShouldBeNull();
         sut.Visible.ShouldBeTrue();
      }
   }

   public class When_updating_a_related_item_node_from_an_item_with_a_short_name : concern_for_RelatedItemNode
   {
      protected override void Because()
      {
         sut.UpdateAttributesFromItem(itemWith("Sim 1", "Simulation"));
      }

      [Observation]
      public void should_indent_the_name_and_append_the_item_type()
      {
         sut.Name.ShouldBeEqualTo("  Sim 1 (Simulation)");
      }
   }

   public class When_updating_a_related_item_node_from_an_item_whose_name_fills_the_available_length : concern_for_RelatedItemNode
   {
      protected override void Because()
      {
         sut.UpdateAttributesFromItem(itemWith("ABCDEFGHIJKLMNOPQRSTUV", "Simulation"));
      }

      [Observation]
      public void should_not_shorten_the_name()
      {
         sut.Name.ShouldBeEqualTo("  ABCDEFGHIJKLMNOPQRSTUV (Simulation)");
      }
   }

   public class When_updating_a_related_item_node_from_an_item_with_a_long_name : concern_for_RelatedItemNode
   {
      protected override void Because()
      {
         sut.UpdateAttributesFromItem(itemWith("A very long simulation name indeed", "Simulation"));
      }

      [Observation]
      public void should_truncate_the_name_to_the_maximum_length_minus_the_item_type_with_an_ellipsis()
      {
         sut.Name.ShouldBeEqualTo("  A very long simulat... (Simulation)");
      }
   }

   public class When_updating_a_related_item_node_from_an_item_whose_type_leaves_no_room_for_the_name : concern_for_RelatedItemNode
   {
      protected override void Because()
      {
         sut.UpdateAttributesFromItem(itemWith("Sim 1", "A very long related item type."));
      }

      [Observation]
      public void should_drop_the_name()
      {
         sut.Name.ShouldBeEqualTo("   (A very long related item type.)");
      }
   }

   public class When_setting_the_colors_of_a_related_item_node : concern_for_RelatedItemNode
   {
      private RelatedItemLink _link;

      protected override void Context()
      {
         base.Context();
         _link = new RelatedItemLink();
         _link.Initialize(new JournalPageNode {Id = "page"}, sut);
      }

      protected override void Because()
      {
         sut.SetColorFrom(new DiagramColors {RelatedItemNode = Color.Yellow, RelatedItemLink = Color.Purple});
      }

      [Observation]
      public void should_fill_the_node_and_color_its_link()
      {
         sut.FillColor.ShouldBeEqualTo(Color.Yellow);
         sut.PortColor.ShouldBeEqualTo(Color.Yellow);
         _link.Color.ShouldBeEqualTo(Color.Purple);
      }
   }

   public class When_copying_a_related_item_node : concern_for_RelatedItemNode
   {
      private RelatedItemNode _copy;

      protected override void Context()
      {
         base.Context();
         sut.UpdateAttributesFromItem(itemWith("Sim 1", "Simulation"));
         sut.Description = "tooltip";
      }

      protected override void Because()
      {
         _copy = sut.Copy() as RelatedItemNode;
      }

      [Observation]
      public void should_create_an_unlinkable_related_item_node_with_the_same_properties()
      {
         _copy.ShouldNotBeNull();
         ReferenceEquals(_copy, sut).ShouldBeFalse();
         _copy.Id.ShouldBeEqualTo("item");
         _copy.Name.ShouldBeEqualTo("  Sim 1 (Simulation)");
         _copy.Description.ShouldBeEqualTo("tooltip");
         _copy.Location.ShouldBeEqualTo(new PointF(80, 127.5F));
         _copy.CanLink.ShouldBeFalse();
         _copy.NodeBaseSize.ShouldBeEqualTo(new SizeF(20, 20));
      }
   }

   public class When_retrieving_the_label_color_of_a_journal_page_node : ContextSpecification<JournalPageNode>
   {
      protected override void Context()
      {
         sut = new JournalPageNode();
      }

      [Observation]
      public void should_use_black_like_the_related_item_nodes()
      {
         sut.LabelColor.ShouldBeEqualTo(System.Drawing.Color.Black);
      }
   }
}
