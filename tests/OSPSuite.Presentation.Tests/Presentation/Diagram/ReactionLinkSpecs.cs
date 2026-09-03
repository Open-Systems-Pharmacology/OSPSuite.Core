using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_ReactionLink : ContextSpecification<ReactionLink>
   {
      protected ReactionNode _reactionNode;
      protected MoleculeNode _moleculeNode;
      protected DiagramColors _diagramColors;

      protected override void Context()
      {
         sut = new ReactionLink();
         _reactionNode = new ReactionNode {Id = "r"};
         _moleculeNode = new MoleculeNode {Id = "m"};
         _diagramColors = new DiagramColors();
      }
   }

   public class When_initializing_an_educt_link : concern_for_ReactionLink
   {
      protected override void Because()
      {
         sut.Initialize(ReactionLinkType.Educt, _reactionNode, _moleculeNode);
      }

      [Observation]
      public void should_link_from_the_molecule_to_the_reaction()
      {
         sut.Type.ShouldBeEqualTo(ReactionLinkType.Educt);
         sut.GetFromNode().ShouldBeEqualTo(_moleculeNode);
         sut.GetToNode().ShouldBeEqualTo(_reactionNode);
      }

      [Observation]
      public void should_expose_the_reaction_and_molecule_nodes()
      {
         sut.ReactionNode.ShouldBeEqualTo(_reactionNode);
         sut.MoleculeNode.ShouldBeEqualTo(_moleculeNode);
      }

      [Observation]
      public void should_be_registered_in_both_nodes()
      {
         _moleculeNode.Links.ShouldOnlyContain(sut);
         _reactionNode.Links.ShouldOnlyContain(sut);
      }

      [Observation]
      public void should_return_the_other_node()
      {
         sut.GetOtherNode(_moleculeNode).ShouldBeEqualTo(_reactionNode);
         sut.GetOtherNode(_reactionNode).ShouldBeEqualTo(_moleculeNode);
         sut.GetOtherNode(new MoleculeNode()).ShouldBeNull();
      }

      [Observation]
      public void should_be_visible()
      {
         sut.IsVisible.ShouldBeTrue();
         sut.Visible.ShouldBeTrue();
      }
   }

   public class When_initializing_a_product_link : concern_for_ReactionLink
   {
      protected override void Because()
      {
         sut.Initialize(ReactionLinkType.Product, _reactionNode, _moleculeNode);
      }

      [Observation]
      public void should_link_from_the_reaction_to_the_molecule()
      {
         sut.Type.ShouldBeEqualTo(ReactionLinkType.Product);
         sut.GetFromNode().ShouldBeEqualTo(_reactionNode);
         sut.GetToNode().ShouldBeEqualTo(_moleculeNode);
      }

      [Observation]
      public void should_expose_the_reaction_and_molecule_nodes()
      {
         sut.ReactionNode.ShouldBeEqualTo(_reactionNode);
         sut.MoleculeNode.ShouldBeEqualTo(_moleculeNode);
      }

      [Observation]
      public void should_be_registered_in_both_nodes()
      {
         _moleculeNode.Links.ShouldOnlyContain(sut);
         _reactionNode.Links.ShouldOnlyContain(sut);
      }
   }

   public class When_initializing_a_modifier_link : concern_for_ReactionLink
   {
      protected override void Because()
      {
         sut.Initialize(ReactionLinkType.Modifier, _reactionNode, _moleculeNode);
      }

      [Observation]
      public void should_link_from_the_molecule_to_the_reaction()
      {
         sut.Type.ShouldBeEqualTo(ReactionLinkType.Modifier);
         sut.GetFromNode().ShouldBeEqualTo(_moleculeNode);
         sut.GetToNode().ShouldBeEqualTo(_reactionNode);
      }

      [Observation]
      public void should_expose_the_reaction_and_molecule_nodes()
      {
         sut.ReactionNode.ShouldBeEqualTo(_reactionNode);
         sut.MoleculeNode.ShouldBeEqualTo(_moleculeNode);
      }
   }

   public class When_setting_the_color_of_an_educt_link : concern_for_ReactionLink
   {
      protected override void Context()
      {
         base.Context();
         sut.Initialize(ReactionLinkType.Educt, _reactionNode, _moleculeNode);
      }

      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_use_the_educt_link_color_and_a_solid_line()
      {
         sut.Color.ShouldBeEqualTo(_diagramColors.ReactionLinkEduct);
         sut.IsDashed.ShouldBeFalse();
      }
   }

   public class When_setting_the_color_of_a_product_link : concern_for_ReactionLink
   {
      protected override void Context()
      {
         base.Context();
         sut.Initialize(ReactionLinkType.Product, _reactionNode, _moleculeNode);
      }

      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_use_the_product_link_color_and_a_solid_line()
      {
         sut.Color.ShouldBeEqualTo(_diagramColors.ReactionLinkProduct);
         sut.IsDashed.ShouldBeFalse();
      }
   }

   public class When_setting_the_color_of_a_modifier_link : concern_for_ReactionLink
   {
      protected override void Context()
      {
         base.Context();
         sut.Initialize(ReactionLinkType.Modifier, _reactionNode, _moleculeNode);
      }

      protected override void Because()
      {
         sut.SetColorFrom(_diagramColors);
      }

      [Observation]
      public void should_use_the_modifier_link_color_and_a_dashed_line()
      {
         sut.Color.ShouldBeEqualTo(_diagramColors.ReactionLinkModifier);
         sut.IsDashed.ShouldBeTrue();
      }
   }

   public class When_unlinking_a_reaction_link : concern_for_ReactionLink
   {
      protected override void Context()
      {
         base.Context();
         sut.Initialize(ReactionLinkType.Educt, _reactionNode, _moleculeNode);
      }

      protected override void Because()
      {
         sut.Unlink();
      }

      [Observation]
      public void should_remove_the_link_from_both_nodes()
      {
         _moleculeNode.Links.ShouldBeEmpty();
         _reactionNode.Links.ShouldBeEmpty();
         _moleculeNode.IsConnectedToReactions.ShouldBeFalse();
      }
   }

   public class When_checking_the_visibility_of_a_reaction_link : concern_for_ReactionLink
   {
      protected override void Context()
      {
         base.Context();
         sut.Initialize(ReactionLinkType.Product, _reactionNode, _moleculeNode);
      }

      [Observation]
      public void should_be_visible_when_both_nodes_are_visible()
      {
         _moleculeNode.Hidden = false;
         _reactionNode.IsVisible = true;
         sut.IsVisible = true;
         sut.Visible.ShouldBeTrue();
      }

      [Observation]
      public void should_not_be_visible_when_the_molecule_node_is_hidden()
      {
         _moleculeNode.Hidden = true;
         _reactionNode.IsVisible = true;
         sut.IsVisible = true;
         sut.Visible.ShouldBeFalse();
      }

      [Observation]
      public void should_not_be_visible_when_the_reaction_node_is_not_visible()
      {
         _moleculeNode.Hidden = false;
         _reactionNode.IsVisible = false;
         sut.IsVisible = true;
         sut.Visible.ShouldBeFalse();
      }

      [Observation]
      public void should_not_be_visible_when_the_link_itself_is_not_visible()
      {
         _moleculeNode.Hidden = false;
         _reactionNode.IsVisible = true;
         sut.Visible = false;
         sut.IsVisible.ShouldBeFalse();
         sut.Visible.ShouldBeFalse();
      }
   }
}
