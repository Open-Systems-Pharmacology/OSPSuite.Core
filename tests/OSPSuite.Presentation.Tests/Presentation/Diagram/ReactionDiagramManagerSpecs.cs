using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram
{
   public class ReactionDiagramForSpecs : IWithDiagramFor<ReactionDiagramForSpecs>, IEnumerable<ReactionBuilder>
   {
      public ReactionBuildingBlock ReactionBuildingBlock { get; } = new ReactionBuildingBlock();
      public IDiagramModel DiagramModel { get; set; } = new DiagramModel();
      public IDiagramManager<ReactionDiagramForSpecs> DiagramManager { get; set; }

      public IEnumerator<ReactionBuilder> GetEnumerator() => ReactionBuildingBlock.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
   }

   public abstract class concern_for_ReactionDiagramManager : ContextSpecification<ReactionDiagramManager<ReactionDiagramForSpecs>>
   {
      protected ReactionDiagramForSpecs _reactionDiagram;
      protected ReactionBuildingBlock _reactionBuildingBlock;
      protected IDiagramModel _diagramModel;
      protected DiagramOptions _diagramOptions;
      protected ReactionBuilder _r1;
      protected ReactionBuilder _r2;

      protected override void Context()
      {
         sut = new ReactionDiagramManager<ReactionDiagramForSpecs>();
         _reactionDiagram = new ReactionDiagramForSpecs();
         _reactionBuildingBlock = _reactionDiagram.ReactionBuildingBlock;
         _diagramModel = _reactionDiagram.DiagramModel;
         _diagramOptions = new DiagramOptions();
         _r1 = createReaction("R1", educts: new[] {"A"}, products: new[] {"B"}, modifiers: new[] {"C"});
         _r2 = createReaction("R2", educts: new[] {"B"}, products: new[] {"D"});
         sut.InitializeWith(_reactionDiagram, _diagramOptions);
      }

      protected ReactionBuilder createReaction(string name, string[] educts = null, string[] products = null, string[] modifiers = null)
      {
         var reactionBuilder = new ReactionBuilder().WithId(name).WithName(name);
         (educts ?? new string[0]).Each(x => reactionBuilder.AddEduct(new ReactionPartnerBuilder(x, 1)));
         (products ?? new string[0]).Each(x => reactionBuilder.AddProduct(new ReactionPartnerBuilder(x, 1)));
         (modifiers ?? new string[0]).Each(reactionBuilder.AddModifier);
         _reactionBuildingBlock.Add(reactionBuilder);
         return reactionBuilder;
      }

      protected ReactionNode reactionNodeFor(ReactionBuilder reactionBuilder) => sut.ReactionNodeFor(reactionBuilder) as ReactionNode;

      protected MoleculeNode moleculeNode(string name) => sut.GetMoleculeNodes(name).Single() as MoleculeNode;

      protected ReactionLink linkFor(ReactionBuilder reactionBuilder, ReactionLinkType type, string moleculeName)
      {
         return reactionNodeFor(reactionBuilder).ReactionLinks.SingleOrDefault(x => x.Type == type && x.MoleculeNode.Name == moleculeName);
      }
   }

   public class When_initializing_the_reaction_diagram_manager : concern_for_ReactionDiagramManager
   {
      [Observation]
      public void should_be_initialized_with_the_model_and_the_options()
      {
         sut.IsInitialized.ShouldBeTrue();
         sut.PkModel.ShouldBeEqualTo(_reactionDiagram);
         sut.DiagramOptions.ShouldBeEqualTo(_diagramOptions);
         _diagramModel.DiagramOptions.ShouldBeEqualTo(_diagramOptions);
      }

      [Observation]
      public void should_create_one_reaction_node_per_reaction()
      {
         _diagramModel.GetAllChildren<ReactionNode>().Count().ShouldBeEqualTo(2);
         reactionNodeFor(_r1).Id.ShouldBeEqualTo("R1");
         reactionNodeFor(_r1).Name.ShouldBeEqualTo("R1");
         reactionNodeFor(_r1).Description.ShouldBeEqualTo("R1");
         reactionNodeFor(_r1).GetParent().ShouldBeEqualTo(_diagramModel);
         reactionNodeFor(_r2).Id.ShouldBeEqualTo("R2");
         sut.MustHandleExisting("R1").ShouldBeTrue();
         sut.MustHandleExisting("unknown").ShouldBeFalse();
      }

      [Observation]
      public void should_create_one_molecule_node_per_distinct_molecule_name()
      {
         sut.GetMoleculeNodes().Select(x => x.Name).ShouldOnlyContain("A", "B", "C", "D");
         sut.GetMoleculeNodes("B").Count().ShouldBeEqualTo(1);
         sut.GetMoleculeNodes("unknown").ShouldBeEmpty();
         moleculeNode("A").Description.ShouldBeEqualTo("A");
      }

      [Observation]
      public void should_apply_the_default_node_sizes()
      {
         moleculeNode("A").NodeSize.ShouldBeEqualTo(_diagramOptions.DefaultNodeSizeMolecule);
         reactionNodeFor(_r1).NodeSize.ShouldBeEqualTo(_diagramOptions.DefaultNodeSizeReaction);
      }

      [Observation]
      public void should_link_educts_products_and_modifiers_with_typed_links()
      {
         reactionNodeFor(_r1).ReactionLinks.Count().ShouldBeEqualTo(3);
         reactionNodeFor(_r2).ReactionLinks.Count().ShouldBeEqualTo(2);

         var eductLink = linkFor(_r1, ReactionLinkType.Educt, "A");
         eductLink.ShouldNotBeNull();
         eductLink.Color.ShouldBeEqualTo(_diagramOptions.DiagramColors.ReactionLinkEduct);
         eductLink.IsDashed.ShouldBeFalse();

         var productLink = linkFor(_r1, ReactionLinkType.Product, "B");
         productLink.ShouldNotBeNull();
         productLink.Color.ShouldBeEqualTo(_diagramOptions.DiagramColors.ReactionLinkProduct);
         productLink.IsDashed.ShouldBeFalse();

         var modifierLink = linkFor(_r1, ReactionLinkType.Modifier, "C");
         modifierLink.ShouldNotBeNull();
         modifierLink.Color.ShouldBeEqualTo(_diagramOptions.DiagramColors.ReactionLinkModifier);
         modifierLink.IsDashed.ShouldBeTrue();
      }

      [Observation]
      public void should_link_a_shared_molecule_to_all_its_reactions()
      {
         moleculeNode("B").GetLinkedNodes<ReactionNode>().ShouldOnlyContain(reactionNodeFor(_r1), reactionNodeFor(_r2));
         moleculeNode("B").IsConnectedToReactions.ShouldBeTrue();
      }

      [Observation]
      public void should_color_the_nodes()
      {
         reactionNodeFor(_r1).FillColor.ShouldBeEqualTo(Color.FromArgb(64, _diagramOptions.DiagramColors.ReactionNode));
         moleculeNode("A").FillColor.ShouldBeEqualTo(Color.FromArgb(64, _diagramOptions.DiagramColors.MoleculeNode));
      }
   }

   public class When_a_reaction_is_renamed : concern_for_ReactionDiagramManager
   {
      protected override void Because()
      {
         _r1.Name = "Renamed";
      }

      [Observation]
      public void should_update_the_reaction_node()
      {
         reactionNodeFor(_r1).Name.ShouldBeEqualTo("Renamed");
         reactionNodeFor(_r1).Description.ShouldBeEqualTo("Renamed");
      }
   }

   public class When_a_molecule_is_added_to_a_reaction : concern_for_ReactionDiagramManager
   {
      protected override void Because()
      {
         _r1.AddModifier("E");
         sut.AddMolecule(_r1, "E");
      }

      [Observation]
      public void should_create_a_single_node_for_the_new_molecule()
      {
         sut.GetMoleculeNodes("E").Count().ShouldBeEqualTo(1);
         moleculeNode("E").Description.ShouldBeEqualTo("E");
      }

      [Observation]
      public void should_link_the_new_molecule_to_the_reaction()
      {
         linkFor(_r1, ReactionLinkType.Modifier, "E").ShouldNotBeNull();
         reactionNodeFor(_r1).ReactionLinks.Count().ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_bring_the_new_node_to_front()
      {
         _diagramModel.GetDirectChildren<IBaseNode>().Last().ShouldBeEqualTo(moleculeNode("E"));
      }
   }

   public class When_a_molecule_is_removed_from_a_reaction : concern_for_ReactionDiagramManager
   {
      protected override void Because()
      {
         _r1.RemoveModifier("C");
         sut.RemoveMolecule(_r1, "C");
      }

      [Observation]
      public void should_unlink_the_molecule_from_the_reaction()
      {
         linkFor(_r1, ReactionLinkType.Modifier, "C").ShouldBeNull();
         reactionNodeFor(_r1).ReactionLinks.Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_keep_the_unconnected_molecule_node()
      {
         moleculeNode("C").IsConnectedToReactions.ShouldBeFalse();
      }
   }

   public class When_a_molecule_is_renamed_in_its_only_reaction : concern_for_ReactionDiagramManager
   {
      private string _oldNodeId;

      protected override void Context()
      {
         base.Context();
         moleculeNode("A").Location = new PointF(77, 88);
         _oldNodeId = moleculeNode("A").Id;
      }

      protected override void Because()
      {
         _r1.EductBy("A").MoleculeName = "A2";
         sut.RenameMolecule(_r1, "A", "A2");
      }

      [Observation]
      public void should_remove_the_old_node_that_is_no_longer_connected()
      {
         sut.GetMoleculeNodes("A").ShouldBeEmpty();
         _diagramModel.GetNode(_oldNodeId).ShouldBeNull();
      }

      [Observation]
      public void should_create_the_new_node_at_the_old_location()
      {
         moleculeNode("A2").Location.ShouldBeEqualTo(new PointF(77, 88));
      }

      [Observation]
      public void should_link_the_new_node_to_the_reaction()
      {
         linkFor(_r1, ReactionLinkType.Educt, "A2").ShouldNotBeNull();
         reactionNodeFor(_r1).ReactionLinks.Count().ShouldBeEqualTo(3);
      }
   }

   public class When_a_molecule_used_by_another_reaction_is_renamed : concern_for_ReactionDiagramManager
   {
      private MoleculeNode _oldNode;

      protected override void Context()
      {
         base.Context();
         _oldNode = moleculeNode("B");
         _oldNode.Location = new PointF(77, 88);
      }

      protected override void Because()
      {
         _r1.ProductBy("B").MoleculeName = "B2";
         sut.RenameMolecule(_r1, "B", "B2");
      }

      [Observation]
      public void should_keep_the_old_node_connected_to_the_other_reaction()
      {
         moleculeNode("B").ShouldBeEqualTo(_oldNode);
         _oldNode.GetLinkedNodes<ReactionNode>().ShouldOnlyContain(reactionNodeFor(_r2));
      }

      [Observation]
      public void should_move_the_old_node_away_from_the_new_node()
      {
         _oldNode.Location.ShouldBeEqualTo(new PointF(77, 88).Plus(Assets.Diagram.Reaction.OldMoleculeNodeOffsetInRename));
         moleculeNode("B2").Location.ShouldBeEqualTo(new PointF(77, 88));
      }

      [Observation]
      public void should_link_the_new_node_to_the_renaming_reaction()
      {
         linkFor(_r1, ReactionLinkType.Product, "B2").ShouldNotBeNull();
         linkFor(_r1, ReactionLinkType.Product, "B").ShouldBeNull();
      }
   }

   public class When_renaming_an_unknown_molecule : concern_for_ReactionDiagramManager
   {
      protected override void Because()
      {
         sut.RenameMolecule(_r1, "X", "Y");
      }

      [Observation]
      public void should_not_change_the_diagram()
      {
         sut.GetMoleculeNodes("Y").ShouldBeEmpty();
         sut.GetMoleculeNodes().Count().ShouldBeEqualTo(4);
      }
   }

   public abstract class concern_for_ReactionDiagramManager_with_twin : concern_for_ReactionDiagramManager
   {
      protected MoleculeNode _originalNode;
      protected IMoleculeNode _twin;

      protected override void Context()
      {
         base.Context();
         reactionNodeFor(_r1).Location = new PointF(0, 0);
         reactionNodeFor(_r2).Location = new PointF(500, 500);
         _originalNode = moleculeNode("B");
         _originalNode.Location = new PointF(0, 50);
         sut.CurrentInsertLocation = new PointF(600, 600);
      }
   }

   public class When_adding_a_twin_molecule_node : concern_for_ReactionDiagramManager_with_twin
   {
      protected override void Because()
      {
         _twin = sut.AddMoleculeNode("B");
      }

      [Observation]
      public void should_create_a_second_node_with_the_same_name()
      {
         sut.GetMoleculeNodes("B").ShouldOnlyContain(_originalNode, _twin);
         _twin.Name.ShouldBeEqualTo("B");
         _twin.Description.ShouldBeEqualTo("B");
         _twin.ShouldBeAnInstanceOf<MoleculeNode>();
      }

      [Observation]
      public void should_place_the_twin_at_the_next_insert_location()
      {
         _twin.Location.ShouldBeEqualTo(new PointF(600, 600));
         _twin.Location.ShouldBeEqualTo(sut.CurrentInsertLocation);
      }

      [Observation]
      public void should_relink_each_reaction_to_the_nearest_twin()
      {
         linkFor(_r1, ReactionLinkType.Product, "B").MoleculeNode.ShouldBeEqualTo(_originalNode);
         linkFor(_r2, ReactionLinkType.Educt, "B").MoleculeNode.ShouldBeEqualTo(_twin);
         _originalNode.Links.Count.ShouldBeEqualTo(1);
         ((MoleculeNode) _twin).Links.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_color_the_twin()
      {
         ((MoleculeNode) _twin).FillColor.ShouldBeEqualTo(_originalNode.FillColor);
      }
   }

   public class When_removing_a_twin_molecule_node : concern_for_ReactionDiagramManager_with_twin
   {
      protected override void Context()
      {
         base.Context();
         _twin = sut.AddMoleculeNode("B");
      }

      protected override void Because()
      {
         sut.RemoveMoleculeNode(_twin);
      }

      [Observation]
      public void should_remove_the_twin_from_the_diagram()
      {
         sut.GetMoleculeNodes("B").ShouldOnlyContain(_originalNode);
         _diagramModel.GetNode(_twin.Id).ShouldBeNull();
      }

      [Observation]
      public void should_relink_the_reaction_to_the_remaining_twin()
      {
         linkFor(_r2, ReactionLinkType.Educt, "B").MoleculeNode.ShouldBeEqualTo(_originalNode);
         _originalNode.GetLinkedNodes<ReactionNode>().ShouldOnlyContain(reactionNodeFor(_r1), reactionNodeFor(_r2));
      }
   }

   public class When_refreshing_the_diagram_after_changing_the_building_block : concern_for_ReactionDiagramManager
   {
      private ReactionBuilder _r3;

      protected override void Because()
      {
         _r3 = createReaction("R3", educts: new[] {"D"}, products: new[] {"E"});
         _reactionBuildingBlock.Remove(_r2);
         sut.RefreshDiagramFromModel();
      }

      [Observation]
      public void should_add_a_node_for_the_new_reaction()
      {
         reactionNodeFor(_r3).ShouldNotBeNull();
         linkFor(_r3, ReactionLinkType.Educt, "D").ShouldNotBeNull();
         linkFor(_r3, ReactionLinkType.Product, "E").ShouldNotBeNull();
         sut.GetMoleculeNodes("E").Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_remove_the_node_of_the_removed_reaction()
      {
         sut.ReactionNodeFor(_r2).ShouldBeNull();
         _diagramModel.GetNode("R2").ShouldBeNull();
         moleculeNode("D").GetLinkedNodes<ReactionNode>().ShouldOnlyContain(reactionNodeFor(_r3));
      }

      [Observation]
      public void should_couple_the_new_reaction_to_its_node()
      {
         _r3.Name = "Renamed";
         reactionNodeFor(_r3).Name.ShouldBeEqualTo("Renamed");
      }
   }

   public class When_adding_a_reaction_as_object_base : concern_for_ReactionDiagramManager
   {
      private ReactionBuilder _r3;
      private ReactionBuilder _foreignReaction;

      protected override void Because()
      {
         _r3 = createReaction("R3", educts: new[] {"A"}, modifiers: new[] {"F"});
         sut.AddObjectBase(_r3);
         _foreignReaction = new ReactionBuilder().WithId("foreign").WithName("foreign");
         sut.AddObjectBase(_foreignReaction);
      }

      [Observation]
      public void should_create_and_link_the_node_of_a_reaction_in_the_building_block()
      {
         reactionNodeFor(_r3).Name.ShouldBeEqualTo("R3");
         linkFor(_r3, ReactionLinkType.Educt, "A").MoleculeNode.ShouldBeEqualTo(moleculeNode("A"));
         linkFor(_r3, ReactionLinkType.Modifier, "F").ShouldNotBeNull();
         moleculeNode("A").GetLinkedNodes<ReactionNode>().ShouldOnlyContain(reactionNodeFor(_r1), reactionNodeFor(_r3));
      }

      [Observation]
      public void should_ignore_reactions_that_are_not_part_of_the_building_block()
      {
         _diagramModel.GetNode("foreign").ShouldBeNull();
      }
   }

   public class When_removing_a_reaction_as_object_base : concern_for_ReactionDiagramManager
   {
      protected override void Because()
      {
         _reactionBuildingBlock.Remove(_r1);
         sut.RemoveObjectBase(_r1);
      }

      [Observation]
      public void should_remove_the_reaction_node_and_its_links()
      {
         _diagramModel.GetNode("R1").ShouldBeNull();
         moleculeNode("A").IsConnectedToReactions.ShouldBeFalse();
         moleculeNode("C").IsConnectedToReactions.ShouldBeFalse();
         moleculeNode("B").GetLinkedNodes<ReactionNode>().ShouldOnlyContain(reactionNodeFor(_r2));
      }

      [Observation]
      public void should_keep_the_molecule_nodes()
      {
         sut.GetMoleculeNodes().Count().ShouldBeEqualTo(4);
      }
   }

   public class When_cleaning_up_the_reaction_diagram_manager : concern_for_ReactionDiagramManager
   {
      protected override void Because()
      {
         sut.Cleanup();
         _r1.Name = "Renamed";
      }

      [Observation]
      public void should_decouple_the_reactions_from_their_nodes()
      {
         _diagramModel.GetNode<ReactionNode>("R1").Name.ShouldBeEqualTo("R1");
      }

      [Observation]
      public void should_forget_the_model()
      {
         sut.PkModel.ShouldBeNull();
      }
   }

   public class When_creating_a_new_reaction_diagram_manager : concern_for_ReactionDiagramManager
   {
      private IDiagramManager<ReactionDiagramForSpecs> _newManager;

      protected override void Because()
      {
         _newManager = sut.Create();
      }

      [Observation]
      public void should_return_a_new_uninitialized_manager()
      {
         _newManager.ShouldBeAnInstanceOf<ReactionDiagramManager<ReactionDiagramForSpecs>>();
         ReferenceEquals(_newManager, sut).ShouldBeFalse();
         _newManager.IsInitialized.ShouldBeFalse();
         _newManager.PkModel.ShouldBeNull();
      }
   }
}
