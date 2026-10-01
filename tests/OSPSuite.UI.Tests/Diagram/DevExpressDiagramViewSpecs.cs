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
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.Diagram;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views.Diagram;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Diagram
{
   public class TestDevExpressDiagramView : DevExpressDiagramView
   {
      public TestDevExpressDiagramView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
      }

      public DiagramControl DiagramControl => _diagramControl;

      public IReadOnlyList<IBaseNode> DeletingNodes { get; private set; }
      public IReadOnlyList<IBaseLink> DeletingLinks { get; private set; }
      public (IBaseNode fromNode, IBaseNode toNode, object fromPort, object toPort)? CreatedLink { get; private set; }

      public void MakeReadOnly() => SetReadOnly();

      protected override void OnSelectionDeleting(IReadOnlyList<IBaseNode> nodes, IReadOnlyList<IBaseLink> links)
      {
         DeletingNodes = nodes;
         DeletingLinks = links;
      }

      protected override void OnLinkCreated(IBaseNode fromNode, IBaseNode toNode, object fromPort, object toPort)
      {
         CreatedLink = (fromNode, toNode, fromPort, toPort);
      }
   }

   public abstract class concern_for_DevExpressDiagramView : ContextSpecification<TestDevExpressDiagramView>
   {
      protected IBaseDiagramPresenter _presenter;
      protected DiagramModel _model;
      protected MoleculeNode _educt;
      protected MoleculeNode _product;
      protected MoleculeNode _modifier;
      protected ReactionNode _reaction;
      protected ReactionLink _eductLink;
      protected ReactionLink _productLink;
      protected ReactionLink _modifierLink;

      protected override void Context()
      {
         var imageListRetriever = A.Fake<IImageListRetriever>();
         A.CallTo(() => imageListRetriever.AllImages16x16).Returns(new SvgImageCollection());
         _presenter = A.Fake<IBaseDiagramPresenter>();
         sut = new TestDevExpressDiagramView(imageListRetriever);
         sut.AttachPresenter(_presenter);
         sut.InitializeResources();

         _model = new DiagramModel();
         _educt = createMolecule("A", new PointF(20, 40));
         _product = createMolecule("B", new PointF(200, 40));
         _modifier = createMolecule("M", new PointF(110, -40));
         _reaction = _model.CreateNode<ReactionNode>("R", new PointF(110, 60), _model);
         _reaction.Name = "R";
         _reaction.Description = "A + B -> C";
         _eductLink = link(ReactionLinkType.Educt, _educt);
         _productLink = link(ReactionLinkType.Product, _product);
         _modifierLink = link(ReactionLinkType.Modifier, _modifier);

         var colors = new DiagramColors();
         _model.GetAllChildren<IBaseNode>().Each(node => node.SetColorFrom(colors));
      }

      private MoleculeNode createMolecule(string name, PointF location)
      {
         var node = _model.CreateNode<MoleculeNode>(name, location, _model);
         node.Name = name;
         node.Description = $"molecule {name}";
         return node;
      }

      private ReactionLink link(ReactionLinkType type, MoleculeNode moleculeNode)
      {
         var reactionLink = new ReactionLink();
         reactionLink.Initialize(type, _reaction, moleculeNode);
         return reactionLink;
      }

      protected IReadOnlyList<DiagramShape> shapes => sut.DiagramControl.Items.OfType<DiagramShape>().ToList();

      protected IReadOnlyList<DiagramConnector> connectors => sut.DiagramControl.Items.OfType<DiagramConnector>().ToList();

      protected DiagramShape shapeFor(IBaseNode node) => shapes.SingleOrDefault(shape => ReferenceEquals(shape.Tag, node));

      protected DiagramConnector connectorFor(IBaseLink link) => connectors.SingleOrDefault(connector => ReferenceEquals(connector.Tag, link));

      protected static PointFloat expectedPosition(IBaseNode node) => new PointFloat(node.Bounds.X, node.Bounds.Y);
   }

   public class When_creating_the_dev_express_diagram_view : concern_for_DevExpressDiagramView
   {
      [Observation]
      public void should_expose_the_attached_presenter()
      {
         sut.Presenter.ShouldBeEqualTo(_presenter);
      }

      [Observation]
      public void should_create_a_popup_bar_manager_for_context_menus()
      {
         sut.PopupBarManager.ShouldNotBeNull();
      }

      [Observation]
      public void should_hide_grid_page_breaks_rulers_and_panels()
      {
         var optionsView = sut.DiagramControl.OptionsView;
         optionsView.ShowGrid.ShouldBeFalse();
         optionsView.ShowPageBreaks.ShouldBeFalse();
         optionsView.ShowRulers.ShouldBeFalse();
         optionsView.ShowPanAndZoomPanel.ShouldBeFalse();
         optionsView.CanvasSizeMode.ShouldBeEqualTo(CanvasSizeMode.Fill);
         optionsView.PropertiesPanelVisibility.ShouldBeEqualTo(PropertiesPanelVisibility.Closed);
         optionsView.ToolboxVisibility.ShouldBeEqualTo(ToolboxVisibility.Closed);
         sut.DiagramControl.OptionsBehavior.ShowQuickShapes.ShouldBeFalse();
      }

      [Observation]
      public void should_limit_the_zoom_like_the_go_diagram_view()
      {
         sut.DiagramControl.OptionsView.MinZoomFactor.ShouldBeEqualTo(Assets.Diagram.Base.MinLimitDocScale);
         sut.DiagramControl.OptionsView.MaxZoomFactor.ShouldBeEqualTo(Assets.Diagram.Base.MaxLimitDocScale);
      }

      [Observation]
      public void should_protect_the_items_from_being_edited()
      {
         var optionsProtection = sut.DiagramControl.OptionsProtection;
         optionsProtection.AllowUndoRedo.ShouldBeEqualTo(false);
         optionsProtection.AllowCopyItems.ShouldBeEqualTo(false);
         optionsProtection.AllowEditItems.ShouldBeEqualTo(false);
         optionsProtection.AllowResizeItems.ShouldBeEqualTo(false);
         optionsProtection.AllowRotateItems.ShouldBeEqualTo(false);
         optionsProtection.AllowChangeConnectorsRoute.ShouldBeEqualTo(false);
         optionsProtection.IsReadOnly.ShouldBeFalse();
      }
   }

   public class When_setting_a_model_that_is_not_a_ui_free_diagram_model : concern_for_DevExpressDiagramView
   {
      [Observation]
      public void should_throw_an_invalid_type_exception()
      {
         The.Action(() => sut.Model = A.Fake<IDiagramModel>()).ShouldThrowAn<InvalidTypeException>();
      }
   }

   public class When_setting_a_reaction_diagram_model : concern_for_DevExpressDiagramView
   {
      protected override void Because()
      {
         sut.Model = _model;
      }

      [Observation]
      public void should_create_one_shape_per_node_tagged_with_the_node()
      {
         shapes.Count.ShouldBeEqualTo(4);
         shapes.Select(shape => shape.Tag).ShouldOnlyContain(_educt, _product, _modifier, _reaction);
      }

      [Observation]
      public void should_render_molecules_as_ellipses_with_eight_connection_points()
      {
         var shape = shapeFor(_educt);
         shape.Shape.ShouldBeEqualTo(BasicShapes.Ellipse);
         shape.ConnectionPoints.Count.ShouldBeEqualTo(8);
      }

      [Observation]
      public void should_render_reactions_as_triangles_with_three_semantic_connection_points()
      {
         var shape = shapeFor(_reaction);
         shape.Shape.ShouldBeEqualTo(BasicShapes.Triangle);
         shape.ConnectionPoints.Count.ShouldBeEqualTo(3);
         shape.ConnectionPoints[ReactionConnectionPoints.EDUCT_INDEX].ShouldBeEqualTo(new PointFloat(0F, 1F));
         shape.ConnectionPoints[ReactionConnectionPoints.MODIFIER_INDEX].ShouldBeEqualTo(new PointFloat(0.5F, 0F));
      }

      [Observation]
      public void should_place_the_shapes_with_the_node_location_as_center()
      {
         var shape = shapeFor(_reaction);
         shape.Position.ShouldBeEqualTo(expectedPosition(_reaction));
         shape.Width.ShouldBeEqualTo(_reaction.Size.Width);
         shape.Height.ShouldBeEqualTo(_reaction.Size.Height);
      }

      [Observation]
      public void should_apply_the_node_colors()
      {
         var shape = shapeFor(_educt);
         shape.Appearance.BackColor.ShouldBeEqualTo(_educt.FillColor);
         shape.Appearance.BorderColor.ShouldBeEqualTo(_educt.BorderColor);
      }

      [Observation]
      public void should_not_allow_resizing_rotating_or_editing_the_shapes()
      {
         shapes.Each(shape =>
         {
            shape.CanResize.ShouldBeEqualTo(false);
            shape.CanRotate.ShouldBeEqualTo(false);
            shape.CanEdit.ShouldBeEqualTo(false);
            shape.CanSelect.ShouldBeEqualTo(true);
         });
      }

      [Observation]
      public void should_create_one_connector_per_link_tagged_with_the_link()
      {
         connectors.Count.ShouldBeEqualTo(3);
         connectors.Select(connector => connector.Tag).ShouldOnlyContain(_eductLink, _productLink, _modifierLink);
      }

      [Observation]
      public void should_glue_the_educt_connector_from_the_molecule_border_to_the_educt_point_of_the_reaction()
      {
         var connector = connectorFor(_eductLink);
         connector.BeginItem.ShouldBeEqualTo(shapeFor(_educt));
         connector.EndItem.ShouldBeEqualTo(shapeFor(_reaction));
         connector.BeginItemPointIndex.ShouldBeEqualTo(-1);
         connector.EndItemPointIndex.ShouldBeEqualTo(ReactionConnectionPoints.EDUCT_INDEX);
      }

      [Observation]
      public void should_glue_the_product_connector_from_the_product_point_of_the_reaction_to_the_molecule_border()
      {
         var connector = connectorFor(_productLink);
         connector.BeginItem.ShouldBeEqualTo(shapeFor(_reaction));
         connector.EndItem.ShouldBeEqualTo(shapeFor(_product));
         connector.BeginItemPointIndex.ShouldBeEqualTo(ReactionConnectionPoints.PRODUCT_INDEX);
         connector.EndItemPointIndex.ShouldBeEqualTo(-1);
      }

      [Observation]
      public void should_glue_the_modifier_connector_to_the_modifier_point_of_the_reaction_and_dash_it()
      {
         var connector = connectorFor(_modifierLink);
         connector.EndItemPointIndex.ShouldBeEqualTo(ReactionConnectionPoints.MODIFIER_INDEX);
         connector.Appearance.BorderDashPattern.Count.ShouldBeEqualTo(2);
         connectorFor(_eductLink).Appearance.BorderDashPattern.Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_draw_curved_connectors_without_arrows_in_the_link_color()
      {
         var connector = connectorFor(_eductLink);
         connector.Type.ShouldBeEqualTo(ConnectorType.Straight);
         connector.BeginArrow.ShouldBeNull();
         connector.EndArrow.ShouldBeNull();
         connector.Appearance.BorderColor.ShouldBeEqualTo(_eductLink.Color);
         connector.CanChangeRoute.ShouldBeFalse();
         connector.CanEdit.ShouldBeEqualTo(false);
         connector.CanMove.ShouldBeEqualTo(false);
         connector.CanSelect.ShouldBeEqualTo(true);
      }

      [Observation]
      public void should_put_the_connectors_behind_the_shapes()
      {
         var items = sut.DiagramControl.Items.ToList();
         items.Take(3).Each(item => item.ShouldBeAnInstanceOf<DiagramConnector>());
         items.Skip(3).Each(item => item.ShouldBeAnInstanceOf<DiagramShape>());
      }
   }

   public class When_rendering_the_diagram_model_to_a_bitmap : concern_for_DevExpressDiagramView
   {
      private Bitmap _bitmap;

      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         sut.Refresh();
      }

      protected override void Because()
      {
         _bitmap = sut.GetBitmap(_model);
      }

      [Observation]
      public void should_return_a_bitmap_covering_the_diagram()
      {
         _bitmap.ShouldNotBeNull();
         (_bitmap.Width > 1).ShouldBeTrue();
         (_bitmap.Height > 1).ShouldBeTrue();
      }
   }

   public class When_rendering_an_empty_diagram_model_to_a_bitmap : concern_for_DevExpressDiagramView
   {
      private Bitmap _bitmap;

      protected override void Context()
      {
         base.Context();
         sut.Model = new DiagramModel();
      }

      protected override void Because()
      {
         _bitmap = sut.GetBitmap(_model);
      }

      [Observation]
      public void should_return_a_placeholder_bitmap()
      {
         _bitmap.ShouldNotBeNull();
      }
   }

   public class When_selecting_nodes_and_links_in_the_view : concern_for_DevExpressDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         sut.Select(_educt);
         sut.Select(_reaction);
         sut.Select(_productLink);
      }

      [Observation]
      public void should_return_the_selected_nodes_of_the_requested_type()
      {
         sut.GetSelectedNodes<MoleculeNode>().ShouldOnlyContain(_educt);
         sut.GetSelectedNodes<ReactionNode>().ShouldOnlyContain(_reaction);
         sut.GetSelectedNodes<IBaseNode>().ShouldOnlyContain(_educt, _reaction);
         sut.GetSelectedNodes<IBaseLink>().ShouldOnlyContain(_productLink);
      }

      [Observation]
      public void should_know_whether_the_selection_contains_an_object()
      {
         sut.SelectionContains(_educt).ShouldBeTrue();
         sut.SelectionContains(_productLink).ShouldBeTrue();
         sut.SelectionContains(_product).ShouldBeFalse();
         sut.SelectionContains(_eductLink).ShouldBeFalse();
      }

      [Observation]
      public void should_clear_the_selection()
      {
         sut.ClearSelection();
         sut.GetSelectedNodes<IBaseNode>().ShouldBeEmpty();
         sut.GetSelectedNodes<IBaseLink>().ShouldBeEmpty();
         sut.SelectionContains(_educt).ShouldBeFalse();
      }

      [Observation]
      public void should_ignore_objects_that_are_not_part_of_the_view()
      {
         sut.Select(new MoleculeNode {Id = "unknown"});
         sut.GetSelectedNodes<IBaseNode>().ShouldOnlyContain(_educt, _reaction);
      }
   }

   public class When_the_location_of_a_node_changes_in_the_model : concern_for_DevExpressDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         _educt.Location = new PointF(300, 400);
      }

      [Observation]
      public void should_move_the_shape()
      {
         shapeFor(_educt).Position.ShouldBeEqualTo(new PointFloat(300 - _educt.Size.Width / 2, 400 - _educt.Size.Height / 2));
      }

      [Observation]
      public void should_keep_the_shape_instance_and_its_connectors()
      {
         shapes.Count.ShouldBeEqualTo(4);
         connectorFor(_eductLink).BeginItem.ShouldBeEqualTo(shapeFor(_educt));
      }
   }

   public class When_the_size_of_a_node_changes_in_the_model : concern_for_DevExpressDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         _educt.NodeSize = NodeSize.Small;
      }

      [Observation]
      public void should_resize_the_shape_around_the_node_center()
      {
         var shape = shapeFor(_educt);
         shape.Width.ShouldBeEqualTo(_educt.Size.Width);
         shape.Height.ShouldBeEqualTo(_educt.Size.Height);
         shape.Position.ShouldBeEqualTo(expectedPosition(_educt));
      }
   }

   public class When_the_educt_display_side_of_a_reaction_changes : concern_for_DevExpressDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         _reaction.DisplayEductsRight = true;
      }

      [Observation]
      public void should_swap_the_educt_and_product_connection_points()
      {
         var shape = shapeFor(_reaction);
         shape.ConnectionPoints[ReactionConnectionPoints.EDUCT_INDEX].ShouldBeEqualTo(new PointFloat(1F, 1F));
         shape.ConnectionPoints[ReactionConnectionPoints.PRODUCT_INDEX].ShouldBeEqualTo(new PointFloat(0F, 1F));
      }

      [Observation]
      public void should_keep_the_connectors_glued_to_the_semantic_indices()
      {
         connectorFor(_eductLink).EndItemPointIndex.ShouldBeEqualTo(ReactionConnectionPoints.EDUCT_INDEX);
         connectorFor(_productLink).BeginItemPointIndex.ShouldBeEqualTo(ReactionConnectionPoints.PRODUCT_INDEX);
      }
   }

   public class When_a_node_is_hidden_in_the_model : concern_for_DevExpressDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         _educt.Hidden = true;
      }

      [Observation]
      public void should_remove_the_shape_and_its_connectors()
      {
         shapeFor(_educt).ShouldBeNull();
         connectorFor(_eductLink).ShouldBeNull();
         shapes.Count.ShouldBeEqualTo(3);
         connectors.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_restore_the_shape_and_its_connectors_when_the_node_is_shown_again()
      {
         _educt.Hidden = false;
         shapeFor(_educt).ShouldNotBeNull();
         connectorFor(_eductLink).BeginItem.ShouldBeEqualTo(shapeFor(_educt));
         sut.DiagramControl.Items.Take(3).Each(item => item.ShouldBeAnInstanceOf<DiagramConnector>());
      }
   }

   public class When_a_node_is_removed_from_the_model : concern_for_DevExpressDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         _model.RemoveNode(_reaction.Id);
      }

      [Observation]
      public void should_remove_the_shape_and_all_its_connectors()
      {
         shapeFor(_reaction).ShouldBeNull();
         shapes.Count.ShouldBeEqualTo(3);
         connectors.ShouldBeEmpty();
      }
   }

   public class When_a_node_is_added_to_the_model : concern_for_DevExpressDiagramView
   {
      private MoleculeNode _newMolecule;

      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
      }

      protected override void Because()
      {
         _newMolecule = _model.CreateNode<MoleculeNode>("N", new PointF(0, 0), _model);
         _newMolecule.Name = "N";
      }

      [Observation]
      public void should_add_a_shape_for_the_new_node()
      {
         shapeFor(_newMolecule).ShouldNotBeNull();
         shapes.Count.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_add_a_connector_when_the_node_is_linked()
      {
         var newLink = new ReactionLink();
         newLink.Initialize(ReactionLinkType.Educt, _reaction, _newMolecule);
         connectorFor(newLink).ShouldNotBeNull();
         connectorFor(newLink).BeginItem.ShouldBeEqualTo(shapeFor(_newMolecule));
      }
   }

   public class When_replacing_the_model_of_the_view : concern_for_DevExpressDiagramView
   {
      private DiagramModel _otherModel;
      private MoleculeNode _otherMolecule;

      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         _otherModel = new DiagramModel();
         _otherMolecule = _otherModel.CreateNode<MoleculeNode>("X", new PointF(1, 1), _otherModel);
      }

      protected override void Because()
      {
         sut.Model = _otherModel;
      }

      [Observation]
      public void should_show_only_the_nodes_of_the_new_model()
      {
         shapes.Select(shape => shape.Tag).ShouldOnlyContain(_otherMolecule);
         connectors.ShouldBeEmpty();
      }

      [Observation]
      public void should_not_react_to_changes_of_the_old_model()
      {
         _educt.Location = new PointF(999, 999);
         shapeFor(_educt).ShouldBeNull();
      }
   }

   public class When_toggling_the_grid_of_the_view : concern_for_DevExpressDiagramView
   {
      protected override void Because()
      {
         sut.GridVisible = true;
      }

      [Observation]
      public void should_show_the_grid_and_snap_to_it()
      {
         sut.GridVisible.ShouldBeTrue();
         sut.DiagramControl.OptionsView.ShowGrid.ShouldBeTrue();
         sut.DiagramControl.OptionsBehavior.SnapToGrid.ShouldBeTrue();
      }

      [Observation]
      public void should_hide_the_grid_again()
      {
         sut.GridVisible = false;
         sut.GridVisible.ShouldBeFalse();
         sut.DiagramControl.OptionsBehavior.SnapToGrid.ShouldBeFalse();
      }
   }

   public class When_setting_the_background_color_of_the_view : concern_for_DevExpressDiagramView
   {
      protected override void Because()
      {
         sut.SetBackColor(Color.Beige);
      }

      [Observation]
      public void should_set_the_background_of_the_diagram_control()
      {
         sut.DiagramControl.BackColor.ShouldBeEqualTo(Color.Beige);
      }
   }

   public class When_zooming_the_view : concern_for_DevExpressDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         sut.DiagramControl.OptionsView.ZoomFactor = 1F;
      }

      [Observation]
      public void should_multiply_the_zoom_factor()
      {
         sut.Zoom(new PointF(10, 10), 1.5F);
         sut.DiagramControl.OptionsView.ZoomFactor.ShouldBeEqualTo(1.5F);
      }

      [Observation]
      public void should_clamp_the_zoom_factor_to_the_limits()
      {
         sut.Zoom(new PointF(10, 10), 100F);
         sut.DiagramControl.OptionsView.ZoomFactor.ShouldBeEqualTo(Assets.Diagram.Base.MaxLimitDocScale);
         sut.Zoom(new PointF(10, 10), 0.001F);
         sut.DiagramControl.OptionsView.ZoomFactor.ShouldBeEqualTo(Assets.Diagram.Base.MinLimitDocScale);
      }
   }

   public class When_making_the_view_read_only : concern_for_DevExpressDiagramView
   {
      protected override void Because()
      {
         sut.MakeReadOnly();
      }

      [Observation]
      public void should_only_allow_moving_and_zooming()
      {
         var optionsProtection = sut.DiagramControl.OptionsProtection;
         optionsProtection.IsReadOnly.ShouldBeTrue();
         optionsProtection.AllowMoveItems.ShouldBeEqualTo(true);
         optionsProtection.AllowZoom.ShouldBeTrue();
      }
   }

   public class When_the_user_deletes_the_selection_in_the_view : concern_for_DevExpressDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         sut.Select(_educt);
         sut.Select(_eductLink);
      }

      protected override void Because()
      {
         sut.DiagramControl.DeleteSelectedItems();
      }

      [Observation]
      public void should_not_delete_anything_from_the_diagram_control()
      {
         shapes.Count.ShouldBeEqualTo(4);
         connectors.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_hand_the_selected_nodes_and_links_over_to_the_deletion_hook()
      {
         sut.DeletingNodes.ShouldOnlyContain(_educt);
         sut.DeletingLinks.ShouldOnlyContain(_eductLink);
      }
   }

   public class When_the_user_deletes_the_selection_in_a_read_only_view : concern_for_DevExpressDiagramView
   {
      protected override void Context()
      {
         base.Context();
         sut.MakeReadOnly();
         sut.Model = _model;
         sut.Select(_educt);
      }

      protected override void Because()
      {
         sut.DiagramControl.DeleteSelectedItems();
      }

      [Observation]
      public void should_neither_delete_nor_notify()
      {
         shapes.Count.ShouldBeEqualTo(4);
         sut.DeletingNodes.ShouldBeNull();
      }
   }

   public class When_the_user_connects_a_molecule_to_the_modifier_point_of_a_reaction : concern_for_DevExpressDiagramView
   {
      private DiagramConnector _userConnector;

      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         _userConnector = new DiagramConnector(shapeFor(_product), shapeFor(_reaction))
         {
            BeginItemPointIndex = -1,
            EndItemPointIndex = ReactionConnectionPoints.MODIFIER_INDEX
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
      public void should_notify_the_link_creation_with_the_reaction_link_type_as_port()
      {
         sut.CreatedLink.HasValue.ShouldBeTrue();
         sut.CreatedLink.Value.fromNode.ShouldBeEqualTo(_product);
         sut.CreatedLink.Value.toNode.ShouldBeEqualTo(_reaction);
         sut.CreatedLink.Value.fromPort.ShouldBeNull();
         sut.CreatedLink.Value.toPort.ShouldBeEqualTo(ReactionLinkType.Modifier);
      }
   }

   public class When_the_user_connects_nodes_in_a_read_only_view : concern_for_DevExpressDiagramView
   {
      private DiagramConnector _userConnector;

      protected override void Context()
      {
         base.Context();
         sut.MakeReadOnly();
         sut.Model = _model;
         _userConnector = new DiagramConnector(shapeFor(_product), shapeFor(_reaction)) {EndItemPointIndex = ReactionConnectionPoints.MODIFIER_INDEX};
      }

      protected override void Because()
      {
         sut.DiagramControl.Items.Add(_userConnector);
      }

      [Observation]
      public void should_remove_the_connector_without_notifying()
      {
         sut.DiagramControl.Items.Contains(_userConnector).ShouldBeFalse();
         sut.CreatedLink.HasValue.ShouldBeFalse();
      }
   }

   public class When_a_foreign_item_is_added_to_the_diagram_control : concern_for_DevExpressDiagramView
   {
      private DiagramShape _foreignShape;

      protected override void Context()
      {
         base.Context();
         sut.Model = _model;
         _foreignShape = new DiagramShape();
      }

      protected override void Because()
      {
         sut.DiagramControl.Items.Add(_foreignShape);
      }

      [Observation]
      public void should_remove_it_again()
      {
         sut.DiagramControl.Items.Contains(_foreignShape).ShouldBeFalse();
         shapes.Count.ShouldBeEqualTo(4);
      }
   }

   public class When_setting_a_spatial_structure_diagram_model : concern_for_DevExpressDiagramView
   {
      private DiagramModel _spatialModel;
      private ContainerNode _organism;
      private ContainerNode _liver;
      private ContainerNode _plasma;
      private ContainerNode _kidney;
      private NeighborhoodNode _neighborhood;

      protected override void Context()
      {
         base.Context();
         _spatialModel = new DiagramModel();
         _organism = container("Organism", _spatialModel, 100, 100, 500, 300);
         _liver = container("Liver", _organism, 120, 130, 200, 150);
         _liver.IsExpanded = false;
         _plasma = container("Plasma", _liver, 140, 160, 100, 50);
         _kidney = container("Kidney", _organism, 400, 130, 150, 80);
         _neighborhood = _spatialModel.CreateNode<NeighborhoodNode>("pls_kidney", PointF.Empty, _organism);
         _neighborhood.Initialize(_plasma, _kidney);
      }

      private ContainerNode container(string name, IContainerBase parent, float x, float y, float width, float height)
      {
         var node = _spatialModel.CreateNode<ContainerNode>(name, new PointF(x, y), parent);
         node.Name = name;
         node.Size = new SizeF(width, height);
         return node;
      }

      protected override void Because()
      {
         sut.Model = _spatialModel;
      }

      private DiagramContainer containerFor(ContainerNode node) => allItems().OfType<DiagramContainer>().Single(x => x.Tag == node);

      private IEnumerable<DiagramItem> allItems() => sut.DiagramControl.Items.Concat(sut.DiagramControl.Items.OfType<DiagramContainer>().SelectMany(descendantsOf));

      private static IEnumerable<DiagramItem> descendantsOf(DiagramContainer container) => container.Items.Concat(container.Items.OfType<DiagramContainer>().SelectMany(descendantsOf));

      [Observation]
      public void should_nest_the_container_items_like_the_model_and_position_children_relative_to_their_parent()
      {
         var organism = containerFor(_organism);
         var liver = containerFor(_liver);
         organism.Items.ShouldContain(liver);
         organism.Position.ShouldBeEqualTo(new PointFloat(_organism.Location));
         liver.Position.ShouldBeEqualTo(new PointFloat(_liver.Location.X - _organism.Location.X, _liver.Location.Y - _organism.Location.Y));
         organism.Size.ShouldBeEqualTo(_organism.Size);
      }

      [Observation]
      public void should_not_create_items_for_the_children_of_a_collapsed_container_and_shrink_it_to_its_label()
      {
         allItems().Any(item => item.Tag == _plasma).ShouldBeFalse();
         containerFor(_liver).Items.Count.ShouldBeEqualTo(0);
         (containerFor(_liver).Size.Height < 30).ShouldBeTrue();
      }

      [Observation]
      public void should_glue_links_into_a_collapsed_container_to_the_container_itself()
      {
         var neighborhoodShape = allItems().OfType<DiagramShape>().Single(x => x.Tag == _neighborhood);
         var connectors = allItems().OfType<DiagramConnector>().Where(x => x.BeginItem == neighborhoodShape).ToList();
         connectors.Count.ShouldBeEqualTo(2);
         connectors.Select(x => x.EndItem).ShouldOnlyContain(containerFor(_liver), containerFor(_kidney));
      }

      [Observation]
      public void should_show_the_children_again_when_the_container_is_expanded_in_the_model()
      {
         _liver.IsExpanded = true;
         sut.Refresh();
         containerFor(_liver).Items.Single().Tag.ShouldBeEqualTo(_plasma);
         containerFor(_liver).Size.ShouldBeEqualTo(_liver.Size);
         allItems().OfType<DiagramConnector>().Where(x => x.EndItem?.Tag == _plasma).Count().ShouldBeEqualTo(1);
      }
   }

   public class When_setting_a_spatial_structure_diagram_model_with_an_explicitly_hidden_container : concern_for_DevExpressDiagramView
   {
      private DiagramModel _spatialModel;
      private ContainerNode _organism;
      private ContainerNode _plasma;
      private ContainerNode _kidney;
      private NeighborhoodNode _neighborhood;

      protected override void Context()
      {
         base.Context();
         _spatialModel = new DiagramModel();
         _organism = container("Organism", _spatialModel, 100, 100, 500, 300);
         _plasma = container("Plasma", _organism, 140, 160, 100, 50);
         _kidney = container("Kidney", _organism, 400, 130, 150, 80);
         _kidney.Hidden = true;
         _neighborhood = _spatialModel.CreateNode<NeighborhoodNode>("pls_kidney", PointF.Empty, _organism);
         _neighborhood.Initialize(_plasma, _kidney);
      }

      private ContainerNode container(string name, IContainerBase parent, float x, float y, float width, float height)
      {
         var node = _spatialModel.CreateNode<ContainerNode>(name, new PointF(x, y), parent);
         node.Name = name;
         node.Size = new SizeF(width, height);
         return node;
      }

      protected override void Because()
      {
         sut.Model = _spatialModel;
      }

      [Observation]
      public void should_not_draw_a_connector_to_the_parent_of_a_container_hidden_in_its_own_right()
      {
         var items = sut.DiagramControl.Items.Concat(sut.DiagramControl.Items.OfType<DiagramContainer>().SelectMany(x => x.Items)).ToList();
         var endItems = items.OfType<DiagramConnector>().Select(connector => connector.EndItem).ToList();
         endItems.Any(item => ReferenceEquals(item?.Tag, _organism)).ShouldBeFalse();
      }
   }
}
