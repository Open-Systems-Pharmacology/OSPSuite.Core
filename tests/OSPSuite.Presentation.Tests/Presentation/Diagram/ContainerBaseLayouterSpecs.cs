using System.Collections.Generic;
using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Diagram.Services;

namespace OSPSuite.Presentation.Diagram
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

   public class When_placing_free_nodes_overlapping_existing_nodes : concern_for_ContainerBaseLayouter
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

   public class When_placing_a_free_node_overlapping_a_hidden_node : concern_for_ContainerBaseLayouter
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

   public class When_placing_no_free_nodes : concern_for_ContainerBaseLayouter
   {
      protected override void Because()
      {
         sut.DoForceLayout(_model, null, 0);
      }

      [Observation]
      public void should_leave_the_diagram_alone()
      {
         _existingNode.Location.ShouldBeEqualTo(new PointF(50, 50));
      }
   }

   public class When_placing_a_free_container_that_has_children : concern_for_ContainerBaseLayouter
   {
      private ContainerNode _freeContainer;
      private MoleculeNode _child;

      protected override void Context()
      {
         base.Context();
         _freeContainer = _model.CreateNode<ContainerNode>("c1", new PointF(300, 300), _model);
         _child = _model.CreateNode<MoleculeNode>("B", new PointF(300, 300), _freeContainer);
      }

      protected override void Because()
      {
         sut.DoForceLayout(_model, new List<IHasLayoutInfo> {_freeContainer}, 0);
      }

      [Observation]
      public void should_not_move_the_container_away_from_its_own_children()
      {
         _child.Location.ShouldBeEqualTo(new PointF(300, 300));
      }
   }
}
