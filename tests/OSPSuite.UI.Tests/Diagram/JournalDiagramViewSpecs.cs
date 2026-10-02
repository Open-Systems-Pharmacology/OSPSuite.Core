using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.XtraDiagram;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views.Diagram;
using OSPSuite.UI.Views.Journal;
using OSPSuite.Utility.Extensions;
using Keys = System.Windows.Forms.Keys;

namespace OSPSuite.UI.Diagram
{
   public class TestJournalDiagramView : JournalDiagramView
   {
      public TestJournalDiagramView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
      }

      public DiagramControl DiagramControl => _diagramControl;

      public void LinkCreated(IBaseNode fromNode, IBaseNode toNode, object fromPort, object toPort) => OnLinkCreated(fromNode, toNode, fromPort, toPort);

      public void SelectionDeleting(IReadOnlyList<IBaseNode> nodes, IReadOnlyList<IBaseLink> links) => OnSelectionDeleting(nodes, links);

      public void NodeDoubleClicked(IBaseNode node) => OnNodeDoubleClicked(node);

      public void NodeClicked(IBaseNode node, PointF documentPoint, Keys modifiers) => OnNodeClicked(node, documentPoint, modifiers);

      public void ContextMenuRequested(IBaseNode node, Point location, PointF documentPoint) => ShowContextMenu(node, location, documentPoint);

      public bool CanConnectNodes(ElementBaseNode node, ElementBaseNode oppositeNode) => CanConnect(node, oppositeNode);

      public bool IsValidConnectionBetween(ElementBaseNode node, int pointIndex, ElementBaseNode oppositeNode, int oppositePointIndex) => IsValidConnection(node, pointIndex, oppositeNode, oppositePointIndex);

      public object PortAt(ElementBaseNode node, int pointIndex, PointFloat point) => PortFor(node, pointIndex, point);

      public bool OverPortOf(ElementBaseNode node, PointF point) => IsOverPortOf(node, point);

      public IEnumerable<IBaseNode> MovingWith(IBaseNode node) => NodesMovingWith(node);
   }

   public abstract class concern_for_JournalDiagramView : ContextSpecification<TestJournalDiagramView>
   {
      protected IJournalDiagramPresenter _presenter;
      protected DiagramModel _model;
      protected JournalPageNode _parentPage;
      protected JournalPageNode _childPage;
      protected JournalPageNode _otherPage;
      protected RelatedItemNode _item1;
      protected RelatedItemNode _item2;
      protected JournalPageLink _pageLink;
      protected RelatedItemLink _itemLink1;
      protected RelatedItemLink _itemLink2;
      protected Point _location = new Point(12, 34);

      protected override void Context()
      {
         var imageListRetriever = A.Fake<IImageListRetriever>();
         A.CallTo(() => imageListRetriever.AllImages16x16).Returns(new SvgImageCollection());
         _presenter = A.Fake<IJournalDiagramPresenter>();
         sut = new TestJournalDiagramView(imageListRetriever);
         sut.AttachPresenter(_presenter);
         sut.InitializeResources();

         _model = new DiagramModel();
         _parentPage = createPage("parent", 1, new PointF(120, 65));
         _childPage = createPage("child", 2, new PointF(353, 62));
         _otherPage = createPage("other", 3, new PointF(586, 59));
         _item1 = createItem("item-1", new PointF(80, 127.5F));
         _item2 = createItem("item-2", new PointF(80, 167.5F));

         _pageLink = new JournalPageLink();
         _pageLink.Initialize(_parentPage, _childPage);
         _itemLink1 = new RelatedItemLink();
         _itemLink1.Initialize(_parentPage, _item1);
         _itemLink2 = new RelatedItemLink();
         _itemLink2.Initialize(_parentPage, _item2);

         var colors = new DiagramColors();
         _model.GetAllChildren<IBaseNode>().Each(node => node.SetColorFrom(colors));
      }

      private JournalPageNode createPage(string id, int uniqueIndex, PointF location)
      {
         var node = _model.CreateNode<JournalPageNode>(id, location, _model);
         node.UpdateAttributesFrom(new JournalPage {Id = id, Title = id, UniqueIndex = uniqueIndex, CreatedAt = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc)});
         node.Description = $"tooltip {id}";
         return node;
      }

      private RelatedItemNode createItem(string id, PointF location)
      {
         var node = _model.CreateNode<RelatedItemNode>(id, location, _model);
         node.UpdateAttributesFromItem(new RelatedItem {Id = id, Name = id, ItemType = "Simulation"});
         return node;
      }

      protected IReadOnlyList<DiagramShape> shapes => sut.DiagramControl.Items.OfType<DiagramShape>().ToList();

      protected IReadOnlyList<DiagramConnector> connectors => sut.DiagramControl.Items.OfType<DiagramConnector>().ToList();

      protected DiagramShape shapeFor(IBaseNode node) => shapes.SingleOrDefault(shape => ReferenceEquals(shape.Tag, node));

      protected DiagramConnector connectorFor(IBaseLink link) => connectors.SingleOrDefault(connector => ReferenceEquals(connector.Tag, link));
   }

   public class When_creating_the_journal_diagram_view : concern_for_JournalDiagramView
   {
      [Observation]
      public void should_expose_the_attached_journal_presenter()
      {
         sut.Presenter.ShouldBeEqualTo(_presenter);
      }

      [Observation]
      public void should_use_the_journal_grid_size()
      {
         sut.DiagramControl.OptionsView.GridSize.ShouldBeEqualTo(new SizeF(10, 10));
      }
   }

   public class When_setting_a_journal_diagram_model : concern_for_JournalDiagramView
   {
      protected override void Because()
      {
         sut.Model = _model;
      }

      [Observation]
      public void should_create_one_shape_per_page_and_related_item()
      {
         shapes.Count.ShouldBeEqualTo(5);
         shapes.Select(shape => shape.Tag).ShouldOnlyContain(_parentPage, _childPage, _otherPage, _item1, _item2);
      }

      [Observation]
      public void should_render_pages_as_rectangles_with_the_journal_connection_points()
      {
         var shape = shapeFor(_parentPage);
         shape.Shape.ShouldBeEqualTo(BasicShapes.Rectangle);
         shape.ConnectionPoints.Count.ShouldBeEqualTo(3);
         shape.ConnectionPoints[JournalConnectionPoints.PARENT_INDEX].ShouldBeEqualTo(new PointFloat(0F, 0.5F));
         shape.ConnectionPoints[JournalConnectionPoints.CHILD_INDEX].ShouldBeEqualTo(new PointFloat(1F, 0.5F));
         shape.ConnectionPoints[JournalConnectionPoints.RELATED_ITEM_INDEX].ShouldBeEqualTo(new PointFloat(JournalConnectionPoints.RELATED_ITEM_RELATIVE_X, 1F));
      }

      [Observation]
      public void should_place_the_page_shapes_around_the_node_location()
      {
         var shape = shapeFor(_parentPage);
         shape.Position.ShouldBeEqualTo(new PointFloat(60, 32.5F));
         shape.Width.ShouldBeEqualTo(120F);
         shape.Height.ShouldBeEqualTo(65F);
         shape.Appearance.BackColor.ShouldBeEqualTo(_parentPage.FillColor);
         shape.CanAttachConnectorBeginPoint.ShouldBeEqualTo(true);
      }

      [Observation]
      public void should_render_related_items_as_unlinkable_ellipses_with_a_single_connection_point()
      {
         var shape = shapeFor(_item1);
         shape.Shape.ShouldBeEqualTo(BasicShapes.Ellipse);
         shape.ConnectionPoints.Count.ShouldBeEqualTo(1);
         shape.CanAttachConnectorBeginPoint.ShouldBeEqualTo(false);
         shape.CanAttachConnectorEndPoint.ShouldBeEqualTo(false);
         shape.Width.ShouldBeEqualTo(20F);
      }

      [Observation]
      public void should_glue_the_curved_page_link_from_the_child_port_of_the_parent_to_the_parent_port_of_the_child()
      {
         var connector = connectorFor(_pageLink);
         connector.Type.ShouldBeEqualTo(ConnectorType.Curved);
         connector.BeginItem.ShouldBeEqualTo(shapeFor(_parentPage));
         connector.BeginItemPointIndex.ShouldBeEqualTo(JournalConnectionPoints.CHILD_INDEX);
         connector.EndItem.ShouldBeEqualTo(shapeFor(_childPage));
         connector.EndItemPointIndex.ShouldBeEqualTo(JournalConnectionPoints.PARENT_INDEX);
         connector.BeginArrow.ShouldBeNull();
         connector.EndArrow.ShouldBeNull();
         connector.Appearance.BorderColor.ShouldBeEqualTo(_pageLink.Color);
      }

      [Observation]
      public void should_glue_the_straight_related_item_links_to_the_related_item_port_of_the_page()
      {
         connectors.Count.ShouldBeEqualTo(3);
         var connector = connectorFor(_itemLink1);
         connector.Type.ShouldBeEqualTo(ConnectorType.Straight);
         connector.BeginItem.ShouldBeEqualTo(shapeFor(_parentPage));
         connector.BeginItemPointIndex.ShouldBeEqualTo(JournalConnectionPoints.RELATED_ITEM_INDEX);
         connector.EndItem.ShouldBeEqualTo(shapeFor(_item1));
         connector.EndItemPointIndex.ShouldBeEqualTo(0);
         connector.Appearance.BorderColor.ShouldBeEqualTo(_itemLink1.Color);
      }

      [Observation]
      public void should_put_the_connectors_behind_the_shapes()
      {
         var items = sut.DiagramControl.Items.ToList();
         items.Take(3).Each(item => item.ShouldBeAnInstanceOf<DiagramConnector>());
         items.Skip(3).Each(item => item.ShouldBeAnInstanceOf<DiagramShape>());
      }
   }

   public class When_a_page_is_collapsed_in_the_model : concern_for_JournalDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         _parentPage.IsExpanded = false;
         sut.Refresh();
      }

      [Observation]
      public void should_remove_the_related_item_shapes_and_connectors_of_the_page()
      {
         shapeFor(_item1).ShouldBeNull();
         shapeFor(_item2).ShouldBeNull();
         connectorFor(_itemLink1).ShouldBeNull();
         connectorFor(_itemLink2).ShouldBeNull();
         shapes.Count.ShouldBeEqualTo(3);
         connectors.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_keep_the_page_shapes_and_the_page_link()
      {
         shapeFor(_parentPage).ShouldNotBeNull();
         connectorFor(_pageLink).BeginItem.ShouldBeEqualTo(shapeFor(_parentPage));
      }

      [Observation]
      public void should_show_the_related_items_again_when_the_page_is_expanded()
      {
         _parentPage.IsExpanded = true;
         sut.Refresh();
         shapes.Count.ShouldBeEqualTo(5);
         connectors.Count.ShouldBeEqualTo(3);
         connectorFor(_itemLink1).BeginItem.ShouldBeEqualTo(shapeFor(_parentPage));
         connectorFor(_itemLink1).EndItem.ShouldBeEqualTo(shapeFor(_item1));
      }
   }

   public class When_retrieving_the_selection_of_the_journal_diagram_view : concern_for_JournalDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         sut.Select(_parentPage);
         sut.Select(_pageLink);
         sut.Select(_item1);
      }

      [Observation]
      public void should_return_the_selected_nodes_and_links_of_the_control()
      {
         sut.GetSelection().ShouldOnlyContain(_parentPage, _pageLink, _item1);
      }

      [Observation]
      public void should_clear_the_selection_of_the_control_when_removing_the_selection_handles()
      {
         sut.RemoveSelectionHandles();
         sut.GetSelection().ShouldBeEmpty();
         sut.DiagramControl.SelectedItems.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_the_user_draws_a_link_from_the_child_port_of_a_page_to_the_parent_port_of_another_page : concern_for_JournalDiagramView
   {
      protected override void Because()
      {
         sut.LinkCreated(_childPage, _otherPage, JournalPort.Child, JournalPort.Parent);
      }

      [Observation]
      public void should_add_the_from_page_as_parent_of_the_to_page()
      {
         A.CallTo(() => _presenter.AddParentLink(_otherPage, _childPage)).MustHaveHappened();
      }
   }

   public class When_the_user_draws_a_link_from_the_parent_port_of_a_page_to_the_child_port_of_another_page : concern_for_JournalDiagramView
   {
      protected override void Because()
      {
         sut.LinkCreated(_otherPage, _childPage, JournalPort.Parent, JournalPort.Child);
      }

      [Observation]
      public void should_add_the_to_page_as_parent_of_the_from_page()
      {
         A.CallTo(() => _presenter.AddParentLink(_otherPage, _childPage)).MustHaveHappened();
      }
   }

   public class When_the_user_connects_two_pages_in_the_diagram_control : concern_for_JournalDiagramView
   {
      private DiagramConnector _userConnector;

      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         _userConnector = new DiagramConnector(shapeFor(_childPage), shapeFor(_otherPage))
         {
            BeginItemPointIndex = JournalConnectionPoints.CHILD_INDEX,
            EndItemPointIndex = JournalConnectionPoints.PARENT_INDEX
         };
      }

      protected override void Because()
      {
         sut.DiagramControl.Items.Add(_userConnector);
      }

      [Observation]
      public void should_remove_the_connector_drawn_by_the_user()
      {
         sut.DiagramControl.Items.Contains(_userConnector).ShouldBeFalse();
         connectors.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_add_the_parent_link_through_the_presenter()
      {
         A.CallTo(() => _presenter.AddParentLink(_otherPage, _childPage)).MustHaveHappened();
      }
   }

   public class When_the_user_deletes_the_selection_in_the_journal_diagram_view : concern_for_JournalDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         sut.Select(_item1);
         sut.Select(_pageLink);
      }

      protected override void Because()
      {
         sut.DiagramControl.DeleteSelectedItems();
      }

      [Observation]
      public void should_delegate_the_deletion_to_the_presenter()
      {
         A.CallTo(() => _presenter.DeleteSelection()).MustHaveHappened();
      }

      [Observation]
      public void should_not_delete_anything_from_the_diagram_control()
      {
         shapes.Count.ShouldBeEqualTo(5);
         connectors.Count.ShouldBeEqualTo(3);
      }
   }

   public class When_the_selection_deleting_hook_is_invoked : concern_for_JournalDiagramView
   {
      protected override void Because()
      {
         sut.SelectionDeleting(new[] {_parentPage}, new IBaseLink[0]);
      }

      [Observation]
      public void should_delete_the_selection_through_the_presenter()
      {
         A.CallTo(() => _presenter.DeleteSelection()).MustHaveHappened();
      }
   }

   public class When_double_clicking_a_page_node : concern_for_JournalDiagramView
   {
      protected override void Because()
      {
         sut.NodeDoubleClicked(_parentPage);
      }

      [Observation]
      public void should_edit_the_journal_page()
      {
         A.CallTo(() => _presenter.EditJournalPage(_parentPage)).MustHaveHappened();
      }
   }

   public class When_double_clicking_a_related_item_node : concern_for_JournalDiagramView
   {
      protected override void Because()
      {
         sut.NodeDoubleClicked(_item1);
      }

      [Observation]
      public void should_not_edit_any_journal_page()
      {
         A.CallTo(() => _presenter.EditJournalPage(A<IJournalPageNode>._)).MustNotHaveHappened();
      }
   }

   public class When_requesting_the_context_menu_on_the_background : concern_for_JournalDiagramView
   {
      protected override void Because()
      {
         sut.ContextMenuRequested(null, _location, new PointF(5, 5));
      }

      [Observation]
      public void should_show_the_background_context_menu()
      {
         A.CallTo(() => _presenter.ShowContextMenu(A<IViewItem>.That.Matches(x => x is JournalDiagramBackground), _location)).MustHaveHappened();
         A.CallTo(() => _presenter.ShowContextMenu(A<IReadOnlyList<IBaseNode>>._, A<Point>._)).MustNotHaveHappened();
      }
   }

   public class When_requesting_the_context_menu_on_a_selected_node : concern_for_JournalDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         sut.Select(_parentPage);
         sut.Select(_childPage);
      }

      protected override void Because()
      {
         sut.ContextMenuRequested(_parentPage, _location, new PointF(120, 65));
      }

      [Observation]
      public void should_show_the_context_menu_for_the_whole_selection()
      {
         A.CallTo(() => _presenter.ShowContextMenu(A<IReadOnlyList<IBaseNode>>.That.Matches(x => x.Count == 2 && x.Contains(_parentPage) && x.Contains(_childPage)), _location)).MustHaveHappened();
         A.CallTo(() => _presenter.ShowContextMenu(A<IViewItem>._, A<Point>._)).MustNotHaveHappened();
      }
   }

   public class When_requesting_the_context_menu_on_a_node_that_is_not_selected : concern_for_JournalDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         sut.Select(_childPage);
      }

      protected override void Because()
      {
         sut.ContextMenuRequested(_otherPage, _location, new PointF(586, 59));
      }

      [Observation]
      public void should_show_the_context_menu_for_that_node_only()
      {
         A.CallTo(() => _presenter.ShowContextMenu(A<IReadOnlyList<IBaseNode>>.That.Matches(x => x.Count == 1 && x.Contains(_otherPage)), _location)).MustHaveHappened();
      }
   }

   public class When_validating_connections_between_journal_nodes : concern_for_JournalDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      [Observation]
      public void should_allow_connecting_the_child_port_of_a_page_to_the_parent_port_of_a_page_without_parent()
      {
         sut.IsValidConnectionBetween(_otherPage, JournalConnectionPoints.PARENT_INDEX, _parentPage, JournalConnectionPoints.CHILD_INDEX).ShouldBeTrue();
         sut.IsValidConnectionBetween(_parentPage, JournalConnectionPoints.CHILD_INDEX, _otherPage, JournalConnectionPoints.PARENT_INDEX).ShouldBeTrue();
      }

      [Observation]
      public void should_allow_starting_a_connection_at_a_page()
      {
         sut.IsValidConnectionBetween(_otherPage, JournalConnectionPoints.PARENT_INDEX, null, -1).ShouldBeTrue();
         sut.IsValidConnectionBetween(_item1, 0, null, -1).ShouldBeFalse();
      }

      [Observation]
      public void should_not_allow_a_second_parent_for_a_page()
      {
         sut.IsValidConnectionBetween(_childPage, JournalConnectionPoints.PARENT_INDEX, _otherPage, JournalConnectionPoints.CHILD_INDEX).ShouldBeFalse();
         sut.IsValidConnectionBetween(_otherPage, JournalConnectionPoints.CHILD_INDEX, _childPage, JournalConnectionPoints.PARENT_INDEX).ShouldBeFalse();
      }

      [Observation]
      public void should_not_allow_connecting_equal_ports_related_item_ports_or_a_page_with_itself()
      {
         sut.IsValidConnectionBetween(_otherPage, JournalConnectionPoints.CHILD_INDEX, _parentPage, JournalConnectionPoints.CHILD_INDEX).ShouldBeFalse();
         sut.IsValidConnectionBetween(_otherPage, JournalConnectionPoints.PARENT_INDEX, _parentPage, JournalConnectionPoints.PARENT_INDEX).ShouldBeFalse();
         sut.IsValidConnectionBetween(_otherPage, JournalConnectionPoints.RELATED_ITEM_INDEX, _parentPage, JournalConnectionPoints.CHILD_INDEX).ShouldBeFalse();
         sut.IsValidConnectionBetween(_otherPage, JournalConnectionPoints.PARENT_INDEX, _parentPage, -1).ShouldBeFalse();
         sut.IsValidConnectionBetween(_otherPage, JournalConnectionPoints.PARENT_INDEX, _otherPage, JournalConnectionPoints.CHILD_INDEX).ShouldBeFalse();
      }

      [Observation]
      public void should_not_allow_connecting_related_items()
      {
         sut.IsValidConnectionBetween(_item1, 0, _parentPage, JournalConnectionPoints.CHILD_INDEX).ShouldBeFalse();
         sut.IsValidConnectionBetween(_otherPage, JournalConnectionPoints.PARENT_INDEX, _item1, 0).ShouldBeFalse();
      }

      [Observation]
      public void should_only_offer_connection_points_between_two_different_pages()
      {
         sut.CanConnectNodes(_otherPage, null).ShouldBeTrue();
         sut.CanConnectNodes(_otherPage, _parentPage).ShouldBeTrue();
         sut.CanConnectNodes(_otherPage, _otherPage).ShouldBeFalse();
         sut.CanConnectNodes(_otherPage, _item1).ShouldBeFalse();
         sut.CanConnectNodes(_item1, null).ShouldBeFalse();
      }
   }

   public class When_resolving_the_port_of_a_journal_node : concern_for_JournalDiagramView
   {
      [Observation]
      public void should_map_the_connection_point_index_to_the_journal_port()
      {
         sut.PortAt(_parentPage, JournalConnectionPoints.PARENT_INDEX, PointFloat.Empty).ShouldBeEqualTo(JournalPort.Parent);
         sut.PortAt(_parentPage, JournalConnectionPoints.CHILD_INDEX, PointFloat.Empty).ShouldBeEqualTo(JournalPort.Child);
         sut.PortAt(_parentPage, JournalConnectionPoints.RELATED_ITEM_INDEX, PointFloat.Empty).ShouldBeEqualTo(JournalPort.RelatedItem);
      }

      [Observation]
      public void should_use_the_side_of_the_page_when_the_connector_is_not_glued_to_a_connection_point()
      {
         sut.PortAt(_parentPage, -1, new PointFloat(50, 65)).ShouldBeEqualTo(JournalPort.Parent);
         sut.PortAt(_parentPage, -1, new PointFloat(170, 65)).ShouldBeEqualTo(JournalPort.Child);
      }

      [Observation]
      public void should_not_return_a_port_for_related_items()
      {
         sut.PortAt(_item1, 0, PointFloat.Empty).ShouldBeNull();
      }

      [Observation]
      public void should_detect_the_parent_and_child_ports_of_a_page_only()
      {
         sut.OverPortOf(_parentPage, new PointF(60, 65)).ShouldBeTrue();
         sut.OverPortOf(_parentPage, new PointF(180, 65)).ShouldBeTrue();
         sut.OverPortOf(_parentPage, new PointF(120, 65)).ShouldBeFalse();
         sut.OverPortOf(_parentPage, new PointF(72, 97.5F)).ShouldBeFalse();
      }
   }

   public class When_moving_a_page_in_the_journal_diagram_view : concern_for_JournalDiagramView
   {
      [Observation]
      public void should_move_the_related_items_of_the_page_along()
      {
         sut.MovingWith(_parentPage).ShouldOnlyContain(_item1, _item2);
      }

      [Observation]
      public void should_not_move_other_nodes_along_with_pages_without_related_items_or_with_related_items()
      {
         sut.MovingWith(_childPage).ShouldBeEmpty();
         sut.MovingWith(_item1).ShouldBeEmpty();
      }
   }

   public class When_clicking_the_collapse_handle_of_a_page : concern_for_JournalDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         sut.NodeClicked(_parentPage, new PointF(72, 97.5F), Keys.None);
      }

      [Observation]
      public void should_collapse_the_related_items_of_the_page()
      {
         _parentPage.IsExpanded.ShouldBeFalse();
         shapeFor(_item1).ShouldBeNull();
         connectorFor(_itemLink1).ShouldBeNull();
      }

      [Observation]
      public void should_expand_the_page_again_on_the_next_click()
      {
         sut.NodeClicked(_parentPage, new PointF(72, 97.5F), Keys.None);
         _parentPage.IsExpanded.ShouldBeTrue();
         shapeFor(_item1).ShouldNotBeNull();
      }
   }

   public class When_clicking_a_page_outside_of_the_collapse_handle : concern_for_JournalDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         sut.NodeClicked(_parentPage, new PointF(120, 65), Keys.None);
         sut.NodeClicked(_childPage, new PointF(305, 94.5F), Keys.None);
      }

      [Observation]
      public void should_not_change_the_expansion_state()
      {
         _parentPage.IsExpanded.ShouldBeTrue();
         _childPage.IsExpanded.ShouldBeTrue();
         shapes.Count.ShouldBeEqualTo(5);
      }
   }
}
