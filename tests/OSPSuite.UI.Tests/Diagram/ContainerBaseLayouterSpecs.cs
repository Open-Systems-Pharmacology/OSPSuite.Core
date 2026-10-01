using System.Collections.Generic;
using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using ContainerBaseLayouter = OSPSuite.UI.Diagram.Elements.ContainerBaseLayouter;

namespace OSPSuite.UI.Diagram
{
   public abstract class concern_for_ContainerBaseLayouter : ContextSpecification<ContainerBaseLayouter>
   {
      protected DiagramModel _model;
      protected MoleculeNode _existingNode;

      protected override void Context()
      {
         sut = new ContainerBaseLayouter();
         _model = new DiagramModel();
         _existingNode = _model.CreateNode<MoleculeNode>("A", new PointF(50, 50), _model);
      }
   }

   public class When_laying_out_a_ui_free_reaction_diagram_without_free_nodes : concern_for_ContainerBaseLayouter
   {
      private ReactionNode _reaction;
      private MoleculeNode _product;

      protected override void Context()
      {
         base.Context();
         _reaction = _model.CreateNode<ReactionNode>("R", new PointF(50, 50), _model);
         _reaction.DisplayEductsRight = true;
         _product = _model.CreateNode<MoleculeNode>("P", new PointF(50, 50), _model);
         new ReactionLink().Initialize(ReactionLinkType.Educt, _reaction, _existingNode);
         new ReactionLink().Initialize(ReactionLinkType.Product, _reaction, _product);
      }

      protected override void Because()
      {
         sut.DoForceLayout(_model, null, 0);
      }

      [Observation]
      public void should_lay_out_the_diagram_in_layers()
      {
         (_existingNode.Location.X < _reaction.Location.X).ShouldBeTrue();
         (_reaction.Location.X < _product.Location.X).ShouldBeTrue();
         _model.IsLayouted.ShouldBeTrue();
      }

      [Observation]
      public void should_keep_the_display_educts_right_setting()
      {
         _reaction.DisplayEductsRight.ShouldBeTrue();
      }

      [Observation]
      public void should_be_undoable()
      {
         _model.Undo();
         _existingNode.Location.ShouldBeEqualTo(new PointF(50, 50));
         _reaction.Location.ShouldBeEqualTo(new PointF(50, 50));
      }
   }

   public class When_laying_out_a_ui_free_reaction_diagram_with_free_nodes_overlapping_existing_nodes : concern_for_ContainerBaseLayouter
   {
      private MoleculeNode _freeNode;
      private MoleculeNode _otherFreeNode;

      protected override void Context()
      {
         base.Context();
         _freeNode = _model.CreateNode<MoleculeNode>("B", _existingNode.Location, _model);
         _otherFreeNode = _model.CreateNode<MoleculeNode>("C", new PointF(500, 500), _model);
      }

      protected override void Because()
      {
         sut.DoForceLayout(_model, new List<IHasLayoutInfo> {_freeNode, _otherFreeNode}, 0);
      }

      [Observation]
      public void should_move_the_overlapping_free_node_down_until_it_is_free()
      {
         _freeNode.Location.ShouldBeEqualTo(new PointF(50, 50 + Assets.Diagram.Base.InsertLocationOffset.Y));
         _freeNode.Bounds.IntersectsWith(_existingNode.Bounds).ShouldBeFalse();
      }

      [Observation]
      public void should_not_move_free_nodes_that_do_not_overlap()
      {
         _otherFreeNode.Location.ShouldBeEqualTo(new PointF(500, 500));
      }

      [Observation]
      public void should_not_move_the_existing_nodes()
      {
         _existingNode.Location.ShouldBeEqualTo(new PointF(50, 50));
      }

      [Observation]
      public void should_be_undoable()
      {
         _model.Undo();
         _freeNode.Location.ShouldBeEqualTo(new PointF(50, 50));
      }
   }

   public class When_laying_out_a_ui_free_reaction_diagram_with_a_free_node_overlapping_a_hidden_node : concern_for_ContainerBaseLayouter
   {
      private MoleculeNode _freeNode;

      protected override void Context()
      {
         base.Context();
         _existingNode.Hidden = true;
         _freeNode = _model.CreateNode<MoleculeNode>("B", _existingNode.Location, _model);
      }

      protected override void Because()
      {
         sut.DoForceLayout(_model, new List<IHasLayoutInfo> {_freeNode}, 0);
      }

      [Observation]
      public void should_ignore_hidden_nodes()
      {
         _freeNode.Location.ShouldBeEqualTo(new PointF(50, 50));
      }
   }
}
