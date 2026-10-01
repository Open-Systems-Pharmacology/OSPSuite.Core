using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_ElementBaseNode : ContextSpecification<ElementBaseNode>
   {
      protected DiagramColors _diagramColors;

      protected override void Context()
      {
         sut = new ElementBaseNode();
         _diagramColors = new DiagramColors();
      }
   }

   public class When_creating_an_element_base_node : concern_for_ElementBaseNode
   {
      [Observation]
      public void should_use_the_default_layout_values()
      {
         sut.NodeBaseSize.ShouldBeEqualTo(new SizeF(20, 20));
         sut.NodeSize.ShouldBeEqualTo(NodeSize.Middle);
         sut.Size.ShouldBeEqualTo(new SizeF(20, 20));
         sut.Location.ShouldBeEqualTo(PointF.Empty);
         sut.UserFlags.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_be_visible_unfixed_and_linkable()
      {
         sut.IsVisible.ShouldBeTrue();
         sut.Hidden.ShouldBeFalse();
         sut.Visible.ShouldBeTrue();
         sut.LocationFixed.ShouldBeFalse();
         sut.CanLink.ShouldBeTrue();
         sut.BorderWidth.ShouldBeEqualTo(1F);
      }

      [Observation]
      public void should_not_have_a_parent_or_links()
      {
         sut.GetParent().ShouldBeNull();
         sut.Links.ShouldBeEmpty();
         sut.GetLinkedNodes<IBaseNode>().ShouldBeEmpty();
      }
   }

   public class When_retrieving_the_size_of_an_element_base_node : concern_for_ElementBaseNode
   {
      protected override void Context()
      {
         base.Context();
         sut.NodeBaseSize = new SizeF(40, 20);
      }

      [Observation]
      public void should_scale_the_base_size_with_the_node_size()
      {
         sut.NodeSize = NodeSize.Small;
         sut.Size.ShouldBeEqualTo(new SizeF(20, 10));
         sut.NodeSize = NodeSize.Middle;
         sut.Size.ShouldBeEqualTo(new SizeF(40, 20));
         sut.NodeSize = NodeSize.Large;
         sut.Size.ShouldBeEqualTo(new SizeF(60, 30));
      }

      [Observation]
      public void setting_the_size_should_be_ignored()
      {
         sut.NodeSize = NodeSize.Middle;
         sut.Size = new SizeF(1, 1);
         sut.Size.ShouldBeEqualTo(new SizeF(40, 20));
      }
   }

   public class When_locating_an_element_base_node : concern_for_ElementBaseNode
   {
      protected override void Because()
      {
         sut.Location = new PointF(100, 50);
      }

      [Observation]
      public void the_location_should_be_the_center_of_the_node()
      {
         sut.Center.ShouldBeEqualTo(new PointF(100, 50));
         sut.Bounds.ShouldBeEqualTo(new RectangleF(90, 40, 20, 20));
      }
   }

   public class When_setting_the_center_of_an_element_base_node : concern_for_ElementBaseNode
   {
      protected override void Because()
      {
         sut.Center = new PointF(7, 8);
      }

      [Observation]
      public void should_move_the_location()
      {
         sut.Location.ShouldBeEqualTo(new PointF(7, 8));
      }
   }

   public class When_setting_the_bounds_of_an_element_base_node : concern_for_ElementBaseNode
   {
      protected override void Because()
      {
         sut.Bounds = new RectangleF(0, 0, 50, 50);
      }

      [Observation]
      public void should_move_the_node_to_the_center_of_the_bounds()
      {
         sut.Location.ShouldBeEqualTo(new PointF(25, 25));
      }

      [Observation]
      public void should_keep_the_size_derived_from_the_node_size()
      {
         sut.Size.ShouldBeEqualTo(new SizeF(20, 20));
         sut.Bounds.ShouldBeEqualTo(new RectangleF(15, 15, 20, 20));
      }
   }

   public class When_setting_the_colors_of_a_fixed_element_base_node : concern_for_ElementBaseNode
   {
      protected override void Context()
      {
         base.Context();
         sut.LocationFixed = true;
      }

      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_use_the_fixed_border()
      {
         sut.BorderWidth.ShouldBeEqualTo(2F);
         sut.BorderColor.ShouldBeEqualTo(_diagramColors.BorderFixed);
      }
   }

   public class When_setting_the_colors_of_an_unfixed_element_base_node : concern_for_ElementBaseNode
   {
      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_use_the_unfixed_border()
      {
         sut.BorderWidth.ShouldBeEqualTo(1F);
         sut.BorderColor.ShouldBeEqualTo(_diagramColors.BorderUnfixed);
      }
   }

   public class When_retrieving_the_label_properties_of_an_element_base_node : concern_for_ElementBaseNode
   {
      [Observation]
      public void small_nodes_should_not_show_a_label()
      {
         sut.NodeSize = NodeSize.Small;
         sut.LabelVisible.ShouldBeFalse();
      }

      [Observation]
      public void middle_nodes_should_show_a_small_gray_label()
      {
         sut.NodeSize = NodeSize.Middle;
         sut.LabelVisible.ShouldBeTrue();
         sut.LabelFontSize.ShouldBeEqualTo(8F);
         sut.LabelColor.ShouldBeEqualTo(SuiteColors.Gray);
      }

      [Observation]
      public void large_nodes_should_show_a_larger_black_label()
      {
         sut.NodeSize = NodeSize.Large;
         sut.LabelVisible.ShouldBeTrue();
         sut.LabelFontSize.ShouldBeEqualTo(10F);
         sut.LabelColor.ShouldBeEqualTo(Color.Black);
      }
   }

   public class When_computing_the_alpha_of_an_element_base_node : concern_for_ElementBaseNode
   {
      [Observation]
      public void should_reduce_the_opacity_for_each_smaller_node_size()
      {
         sut.NodeSize = NodeSize.Large;
         sut.Alpha(0.5F).ShouldBeEqualTo(255);
         sut.NodeSize = NodeSize.Middle;
         sut.Alpha(0.5F).ShouldBeEqualTo(128);
         sut.NodeSize = NodeSize.Small;
         sut.Alpha(0.5F).ShouldBeEqualTo(64);
      }
   }

   public class When_copying_an_element_base_node : concern_for_ElementBaseNode
   {
      private ElementBaseNode _copy;

      protected override void Context()
      {
         base.Context();
         sut.Id = "id";
         sut.Name = "name";
         sut.Description = "description";
         sut.Location = new PointF(1, 2);
         sut.NodeSize = NodeSize.Large;
         sut.NodeBaseSize = new SizeF(10, 10);
         sut.LocationFixed = true;
         sut.Hidden = true;
         sut.IsVisible = false;
         sut.UserFlags = 7;
         sut.CanLink = false;
         sut.SetColorFrom(_diagramColors);
      }

      protected override void Because()
      {
         _copy = sut.Copy() as ElementBaseNode;
      }

      [Observation]
      public void should_return_a_new_node_of_the_same_type()
      {
         _copy.ShouldNotBeNull();
         ReferenceEquals(_copy, sut).ShouldBeFalse();
         _copy.GetType().ShouldBeEqualTo(typeof(ElementBaseNode));
         _copy.GetParent().ShouldBeNull();
      }

      [Observation]
      public void should_copy_all_properties()
      {
         _copy.Id.ShouldBeEqualTo("id");
         _copy.Name.ShouldBeEqualTo("name");
         _copy.Description.ShouldBeEqualTo("description");
         _copy.Location.ShouldBeEqualTo(new PointF(1, 2));
         _copy.NodeSize.ShouldBeEqualTo(NodeSize.Large);
         _copy.NodeBaseSize.ShouldBeEqualTo(new SizeF(10, 10));
         _copy.LocationFixed.ShouldBeTrue();
         _copy.Hidden.ShouldBeTrue();
         _copy.IsVisible.ShouldBeFalse();
         _copy.UserFlags.ShouldBeEqualTo(7);
         _copy.CanLink.ShouldBeFalse();
         _copy.BorderWidth.ShouldBeEqualTo(2F);
         _copy.BorderColor.ShouldBeEqualTo(_diagramColors.BorderFixed);
      }
   }

   public abstract class concern_for_ElementBaseNode_layout_source : concern_for_ElementBaseNode
   {
      protected ElementBaseNode _source;

      protected override void Context()
      {
         base.Context();
         _source = new ElementBaseNode
         {
            Id = "source",
            Location = new PointF(150, 160),
            NodeSize = NodeSize.Large,
            LocationFixed = true,
            Hidden = true,
            IsVisible = false
         };
      }
   }

   public class When_copying_the_layout_info_from_a_node_in_another_container : concern_for_ElementBaseNode_layout_source
   {
      protected override void Context()
      {
         base.Context();
         var sourceContainer = new ContainerNode {Id = "c1", Location = new PointF(100, 100)};
         sourceContainer.AddChildNode(_source);
         var targetContainer = new ContainerNode {Id = "c2"};
         targetContainer.AddChildNode(sut);
      }

      protected override void Because()
      {
         sut.CopyLayoutInfoFrom(_source, new PointF(300, 300));
      }

      [Observation]
      public void should_copy_the_location_relative_to_the_parent_location()
      {
         sut.Location.ShouldBeEqualTo(new PointF(350, 360));
      }

      [Observation]
      public void should_copy_the_layout_properties()
      {
         sut.NodeSize.ShouldBeEqualTo(NodeSize.Large);
         sut.LocationFixed.ShouldBeTrue();
         sut.Hidden.ShouldBeTrue();
         sut.IsVisible.ShouldBeFalse();
      }

      [Observation]
      public void should_not_copy_the_identity()
      {
         sut.Id.ShouldBeNull();
      }
   }

   public class When_copying_the_layout_info_from_a_node_without_parent : concern_for_ElementBaseNode_layout_source
   {
      protected override void Because()
      {
         sut.CopyLayoutInfoFrom(_source, new PointF(300, 300));
      }

      [Observation]
      public void should_copy_the_location_as_is()
      {
         sut.Location.ShouldBeEqualTo(new PointF(150, 160));
         sut.NodeSize.ShouldBeEqualTo(NodeSize.Large);
      }
   }

   public class When_copying_the_layout_info_from_a_node_that_is_not_an_element_node : concern_for_ElementBaseNode
   {
      protected override void Because()
      {
         sut.CopyLayoutInfoFrom(new ContainerNode {Location = new PointF(5, 5), LocationFixed = true}, PointF.Empty);
      }

      [Observation]
      public void should_not_change_the_node()
      {
         sut.Location.ShouldBeEqualTo(PointF.Empty);
         sut.LocationFixed.ShouldBeFalse();
      }
   }

   public class When_an_element_base_node_is_hidden : concern_for_ElementBaseNode
   {
      protected override void Because()
      {
         sut.Hidden = true;
      }

      [Observation]
      public void should_not_be_visible()
      {
         sut.IsVisible.ShouldBeTrue();
         sut.Visible.ShouldBeFalse();
      }
   }

   public class When_an_element_base_node_is_set_invisible : concern_for_ElementBaseNode
   {
      protected override void Because()
      {
         sut.Visible = false;
      }

      [Observation]
      public void should_not_be_visible()
      {
         sut.IsVisible.ShouldBeFalse();
         sut.Hidden.ShouldBeFalse();
         sut.Visible.ShouldBeFalse();
      }
   }
}
