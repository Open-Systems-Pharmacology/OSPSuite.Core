using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_DiagramModel : ContextSpecification<DiagramModel>
   {
      protected int _changedCount;

      protected override void Context()
      {
         sut = new DiagramModel {DiagramOptions = new DiagramOptions()};
         _changedCount = 0;
         sut.Changed += () => _changedCount++;
      }

      protected MoleculeNode createMolecule(string id, PointF location, IContainerBase parent = null)
      {
         var node = sut.CreateNode<MoleculeNode>(id, location, parent ?? sut);
         node.Name = id;
         return node;
      }

      protected ReactionNode createReaction(string id, PointF location, IContainerBase parent = null)
      {
         var node = sut.CreateNode<ReactionNode>(id, location, parent ?? sut);
         node.Name = id;
         return node;
      }

      protected ContainerNode createContainer(string id, IContainerBase parent = null)
      {
         var node = sut.CreateNode<ContainerNode>(id, PointF.Empty, parent ?? sut);
         node.Name = id;
         return node;
      }

      protected static ReactionLink link(ReactionLinkType type, ReactionNode reactionNode, MoleculeNode moleculeNode)
      {
         var reactionLink = new ReactionLink();
         reactionLink.Initialize(type, reactionNode, moleculeNode);
         return reactionLink;
      }
   }

   public class When_creating_an_empty_diagram_model : concern_for_DiagramModel
   {
      [Observation]
      public void should_be_empty()
      {
         sut.IsEmpty().ShouldBeTrue();
         sut.GetDirectChildren<IBaseNode>().ShouldBeEmpty();
         sut.Bounds.ShouldBeEqualTo(RectangleF.Empty);
         sut.IsLayouted.ShouldBeFalse();
         sut.InUpdate.ShouldBeFalse();
      }

      [Observation]
      public void should_not_find_unknown_nodes()
      {
         sut.GetNode("unknown").ShouldBeNull();
         sut.GetNode(null).ShouldBeNull();
         sut.FindByName("unknown").ShouldBeNull();
      }

      [Observation]
      public void should_create_a_new_model_of_the_same_type()
      {
         sut.Create().ShouldBeAnInstanceOf<DiagramModel>();
      }
   }

   public class When_creating_nodes_in_the_diagram_model : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;
      private ReactionNode _reactionNode;

      protected override void Context()
      {
         base.Context();
         sut.DiagramOptions.DefaultNodeSizeMolecule = NodeSize.Small;
         sut.DiagramOptions.DefaultNodeSizeReaction = NodeSize.Large;
      }

      protected override void Because()
      {
         _moleculeNode = sut.CreateNode<MoleculeNode>("m", new PointF(1, 2), sut);
         _reactionNode = sut.CreateNode<ReactionNode>("r", new PointF(3, 4), sut);
      }

      [Observation]
      public void should_register_the_nodes_by_id()
      {
         sut.GetNode("m").ShouldBeEqualTo(_moleculeNode);
         sut.GetNode<MoleculeNode>("m").ShouldBeEqualTo(_moleculeNode);
         sut.GetNode<ReactionNode>("r").ShouldBeEqualTo(_reactionNode);
         sut.GetNode<ReactionNode>("m").ShouldBeNull();
      }

      [Observation]
      public void should_add_the_nodes_as_direct_children_of_the_model()
      {
         _moleculeNode.GetParent().ShouldBeEqualTo(sut);
         sut.ContainsChildNode(_moleculeNode, recursive: false).ShouldBeTrue();
         sut.ContainsChildNode(_moleculeNode, recursive: true).ShouldBeTrue();
         sut.GetDirectChildren<IBaseNode>().ShouldOnlyContain(_moleculeNode, _reactionNode);
         sut.IsEmpty().ShouldBeFalse();
      }

      [Observation]
      public void should_set_the_location()
      {
         _moleculeNode.Location.ShouldBeEqualTo(new PointF(1, 2));
         _reactionNode.Location.ShouldBeEqualTo(new PointF(3, 4));
      }

      [Observation]
      public void should_apply_the_default_node_sizes_from_the_diagram_options()
      {
         _moleculeNode.NodeSize.ShouldBeEqualTo(NodeSize.Small);
         _reactionNode.NodeSize.ShouldBeEqualTo(NodeSize.Large);
      }

      [Observation]
      public void should_raise_the_changed_event_for_each_node()
      {
         _changedCount.ShouldBeEqualTo(2);
      }
   }

   public class When_removing_a_linked_node : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;
      private ReactionNode _reactionNode;

      protected override void Context()
      {
         base.Context();
         _moleculeNode = createMolecule("m", new PointF(0, 0));
         _reactionNode = createReaction("r", new PointF(50, 0));
         link(ReactionLinkType.Educt, _reactionNode, _moleculeNode);
      }

      protected override void Because()
      {
         sut.RemoveNode("m");
         sut.RemoveNode("unknown");
      }

      [Observation]
      public void should_remove_the_node_from_the_model()
      {
         sut.GetNode("m").ShouldBeNull();
         sut.GetDirectChildren<IBaseNode>().ShouldOnlyContain(_reactionNode);
         _moleculeNode.GetParent().ShouldBeNull();
         sut.ContainsChildNode(_moleculeNode, recursive: true).ShouldBeFalse();
      }

      [Observation]
      public void should_unlink_the_node()
      {
         _reactionNode.Links.ShouldBeEmpty();
         _moleculeNode.Links.ShouldBeEmpty();
         sut.GetAllChildren<ReactionLink>().ShouldBeEmpty();
      }
   }

   public class When_renaming_a_node : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;

      protected override void Context()
      {
         base.Context();
         _moleculeNode = createMolecule("m", PointF.Empty);
         createReaction("r", PointF.Empty);
      }

      protected override void Because()
      {
         sut.RenameNode("m", "renamed");
         sut.RenameNode("unknown", "ignored");
      }

      [Observation]
      public void should_rename_the_node()
      {
         _moleculeNode.Name.ShouldBeEqualTo("renamed");
      }

      [Observation]
      public void should_find_the_node_by_its_new_name()
      {
         sut.FindByName("renamed").ShouldBeEqualTo(_moleculeNode);
         sut.FindByName("m").ShouldBeNull();
      }
   }

   public class When_replacing_node_ids : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;
      private ReactionNode _reactionNode;

      protected override void Context()
      {
         base.Context();
         _moleculeNode = createMolecule("m", PointF.Empty);
         _reactionNode = createReaction("r", PointF.Empty);
      }

      protected override void Because()
      {
         sut.ReplaceNodeIds(new Dictionary<string, string> {{"m", "m2"}, {"unknown", "u2"}});
      }

      [Observation]
      public void should_change_the_id_of_the_found_node()
      {
         _moleculeNode.Id.ShouldBeEqualTo("m2");
         sut.GetNode("m2").ShouldBeEqualTo(_moleculeNode);
         sut.GetNode("m").ShouldBeNull();
      }

      [Observation]
      public void should_leave_other_nodes_untouched()
      {
         sut.GetNode("r").ShouldBeEqualTo(_reactionNode);
         sut.GetNode("u2").ShouldBeNull();
      }
   }

   public class When_clearing_the_diagram_model : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;

      protected override void Context()
      {
         base.Context();
         _moleculeNode = createMolecule("m", PointF.Empty);
         createReaction("r", PointF.Empty);
         _changedCount = 0;
      }

      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_be_empty()
      {
         sut.IsEmpty().ShouldBeTrue();
         sut.GetNode("m").ShouldBeNull();
         sut.GetAllChildren<IBaseNode>().ShouldBeEmpty();
      }

      [Observation]
      public void should_detach_the_nodes()
      {
         _moleculeNode.GetParent().ShouldBeNull();
         sut.ContainsChildNode(_moleculeNode, recursive: false).ShouldBeFalse();
      }

      [Observation]
      public void should_raise_the_changed_event_once()
      {
         _changedCount.ShouldBeEqualTo(1);
      }
   }

   public class When_retrieving_the_children_of_a_diagram_model_with_containers : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;
      private ReactionNode _reactionNode;
      private ContainerNode _containerNode;
      private MoleculeNode _nestedMoleculeNode;
      private ReactionLink _link;

      protected override void Context()
      {
         base.Context();
         _moleculeNode = createMolecule("m", PointF.Empty);
         _reactionNode = createReaction("r", PointF.Empty);
         _containerNode = createContainer("c");
         _nestedMoleculeNode = createMolecule("nested", PointF.Empty, _containerNode);
         _link = link(ReactionLinkType.Product, _reactionNode, _moleculeNode);
      }

      [Observation]
      public void direct_children_should_contain_the_top_level_nodes_and_their_links()
      {
         sut.GetDirectChildren<IBaseNode>().ShouldOnlyContain(_moleculeNode, _reactionNode, _containerNode);
         sut.GetDirectChildren<MoleculeNode>().ShouldOnlyContain(_moleculeNode);
         sut.GetDirectChildren<ReactionLink>().ShouldOnlyContain(_link);
      }

      [Observation]
      public void all_children_should_also_contain_the_nested_nodes()
      {
         sut.GetAllChildren<IBaseNode>().ShouldOnlyContain(_moleculeNode, _reactionNode, _containerNode, _nestedMoleculeNode);
         sut.GetAllChildren<MoleculeNode>().ShouldOnlyContain(_moleculeNode, _nestedMoleculeNode);
         sut.GetAllChildren<ReactionLink>().ShouldOnlyContain(_link);
         sut.AllLinks.ShouldOnlyContain(_link);
      }

      [Observation]
      public void should_register_the_nested_node_by_id()
      {
         sut.GetNode("nested").ShouldBeEqualTo(_nestedMoleculeNode);
         _nestedMoleculeNode.GetParent().ShouldBeEqualTo(_containerNode);
      }

      [Observation]
      public void should_contain_the_nested_node_only_recursively()
      {
         sut.ContainsChildNode(_nestedMoleculeNode, recursive: true).ShouldBeTrue();
         sut.ContainsChildNode(_nestedMoleculeNode, recursive: false).ShouldBeFalse();
         sut.ContainsChildNode(_containerNode, recursive: false).ShouldBeTrue();
         _containerNode.ContainsChildNode(_nestedMoleculeNode, recursive: false).ShouldBeTrue();
      }
   }

   public abstract class concern_for_DiagramModel_with_two_nodes : concern_for_DiagramModel
   {
      protected ElementBaseNode _node1;
      protected ElementBaseNode _node2;

      protected override void Context()
      {
         base.Context();
         _node1 = sut.CreateNode<ElementBaseNode>("n1", new PointF(10, 10), sut);
         _node2 = sut.CreateNode<ElementBaseNode>("n2", new PointF(110, 110), sut);
      }
   }

   public class When_retrieving_the_bounds_of_a_diagram_model : concern_for_DiagramModel_with_two_nodes
   {
      [Observation]
      public void bounds_should_be_the_union_of_the_node_bounds()
      {
         sut.Bounds.ShouldBeEqualTo(new RectangleF(0, 0, 120, 120));
         sut.CalculateBounds().ShouldBeEqualTo(new RectangleF(0, 0, 120, 120));
         sut.Size.ShouldBeEqualTo(new SizeF(120, 120));
         sut.Location.ShouldBeEqualTo(new PointF(0, 0));
         sut.Center.ShouldBeEqualTo(new PointF(60, 60));
      }
   }

   public class When_setting_the_location_of_a_diagram_model : concern_for_DiagramModel_with_two_nodes
   {
      protected override void Because()
      {
         sut.Location = new PointF(50, -20);
      }

      [Observation]
      public void should_report_the_location_as_origin_without_moving_the_nodes()
      {
         _node1.Location.ShouldBeEqualTo(new PointF(10, 10));
         _node2.Location.ShouldBeEqualTo(new PointF(110, 110));
         sut.Location.ShouldBeEqualTo(new PointF(50, -20));
         sut.Size.ShouldBeEqualTo(new SizeF(120, 120));
      }

      [Observation]
      public void should_forget_the_origin_when_cleared()
      {
         sut.Clear();
         sut.Location.ShouldBeEqualTo(PointF.Empty);
      }
   }

   public class When_setting_the_center_of_a_diagram_model : concern_for_DiagramModel_with_two_nodes
   {
      protected override void Because()
      {
         sut.Center = new PointF(0, 0);
      }

      [Observation]
      public void should_move_the_origin_without_moving_the_nodes()
      {
         _node1.Location.ShouldBeEqualTo(new PointF(10, 10));
         _node2.Location.ShouldBeEqualTo(new PointF(110, 110));
         sut.Center.ShouldBeEqualTo(new PointF(0, 0));
         sut.Location.ShouldBeEqualTo(new PointF(-60, -60));
      }
   }

   public class When_changing_node_properties : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;

      protected override void Context()
      {
         base.Context();
         _moleculeNode = createMolecule("m", new PointF(0, 0));
         _changedCount = 0;
      }

      protected override void Because()
      {
         _moleculeNode.Location = new PointF(5, 5);
         _moleculeNode.Location = new PointF(5, 5);
         _moleculeNode.Hidden = false;
         _moleculeNode.NodeSize = _moleculeNode.NodeSize;
      }

      [Observation]
      public void should_raise_the_changed_event_only_for_actual_changes()
      {
         _changedCount.ShouldBeEqualTo(1);
      }
   }

   public class When_changing_node_properties_inside_an_update : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;
      private ReactionNode _reactionNode;
      private int _changedCountBeforeEndUpdate;
      private bool _inUpdate;

      protected override void Context()
      {
         base.Context();
         _moleculeNode = createMolecule("m", new PointF(0, 0));
         _reactionNode = createReaction("r", new PointF(0, 0));
         _changedCount = 0;
      }

      protected override void Because()
      {
         sut.BeginUpdate();
         sut.BeginUpdate();
         _moleculeNode.Location = new PointF(5, 5);
         _reactionNode.NodeSize = NodeSize.Large;
         _reactionNode.DisplayEductsRight = true;
         sut.EndUpdate();
         _inUpdate = sut.InUpdate;
         _changedCountBeforeEndUpdate = _changedCount;
         sut.EndUpdate();
      }

      [Observation]
      public void should_not_raise_the_changed_event_while_updating()
      {
         _inUpdate.ShouldBeTrue();
         _changedCountBeforeEndUpdate.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_raise_the_changed_event_once_at_the_end_of_the_outermost_update()
      {
         _changedCount.ShouldBeEqualTo(1);
         sut.InUpdate.ShouldBeFalse();
      }
   }

   public class When_ending_an_update_without_changes : concern_for_DiagramModel
   {
      protected override void Context()
      {
         base.Context();
         createMolecule("m", new PointF(0, 0));
         _changedCount = 0;
      }

      protected override void Because()
      {
         sut.BeginUpdate();
         sut.EndUpdate();
         sut.EndUpdate();
      }

      [Observation]
      public void should_not_raise_the_changed_event()
      {
         _changedCount.ShouldBeEqualTo(0);
      }
   }

   public class When_undoing_a_transaction : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;
      private ReactionNode _reactionNode;
      private bool _started;
      private bool _finished;
      private bool _finishedWithoutStart;

      protected override void Context()
      {
         base.Context();
         _moleculeNode = createMolecule("m", new PointF(0, 0));
         _reactionNode = createReaction("r", new PointF(100, 0));
         _finishedWithoutStart = sut.FinishTransaction("");
      }

      protected override void Because()
      {
         _started = sut.StartTransaction();
         _moleculeNode.Location = new PointF(50, 50);
         _reactionNode.LocationFixed = true;
         _finished = sut.FinishTransaction("");
         sut.StartTransaction();
         sut.FinishTransaction("");
         _changedCount = 0;
         sut.Undo();
      }

      [Observation]
      public void should_report_the_transaction_state()
      {
         _finishedWithoutStart.ShouldBeFalse();
         _started.ShouldBeTrue();
         _finished.ShouldBeTrue();
      }

      [Observation]
      public void should_discard_the_empty_transaction_and_restore_the_layout_of_the_last_changing_transaction()
      {
         _moleculeNode.Location.ShouldBeEqualTo(new PointF(0, 0));
         _reactionNode.LocationFixed.ShouldBeFalse();
      }

      [Observation]
      public void should_raise_the_changed_event_once()
      {
         _changedCount.ShouldBeEqualTo(1);
      }

      [Observation]
      public void undoing_again_should_not_change_anything()
      {
         sut.Undo();
         _moleculeNode.Location.ShouldBeEqualTo(new PointF(0, 0));
      }
   }

   public class When_clearing_the_undo_stack : concern_for_DiagramModel
   {
      private MoleculeNode _moleculeNode;

      protected override void Context()
      {
         base.Context();
         _moleculeNode = createMolecule("m", new PointF(0, 0));
         sut.StartTransaction();
         _moleculeNode.Location = new PointF(50, 50);
         sut.FinishTransaction("");
      }

      protected override void Because()
      {
         sut.ClearUndoStack();
         sut.Undo();
      }

      [Observation]
      public void should_not_undo_anything()
      {
         _moleculeNode.Location.ShouldBeEqualTo(new PointF(50, 50));
      }
   }

   public abstract class concern_for_DiagramModel_copy : concern_for_DiagramModel
   {
      protected MoleculeNode _moleculeNode;
      protected ReactionNode _reactionNode;
      protected IDiagramModel _copy;
      protected MoleculeNode _copiedMoleculeNode;
      protected ReactionNode _copiedReactionNode;

      protected override void Context()
      {
         base.Context();
         sut.IsLayouted = true;
         _moleculeNode = createMolecule("m", new PointF(10, 20));
         _moleculeNode.LocationFixed = true;
         _moleculeNode.Hidden = true;
         _moleculeNode.NodeSize = NodeSize.Small;
         _moleculeNode.Description = "molecule";
         _reactionNode = createReaction("r", new PointF(30, 40));
         _reactionNode.DisplayEductsRight = true;
         link(ReactionLinkType.Educt, _reactionNode, _moleculeNode);
         link(ReactionLinkType.Modifier, _reactionNode, _moleculeNode);
      }

      protected override void Because()
      {
         _copy = sut.CreateCopy();
         _copiedMoleculeNode = _copy.GetNode<MoleculeNode>("m");
         _copiedReactionNode = _copy.GetNode<ReactionNode>("r");
      }
   }

   public class When_creating_a_copy_of_the_diagram_model : concern_for_DiagramModel_copy
   {
      [Observation]
      public void should_return_a_new_model_with_the_same_options_and_layout_state()
      {
         _copy.ShouldBeAnInstanceOf<DiagramModel>();
         ReferenceEquals(_copy, sut).ShouldBeFalse();
         _copy.IsLayouted.ShouldBeTrue();
         _copy.DiagramOptions.ShouldBeEqualTo(sut.DiagramOptions);
      }

      [Observation]
      public void should_copy_the_nodes_with_their_properties()
      {
         _copiedMoleculeNode.ShouldNotBeNull();
         ReferenceEquals(_copiedMoleculeNode, _moleculeNode).ShouldBeFalse();
         _copiedMoleculeNode.Name.ShouldBeEqualTo("m");
         _copiedMoleculeNode.Description.ShouldBeEqualTo("molecule");
         _copiedMoleculeNode.Location.ShouldBeEqualTo(new PointF(10, 20));
         _copiedMoleculeNode.LocationFixed.ShouldBeTrue();
         _copiedMoleculeNode.Hidden.ShouldBeTrue();
         _copiedMoleculeNode.NodeSize.ShouldBeEqualTo(NodeSize.Small);
         _copiedMoleculeNode.GetParent().ShouldBeEqualTo(_copy);
         _copiedReactionNode.Location.ShouldBeEqualTo(new PointF(30, 40));
         _copiedReactionNode.DisplayEductsRight.ShouldBeTrue();
         _copy.GetDirectChildren<IBaseNode>().Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_copy_the_reaction_links_between_the_copied_nodes()
      {
         var links = _copy.GetAllChildren<ReactionLink>().ToList();
         links.Count.ShouldBeEqualTo(2);
         links.Select(x => x.Type).ShouldOnlyContain(ReactionLinkType.Educt, ReactionLinkType.Modifier);
         links.Each(x => x.ReactionNode.ShouldBeEqualTo(_copiedReactionNode));
         links.Each(x => x.MoleculeNode.ShouldBeEqualTo(_copiedMoleculeNode));
         _copiedReactionNode.Links.Count.ShouldBeEqualTo(2);
         _copiedMoleculeNode.IsConnectedToReactions.ShouldBeTrue();
      }
   }

   public class When_changing_the_source_of_a_copied_diagram_model : concern_for_DiagramModel_copy
   {
      protected override void Because()
      {
         base.Because();
         _moleculeNode.Location = new PointF(99, 99);
         _moleculeNode.Name = "changed";
         sut.RemoveNode("r");
      }

      [Observation]
      public void the_copy_should_not_be_affected()
      {
         _copiedMoleculeNode.Location.ShouldBeEqualTo(new PointF(10, 20));
         _copiedMoleculeNode.Name.ShouldBeEqualTo("m");
         _copy.GetNode("r").ShouldBeEqualTo(_copiedReactionNode);
         _copiedReactionNode.Links.Count.ShouldBeEqualTo(2);
         _copy.GetAllChildren<ReactionLink>().Count().ShouldBeEqualTo(2);
      }
   }

   public class When_creating_a_copy_of_a_container_node : concern_for_DiagramModel
   {
      private ContainerNode _containerNode;
      private IDiagramModel _copy;

      protected override void Context()
      {
         base.Context();
         createMolecule("m", PointF.Empty);
         _containerNode = createContainer("c");
         _containerNode.Location = new PointF(10, 10);
         _containerNode.IsLogical = true;
         createMolecule("nested", new PointF(20, 30), _containerNode);
         createContainer("c2");
      }

      protected override void Because()
      {
         _copy = sut.CreateCopy("c");
      }

      [Observation]
      public void should_copy_the_container_node_as_top_level_node()
      {
         var copiedContainer = _copy.GetNode<ContainerNode>("c");
         copiedContainer.ShouldNotBeNull();
         ReferenceEquals(copiedContainer, _containerNode).ShouldBeFalse();
         copiedContainer.GetParent().ShouldBeEqualTo(_copy);
         copiedContainer.IsLogical.ShouldBeTrue();
         copiedContainer.Location.ShouldBeEqualTo(_containerNode.Location);
         _copy.GetDirectChildren<IBaseNode>().ShouldOnlyContain(copiedContainer);
      }

      [Observation]
      public void should_copy_the_children_of_the_container()
      {
         var copiedNested = _copy.GetNode<MoleculeNode>("nested");
         copiedNested.ShouldNotBeNull();
         ReferenceEquals(copiedNested.GetParent(), _copy.GetNode("c")).ShouldBeTrue();
         copiedNested.Location.ShouldBeEqualTo(new PointF(20, 30));
         _copy.GetAllChildren<IBaseNode>().Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_not_copy_nodes_outside_of_the_container()
      {
         _copy.GetNode("m").ShouldBeNull();
         _copy.GetNode("c2").ShouldBeNull();
      }

      [Observation]
      public void should_return_null_for_an_unknown_container()
      {
         sut.CreateCopy("unknown").ShouldBeNull();
         sut.CreateCopy("m").ShouldBeNull();
      }
   }

   public class When_changing_the_z_order_of_nodes : concern_for_DiagramModel
   {
      private MoleculeNode _node1;
      private MoleculeNode _node2;
      private MoleculeNode _node3;

      protected override void Context()
      {
         base.Context();
         _node1 = createMolecule("1", PointF.Empty);
         _node2 = createMolecule("2", PointF.Empty);
         _node3 = createMolecule("3", PointF.Empty);
         _changedCount = 0;
      }

      protected override void Because()
      {
         _node1.ToFront();
         _node3.ToBack();
      }

      [Observation]
      public void should_reorder_the_direct_children()
      {
         sut.GetDirectChildren<IBaseNode>().ShouldOnlyContainInOrder(_node3, _node2, _node1);
      }

      [Observation]
      public void should_raise_the_changed_event()
      {
         _changedCount.ShouldBeEqualTo(2);
      }
   }

   public abstract class concern_for_DiagramModel_with_nested_containers : concern_for_DiagramModel
   {
      protected ContainerNode _containerNode;
      protected ContainerNode _nestedContainerNode;
      protected MoleculeNode _rootMoleculeNode;
      protected MoleculeNode _nestedMoleculeNode;
      protected MoleculeNode _deepMoleculeNode;
      protected IBaseNode[] _allNodes;

      protected override void Context()
      {
         base.Context();
         _rootMoleculeNode = createMolecule("m", PointF.Empty);
         _containerNode = createContainer("c");
         _nestedMoleculeNode = createMolecule("nested", PointF.Empty, _containerNode);
         _nestedContainerNode = createContainer("c2", _containerNode);
         _deepMoleculeNode = createMolecule("deep", PointF.Empty, _nestedContainerNode);
         _allNodes = new IBaseNode[] {_rootMoleculeNode, _containerNode, _nestedMoleculeNode, _nestedContainerNode, _deepMoleculeNode};
      }
   }

   public class When_hiding_all_nodes_recursively : concern_for_DiagramModel_with_nested_containers
   {
      protected override void Because()
      {
         sut.SetHiddenRecursive(true);
      }

      [Observation]
      public void should_hide_all_nodes()
      {
         _allNodes.Each(x => x.Hidden.ShouldBeTrue());
         _allNodes.Each(x => x.Visible.ShouldBeFalse());
      }
   }

   public class When_showing_all_nodes_recursively : concern_for_DiagramModel_with_nested_containers
   {
      protected override void Context()
      {
         base.Context();
         sut.SetHiddenRecursive(true);
      }

      protected override void Because()
      {
         sut.SetHiddenRecursive(false);
      }

      [Observation]
      public void should_show_all_nodes()
      {
         _allNodes.Each(x => x.Hidden.ShouldBeFalse());
         _allNodes.Each(x => x.Visible.ShouldBeTrue());
      }
   }

   public class When_collapsing_the_diagram_model_one_level : concern_for_DiagramModel_with_nested_containers
   {
      protected override void Because()
      {
         sut.Collapse(1);
      }

      [Observation]
      public void should_collapse_only_the_top_level_containers()
      {
         _containerNode.IsExpanded.ShouldBeFalse();
         _nestedContainerNode.IsExpanded.ShouldBeTrue();
      }

      [Observation]
      public void nodes_inside_collapsed_containers_should_not_be_visible()
      {
         _rootMoleculeNode.Visible.ShouldBeTrue();
         _containerNode.Visible.ShouldBeTrue();
         _nestedMoleculeNode.Visible.ShouldBeFalse();
         _nestedContainerNode.Visible.ShouldBeFalse();
         _deepMoleculeNode.Visible.ShouldBeFalse();
      }
   }

   public class When_collapsing_the_diagram_model_two_levels : concern_for_DiagramModel_with_nested_containers
   {
      protected override void Because()
      {
         sut.Collapse(2);
      }

      [Observation]
      public void should_collapse_all_containers()
      {
         _containerNode.IsExpanded.ShouldBeFalse();
         _nestedContainerNode.IsExpanded.ShouldBeFalse();
      }
   }

   public class When_expanding_a_collapsed_diagram_model_one_level : concern_for_DiagramModel_with_nested_containers
   {
      protected override void Context()
      {
         base.Context();
         sut.Collapse(2);
      }

      protected override void Because()
      {
         sut.Expand(1);
      }

      [Observation]
      public void should_expand_only_the_top_level_containers()
      {
         _containerNode.IsExpanded.ShouldBeTrue();
         _nestedContainerNode.IsExpanded.ShouldBeFalse();
         _nestedMoleculeNode.Visible.ShouldBeTrue();
         _deepMoleculeNode.Visible.ShouldBeFalse();
      }

      [Observation]
      public void expanding_all_levels_should_expand_the_nested_containers_as_well()
      {
         sut.Expand(2);
         _nestedContainerNode.IsExpanded.ShouldBeTrue();
         _deepMoleculeNode.Visible.ShouldBeTrue();
      }
   }

   public class When_setting_and_showing_the_default_expansion : concern_for_DiagramModel_with_nested_containers
   {
      protected override void Context()
      {
         base.Context();
         _nestedContainerNode.IsExpanded = false;
         sut.SetDefaultExpansion();
         _nestedContainerNode.IsExpanded = true;
         _containerNode.IsExpanded = false;
      }

      protected override void Because()
      {
         sut.ShowDefaultExpansion();
      }

      [Observation]
      public void should_restore_the_expansion_state_of_all_containers()
      {
         _containerNode.IsExpanded.ShouldBeTrue();
         _nestedContainerNode.IsExpanded.ShouldBeFalse();
      }
   }

   public class When_undoing_the_expansion_of_a_journal_page : concern_for_DiagramModel
   {
      private JournalPageNode _journalPageNode;

      protected override void Context()
      {
         base.Context();
         _journalPageNode = sut.CreateNode<JournalPageNode>("page", new PointF(10, 10), sut);
      }

      protected override void Because()
      {
         sut.StartTransaction();
         _journalPageNode.IsExpanded = false;
         sut.FinishTransaction("Collapse");
         sut.Undo();
      }

      [Observation]
      public void should_restore_the_expanded_state()
      {
         _journalPageNode.IsExpanded.ShouldBeTrue();
      }
   }
}
