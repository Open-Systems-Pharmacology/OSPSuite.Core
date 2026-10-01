using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_MoleculeNode : ContextSpecification<MoleculeNode>
   {
      protected DiagramColors _diagramColors;

      protected override void Context()
      {
         sut = new MoleculeNode();
         _diagramColors = new DiagramColors();
      }
   }

   public class When_creating_a_molecule_node : concern_for_MoleculeNode
   {
      [Observation]
      public void should_be_a_large_node_based_on_15x15()
      {
         sut.NodeBaseSize.ShouldBeEqualTo(new SizeF(15, 15));
         sut.NodeSize.ShouldBeEqualTo(NodeSize.Large);
         sut.Size.ShouldBeEqualTo(new SizeF(22.5F, 22.5F));
      }

      [Observation]
      public void should_be_flagged_as_molecule_node()
      {
         sut.UserFlags.ShouldBeEqualTo(NodeLayoutType.MOLECULE_NODE);
         sut.UserFlags.ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_not_be_connected_to_reactions()
      {
         sut.IsConnectedToReactions.ShouldBeFalse();
      }
   }

   public class When_setting_the_colors_of_a_large_molecule_node : concern_for_MoleculeNode
   {
      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_use_the_full_opacity_for_the_port_and_the_port_opacity_for_the_fill()
      {
         sut.PortColor.ShouldBeEqualTo(Color.FromArgb(255, _diagramColors.MoleculeNode));
         sut.FillColor.ShouldBeEqualTo(Color.FromArgb(128, _diagramColors.MoleculeNode));
      }

      [Observation]
      public void should_use_the_unfixed_border()
      {
         sut.BorderWidth.ShouldBeEqualTo(1F);
         sut.BorderColor.ShouldBeEqualTo(_diagramColors.BorderUnfixed);
      }
   }

   public class When_setting_the_colors_of_a_middle_molecule_node : concern_for_MoleculeNode
   {
      protected override void Context()
      {
         base.Context();
         sut.NodeSize = NodeSize.Middle;
         sut.LocationFixed = true;
      }

      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_reduce_the_opacity_by_the_node_size_opacity()
      {
         sut.PortColor.ShouldBeEqualTo(Color.FromArgb(128, _diagramColors.MoleculeNode));
         sut.FillColor.ShouldBeEqualTo(Color.FromArgb(64, _diagramColors.MoleculeNode));
      }

      [Observation]
      public void should_use_the_fixed_border()
      {
         sut.BorderWidth.ShouldBeEqualTo(2F);
         sut.BorderColor.ShouldBeEqualTo(_diagramColors.BorderFixed);
      }
   }

   public class When_setting_the_colors_of_a_small_molecule_node : concern_for_MoleculeNode
   {
      protected override void Context()
      {
         base.Context();
         sut.NodeSize = NodeSize.Small;
      }

      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_reduce_the_opacity_twice_by_the_node_size_opacity()
      {
         sut.PortColor.ShouldBeEqualTo(Color.FromArgb(64, _diagramColors.MoleculeNode));
         sut.FillColor.ShouldBeEqualTo(Color.FromArgb(32, _diagramColors.MoleculeNode));
      }
   }

   public class When_retrieving_the_label_properties_of_a_molecule_node : concern_for_MoleculeNode
   {
      [Observation]
      public void small_nodes_should_show_a_tiny_black_label()
      {
         sut.NodeSize = NodeSize.Small;
         sut.LabelVisible.ShouldBeTrue();
         sut.LabelFontSize.ShouldBeEqualTo(7F);
         sut.LabelColor.ShouldBeEqualTo(Color.Black);
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

   public class When_a_molecule_node_is_linked_to_a_reaction : concern_for_MoleculeNode
   {
      private ReactionNode _reactionNode;
      private ReactionLink _link;

      protected override void Context()
      {
         base.Context();
         _reactionNode = new ReactionNode {Id = "r"};
         _link = new ReactionLink();
         _link.Initialize(ReactionLinkType.Educt, _reactionNode, sut);
      }

      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_be_connected_to_reactions()
      {
         sut.IsConnectedToReactions.ShouldBeTrue();
         sut.Links.ShouldOnlyContain(_link);
         sut.GetLinkedNodes<ReactionNode>().ShouldOnlyContain(_reactionNode);
      }

      [Observation]
      public void should_propagate_the_colors_to_its_links()
      {
         _link.Color.ShouldBeEqualTo(_diagramColors.ReactionLinkEduct);
      }
   }

   public class When_copying_a_molecule_node : concern_for_MoleculeNode
   {
      private IBaseNode _copy;

      protected override void Context()
      {
         base.Context();
         sut.Id = "m";
         sut.Name = "A";
         sut.Location = new PointF(3, 4);
         sut.NodeSize = NodeSize.Small;
         sut.LocationFixed = true;
         var link = new ReactionLink();
         link.Initialize(ReactionLinkType.Product, new ReactionNode {Id = "r"}, sut);
      }

      protected override void Because()
      {
         _copy = sut.Copy();
      }

      [Observation]
      public void should_return_a_molecule_node_with_the_same_layout_info()
      {
         _copy.ShouldBeAnInstanceOf<MoleculeNode>();
         ReferenceEquals(_copy, sut).ShouldBeFalse();
         _copy.Id.ShouldBeEqualTo("m");
         _copy.Name.ShouldBeEqualTo("A");
         _copy.Location.ShouldBeEqualTo(new PointF(3, 4));
         _copy.LocationFixed.ShouldBeTrue();
         _copy.UserFlags.ShouldBeEqualTo(NodeLayoutType.MOLECULE_NODE);
         var copiedMoleculeNode = (MoleculeNode) _copy;
         copiedMoleculeNode.NodeSize.ShouldBeEqualTo(NodeSize.Small);
         copiedMoleculeNode.NodeBaseSize.ShouldBeEqualTo(new SizeF(15, 15));
      }

      [Observation]
      public void should_not_copy_the_links()
      {
         ((MoleculeNode) _copy).IsConnectedToReactions.ShouldBeFalse();
      }
   }
}
