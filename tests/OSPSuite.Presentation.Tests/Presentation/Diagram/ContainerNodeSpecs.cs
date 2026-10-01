using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_ContainerNode : ContextSpecification<ContainerNode>
   {
      protected DiagramModel _model;

      protected override void Context()
      {
         _model = new DiagramModel();
         sut = createContainer("organism", _model, 100, 100);
      }

      protected ContainerNode createContainer(string id, IContainerBase parent, float x, float y)
      {
         var node = _model.CreateNode<ContainerNode>(id, new PointF(x, y), parent);
         node.Name = id;
         node.Size = new SizeF(100, 60);
         return node;
      }

      protected static RectangleF frameOf(RectangleF children)
      {
         return new RectangleF(children.X - ContainerNode.LEFT_MARGIN, children.Y - ContainerNode.TOP_MARGIN,
            children.Width + ContainerNode.LEFT_MARGIN + ContainerNode.RIGHT_MARGIN, children.Height + ContainerNode.TOP_MARGIN + ContainerNode.BOTTOM_MARGIN);
      }
   }

   public class When_retrieving_the_bounds_of_an_expanded_container_node : concern_for_ContainerNode
   {
      private ContainerNode _liver;
      private ContainerNode _kidney;

      protected override void Context()
      {
         base.Context();
         _liver = createContainer("liver", sut, 400, 900);
         _kidney = createContainer("kidney", sut, 700, 1200);
      }

      [Observation]
      public void should_enclose_the_children_with_the_container_margins()
      {
         sut.Bounds.ShouldBeEqualTo(frameOf(RectangleF.Union(_liver.Bounds, _kidney.Bounds)));
         sut.Bounds.Contains(_liver.Bounds).ShouldBeTrue();
         sut.Bounds.Contains(_kidney.Bounds).ShouldBeTrue();
      }

      [Observation]
      public void should_follow_a_child_that_moves()
      {
         _kidney.Location = new PointF(1500, 2000);
         sut.Bounds.Contains(_kidney.Bounds).ShouldBeTrue();
      }

      [Observation]
      public void should_ignore_children_that_are_not_visible()
      {
         _kidney.IsVisible = false;
         sut.Bounds.ShouldBeEqualTo(frameOf(_liver.Bounds));
      }
   }

   public class When_retrieving_the_bounds_of_a_container_node_without_visible_children : concern_for_ContainerNode
   {
      [Observation]
      public void should_use_its_own_location_and_size()
      {
         sut.Location.ShouldBeEqualTo(new PointF(100, 100));
         sut.Size.ShouldBeEqualTo(new SizeF(100, 60));
      }
   }

   public class When_retrieving_the_bounds_of_a_collapsed_container_node : concern_for_ContainerNode
   {
      protected override void Context()
      {
         base.Context();
         createContainer("liver", sut, 400, 900);
         sut.IsExpanded = false;
      }

      [Observation]
      public void should_use_its_own_location_and_size()
      {
         sut.Location.ShouldBeEqualTo(new PointF(100, 100));
         sut.Size.ShouldBeEqualTo(new SizeF(100, 60));
      }
   }

   public class When_moving_an_expanded_container_node : concern_for_ContainerNode
   {
      private ContainerNode _liver;
      private PointF _offset;

      protected override void Context()
      {
         base.Context();
         _liver = createContainer("liver", sut, 400, 900);
         _offset = new PointF(_liver.Location.X - sut.Location.X, _liver.Location.Y - sut.Location.Y);
      }

      protected override void Because()
      {
         sut.Location = new PointF(0, 0);
      }

      [Observation]
      public void should_move_its_children_along_and_keep_the_new_location()
      {
         sut.Location.ShouldBeEqualTo(new PointF(0, 0));
         _liver.Location.ShouldBeEqualTo(new PointF(_offset.X, _offset.Y));
      }
   }

   public class When_nesting_container_nodes : concern_for_ContainerNode
   {
      private ContainerNode _liver;
      private ContainerNode _plasma;

      protected override void Context()
      {
         base.Context();
         _liver = createContainer("liver", sut, 400, 900);
         _plasma = createContainer("plasma", _liver, 420, 950);
      }

      [Observation]
      public void should_enclose_the_whole_subtree()
      {
         _liver.Bounds.Contains(_plasma.Bounds).ShouldBeTrue();
         sut.Bounds.Contains(_liver.Bounds).ShouldBeTrue();
         sut.Bounds.Contains(_plasma.Bounds).ShouldBeTrue();
      }
   }
}
