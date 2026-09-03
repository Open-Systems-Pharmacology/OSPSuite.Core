using System.Drawing;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Diagram.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class concern_for_LayeredReactionLayouter : ContextSpecification<LayeredReactionLayouter>
   {
      protected DiagramModel _model;

      protected override void Context()
      {
         sut = new LayeredReactionLayouter(node => new SizeF(node.Name.Length * 6F, 13F));
         _model = new DiagramModel();
      }

      protected MoleculeNode molecule(string name, float x, float y)
      {
         var node = _model.CreateNode<MoleculeNode>(name, new PointF(x, y), _model);
         node.Name = name;
         return node;
      }

      protected ReactionNode reaction(string name, float x, float y)
      {
         var node = _model.CreateNode<ReactionNode>(name, new PointF(x, y), _model);
         node.Name = name;
         return node;
      }

      protected static void link(ReactionLinkType type, ReactionNode reactionNode, MoleculeNode moleculeNode)
      {
         new ReactionLink().Initialize(type, reactionNode, moleculeNode);
      }
   }

   public class When_laying_out_a_diclofenac_like_reaction_network : concern_for_LayeredReactionLayouter
   {
      private MoleculeNode _cyp, _diclofenac, _unspecified, _hydroxy, _undefinedLiver, _unspecifiedMetabolite, _liverMetabolite;
      private ReactionNode _cypPaper, _unspecifiedPaper, _clearancePaper;

      protected override void Context()
      {
         base.Context();
         _cyp = molecule("CYP2C9", 10, 10);
         _diclofenac = molecule("Diclofenac", 10, 90);
         _unspecified = molecule("unspecified", 10, 150);
         _cypPaper = reaction("CYP2C9-Paper", 200, 50);
         _unspecifiedPaper = reaction("unspecified-Paper", 200, 150);
         _hydroxy = molecule("4'-Hydroxy-Diclofenac", 300, 50);
         _undefinedLiver = molecule("Undefined Liver", 300, 100);
         _unspecifiedMetabolite = molecule("Diclofenac-unspecified Metabolite", 300, 150);
         _clearancePaper = reaction("Total Hepatic Clearance-Paper", 500, 50);
         _liverMetabolite = molecule("4'-Hydroxy-Diclofenac-Undefined Liver Metabolite", 700, 50);

         link(ReactionLinkType.Modifier, _cypPaper, _cyp);
         link(ReactionLinkType.Educt, _cypPaper, _diclofenac);
         link(ReactionLinkType.Product, _cypPaper, _hydroxy);
         link(ReactionLinkType.Educt, _unspecifiedPaper, _diclofenac);
         link(ReactionLinkType.Educt, _unspecifiedPaper, _unspecified);
         link(ReactionLinkType.Product, _unspecifiedPaper, _unspecifiedMetabolite);
         link(ReactionLinkType.Educt, _clearancePaper, _hydroxy);
         link(ReactionLinkType.Modifier, _clearancePaper, _undefinedLiver);
         link(ReactionLinkType.Product, _clearancePaper, _liverMetabolite);
      }

      protected override void Because()
      {
         sut.Layout(_model);
      }

      [Observation]
      public void should_place_the_nodes_of_a_layer_at_the_same_horizontal_position()
      {
         _cyp.Location.X.ShouldBeEqualTo(_diclofenac.Location.X);
         _cyp.Location.X.ShouldBeEqualTo(_unspecified.Location.X);
         _hydroxy.Location.X.ShouldBeEqualTo(_undefinedLiver.Location.X);
         _hydroxy.Location.X.ShouldBeEqualTo(_unspecifiedMetabolite.Location.X);
      }

      [Observation]
      public void should_order_the_layers_from_left_to_right()
      {
         (_cyp.Location.X < _cypPaper.Location.X).ShouldBeTrue();
         (_cypPaper.Location.X < _hydroxy.Location.X).ShouldBeTrue();
         (_hydroxy.Location.X < _clearancePaper.Location.X).ShouldBeTrue();
         (_clearancePaper.Location.X < _liverMetabolite.Location.X).ShouldBeTrue();
      }

      [Observation]
      public void should_space_the_molecules_of_a_layer_evenly()
      {
         var firstLayer = new[] {_cyp, _diclofenac, _unspecified}.Select(x => x.Location.Y).OrderBy(y => y).ToList();
         var pitch = firstLayer[1] - firstLayer[0];
         (pitch > 0).ShouldBeTrue();
         (firstLayer[2] - firstLayer[1]).ShouldBeEqualTo(pitch);
      }

      [Observation]
      public void should_align_the_molecules_of_the_third_layer_with_the_rows_of_the_first_layer()
      {
         var firstLayerRows = new[] {_cyp, _diclofenac, _unspecified}.Select(x => x.Location.Y).ToList();
         new[] {_hydroxy, _undefinedLiver, _unspecifiedMetabolite}.Each(node => firstLayerRows.ShouldContain(node.Location.Y));
      }

      [Observation]
      public void should_place_the_modifier_above_the_educt_of_the_same_reaction()
      {
         (_undefinedLiver.Location.Y < _hydroxy.Location.Y).ShouldBeTrue();
         (_cyp.Location.Y < _diclofenac.Location.Y).ShouldBeTrue();
      }

      [Observation]
      public void should_place_the_reaction_between_its_connected_molecules()
      {
         (_clearancePaper.Location.Y > _undefinedLiver.Location.Y).ShouldBeTrue();
         (_clearancePaper.Location.Y < _hydroxy.Location.Y).ShouldBeTrue();
      }

      [Observation]
      public void should_start_the_diagram_at_the_margin()
      {
         var nodes = _model.GetAllChildren<ElementBaseNode>().ToList();
         nodes.Min(node => node.Bounds.Left).ShouldBeEqualTo(LayeredReactionLayouter.DIAGRAM_MARGIN);
         nodes.Min(node => node.Bounds.Top).ShouldBeEqualTo(LayeredReactionLayouter.DIAGRAM_MARGIN);
      }
   }

   public class When_laying_out_a_reaction_network_with_a_fixed_node : concern_for_LayeredReactionLayouter
   {
      private MoleculeNode _fixed, _educt, _product;
      private ReactionNode _reaction;

      protected override void Context()
      {
         base.Context();
         _fixed = molecule("Fixed", -500, 700);
         _fixed.LocationFixed = true;
         _educt = molecule("A", 0, 0);
         _reaction = reaction("R", 0, 0);
         _product = molecule("B", 0, 0);
         link(ReactionLinkType.Educt, _reaction, _educt);
         link(ReactionLinkType.Product, _reaction, _product);
         link(ReactionLinkType.Modifier, _reaction, _fixed);
      }

      protected override void Because()
      {
         sut.Layout(_model);
      }

      [Observation]
      public void should_not_move_the_fixed_node()
      {
         _fixed.Location.ShouldBeEqualTo(new PointF(-500, 700));
      }

      [Observation]
      public void should_lay_out_the_other_nodes_from_left_to_right()
      {
         (_educt.Location.X < _reaction.Location.X).ShouldBeTrue();
         (_reaction.Location.X < _product.Location.X).ShouldBeTrue();
      }
   }

   public class When_laying_out_a_cyclic_reaction_network : concern_for_LayeredReactionLayouter
   {
      private MoleculeNode _a, _b;
      private ReactionNode _forward, _backward;

      protected override void Context()
      {
         base.Context();
         _a = molecule("A", 0, 0);
         _b = molecule("B", 0, 0);
         _forward = reaction("forward", 0, 0);
         _backward = reaction("backward", 0, 0);
         link(ReactionLinkType.Educt, _forward, _a);
         link(ReactionLinkType.Product, _forward, _b);
         link(ReactionLinkType.Educt, _backward, _b);
         link(ReactionLinkType.Product, _backward, _a);
      }

      protected override void Because()
      {
         sut.Layout(_model);
      }

      [Observation]
      public void should_terminate_and_separate_all_nodes()
      {
         var locations = _model.GetAllChildren<ElementBaseNode>().Select(node => node.Location).ToList();
         locations.Distinct().Count().ShouldBeEqualTo(4);
      }
   }
}
