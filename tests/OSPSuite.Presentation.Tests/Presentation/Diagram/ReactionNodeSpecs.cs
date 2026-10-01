using System.Drawing;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_ReactionNode : ContextSpecification<ReactionNode>
   {
      protected DiagramColors _diagramColors;

      protected override void Context()
      {
         sut = new ReactionNode();
         _diagramColors = new DiagramColors();
      }

      protected ReactionLink linkTo(ReactionLinkType type, MoleculeNode moleculeNode)
      {
         var link = new ReactionLink();
         link.Initialize(type, sut, moleculeNode);
         return link;
      }
   }

   public class When_creating_a_reaction_node : concern_for_ReactionNode
   {
      [Observation]
      public void should_be_a_middle_node_based_on_30x20()
      {
         sut.NodeBaseSize.ShouldBeEqualTo(new SizeF(30, 20));
         sut.NodeSize.ShouldBeEqualTo(NodeSize.Middle);
         sut.Size.ShouldBeEqualTo(new SizeF(30, 20));
      }

      [Observation]
      public void should_scale_with_the_node_size()
      {
         sut.NodeSize = NodeSize.Small;
         sut.Size.ShouldBeEqualTo(new SizeF(15, 10));
         sut.NodeSize = NodeSize.Large;
         sut.Size.ShouldBeEqualTo(new SizeF(45, 30));
      }

      [Observation]
      public void should_be_flagged_as_reaction_node()
      {
         sut.UserFlags.ShouldBeEqualTo(NodeLayoutType.REACTION_NODE);
         sut.UserFlags.ShouldBeEqualTo(6);
      }

      [Observation]
      public void should_display_the_educts_on_the_left()
      {
         sut.DisplayEductsRight.ShouldBeFalse();
      }

      [Observation]
      public void should_not_have_links()
      {
         sut.ReactionLinks.ShouldBeEmpty();
      }
   }

   public class When_setting_the_colors_of_a_middle_reaction_node : concern_for_ReactionNode
   {
      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_set_the_fill_and_port_colors_with_the_node_size_opacity()
      {
         sut.PortColor.ShouldBeEqualTo(Color.FromArgb(128, _diagramColors.ReactionNode));
         sut.FillColor.ShouldBeEqualTo(Color.FromArgb(64, _diagramColors.ReactionNode));
      }

      [Observation]
      public void should_set_the_reaction_port_colors()
      {
         sut.EductPortColor.ShouldBeEqualTo(Color.FromArgb(128, _diagramColors.ReactionPortEduct));
         sut.ProductPortColor.ShouldBeEqualTo(Color.FromArgb(128, _diagramColors.ReactionPortProduct));
         sut.ModifierPortColor.ShouldBeEqualTo(Color.FromArgb(128, _diagramColors.ReactionPortModifier));
      }

      [Observation]
      public void should_use_the_unfixed_border()
      {
         sut.BorderWidth.ShouldBeEqualTo(1F);
         sut.BorderColor.ShouldBeEqualTo(_diagramColors.BorderUnfixed);
      }
   }

   public class When_setting_the_colors_of_a_large_fixed_reaction_node : concern_for_ReactionNode
   {
      protected override void Context()
      {
         base.Context();
         sut.NodeSize = NodeSize.Large;
         sut.LocationFixed = true;
      }

      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_use_the_full_opacity()
      {
         sut.PortColor.ShouldBeEqualTo(Color.FromArgb(255, _diagramColors.ReactionNode));
         sut.FillColor.ShouldBeEqualTo(Color.FromArgb(128, _diagramColors.ReactionNode));
         sut.EductPortColor.ShouldBeEqualTo(Color.FromArgb(255, _diagramColors.ReactionPortEduct));
         sut.ProductPortColor.ShouldBeEqualTo(Color.FromArgb(255, _diagramColors.ReactionPortProduct));
         sut.ModifierPortColor.ShouldBeEqualTo(Color.FromArgb(255, _diagramColors.ReactionPortModifier));
      }

      [Observation]
      public void should_use_the_fixed_border()
      {
         sut.BorderWidth.ShouldBeEqualTo(2F);
         sut.BorderColor.ShouldBeEqualTo(_diagramColors.BorderFixed);
      }
   }

   public class When_setting_the_colors_of_a_small_reaction_node : concern_for_ReactionNode
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
      public void should_reduce_the_opacity_twice()
      {
         sut.PortColor.ShouldBeEqualTo(Color.FromArgb(64, _diagramColors.ReactionNode));
         sut.FillColor.ShouldBeEqualTo(Color.FromArgb(32, _diagramColors.ReactionNode));
         sut.EductPortColor.ShouldBeEqualTo(Color.FromArgb(64, _diagramColors.ReactionPortEduct));
      }
   }

   public class When_retrieving_the_label_properties_of_a_reaction_node : concern_for_ReactionNode
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

   public abstract class concern_for_linked_ReactionNode : concern_for_ReactionNode
   {
      protected MoleculeNode _educt;
      protected MoleculeNode _product;
      protected MoleculeNode _modifier;
      protected ReactionLink _eductLink;
      protected ReactionLink _productLink;
      protected ReactionLink _modifierLink;

      protected override void Context()
      {
         base.Context();
         _educt = new MoleculeNode {Id = "e", Name = "E"};
         _product = new MoleculeNode {Id = "p", Name = "P"};
         _modifier = new MoleculeNode {Id = "m", Name = "M"};
         _eductLink = linkTo(ReactionLinkType.Educt, _educt);
         _productLink = linkTo(ReactionLinkType.Product, _product);
         _modifierLink = linkTo(ReactionLinkType.Modifier, _modifier);
      }
   }

   public class When_a_reaction_node_is_linked_to_molecules : concern_for_linked_ReactionNode
   {
      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_return_the_typed_reaction_links()
      {
         sut.ReactionLinks.ShouldOnlyContain(_eductLink, _productLink, _modifierLink);
         sut.Links.Count.ShouldBeEqualTo(3);
         sut.GetLinkedNodes<MoleculeNode>().ShouldOnlyContain(_educt, _product, _modifier);
      }

      [Observation]
      public void should_propagate_the_colors_to_its_links()
      {
         _eductLink.Color.ShouldBeEqualTo(_diagramColors.ReactionLinkEduct);
         _productLink.Color.ShouldBeEqualTo(_diagramColors.ReactionLinkProduct);
         _modifierLink.Color.ShouldBeEqualTo(_diagramColors.ReactionLinkModifier);
      }
   }

   public class When_clearing_the_links_of_a_reaction_node : concern_for_linked_ReactionNode
   {
      protected override void Because()
      {
         sut.ClearLinks();
      }

      [Observation]
      public void should_remove_the_links_from_the_reaction_node()
      {
         sut.Links.ShouldBeEmpty();
         sut.ReactionLinks.ShouldBeEmpty();
      }

      [Observation]
      public void should_remove_the_links_from_the_molecule_nodes()
      {
         _educt.Links.ShouldBeEmpty();
         _product.Links.ShouldBeEmpty();
         _modifier.Links.ShouldBeEmpty();
         _educt.IsConnectedToReactions.ShouldBeFalse();
      }
   }

   public class When_copying_a_reaction_node : concern_for_linked_ReactionNode
   {
      private IBaseNode _copy;

      protected override void Context()
      {
         base.Context();
         sut.Id = "r";
         sut.Name = "R";
         sut.Description = "reaction";
         sut.Location = new PointF(5, 6);
         sut.NodeSize = NodeSize.Large;
         sut.DisplayEductsRight = true;
         sut.Hidden = true;
      }

      protected override void Because()
      {
         _copy = sut.Copy();
      }

      [Observation]
      public void should_return_a_reaction_node_with_the_same_layout_info()
      {
         _copy.ShouldBeAnInstanceOf<ReactionNode>();
         ReferenceEquals(_copy, sut).ShouldBeFalse();
         _copy.Id.ShouldBeEqualTo("r");
         _copy.Name.ShouldBeEqualTo("R");
         _copy.Description.ShouldBeEqualTo("reaction");
         _copy.Location.ShouldBeEqualTo(new PointF(5, 6));
         _copy.Hidden.ShouldBeTrue();
         _copy.UserFlags.ShouldBeEqualTo(NodeLayoutType.REACTION_NODE);
         var copiedReactionNode = (ReactionNode) _copy;
         copiedReactionNode.NodeSize.ShouldBeEqualTo(NodeSize.Large);
         copiedReactionNode.DisplayEductsRight.ShouldBeTrue();
         copiedReactionNode.NodeBaseSize.ShouldBeEqualTo(new SizeF(30, 20));
      }

      [Observation]
      public void should_not_copy_the_links()
      {
         ((ReactionNode) _copy).ReactionLinks.ShouldBeEmpty();
      }
   }

   public class When_copying_the_layout_info_from_another_reaction_node : concern_for_ReactionNode
   {
      protected override void Because()
      {
         sut.CopyLayoutInfoFrom(new ReactionNode
         {
            Id = "other",
            Location = new PointF(40, 50),
            NodeSize = NodeSize.Small,
            LocationFixed = true,
            DisplayEductsRight = true
         }, PointF.Empty);
      }

      [Observation]
      public void should_copy_the_layout_info_including_the_educt_side()
      {
         sut.Location.ShouldBeEqualTo(new PointF(40, 50));
         sut.NodeSize.ShouldBeEqualTo(NodeSize.Small);
         sut.LocationFixed.ShouldBeTrue();
         sut.DisplayEductsRight.ShouldBeTrue();
         sut.Id.ShouldBeNull();
      }
   }

   public class When_copying_the_layout_info_from_a_molecule_node_into_a_reaction_node : concern_for_ReactionNode
   {
      protected override void Because()
      {
         sut.CopyLayoutInfoFrom(new MoleculeNode {Location = new PointF(40, 50), NodeSize = NodeSize.Small}, PointF.Empty);
      }

      [Observation]
      public void should_copy_the_common_layout_info_and_keep_the_educt_side()
      {
         sut.Location.ShouldBeEqualTo(new PointF(40, 50));
         sut.NodeSize.ShouldBeEqualTo(NodeSize.Small);
         sut.DisplayEductsRight.ShouldBeFalse();
      }
   }
}
