using System.Collections.Generic;
using System.Drawing;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Diagram.Services;
using GoDiagramModel = OSPSuite.UI.Diagram.Elements.DiagramModel;
using GoReactionNode = OSPSuite.UI.Diagram.Elements.ReactionNode;

namespace OSPSuite.UI.Diagram
{
   public abstract class concern_for_DiagramLayoutTask : ContextSpecification<DiagramLayoutTask>
   {
      protected ILayerLayouter _layerLayouter;

      protected override void Context()
      {
         _layerLayouter = A.Fake<ILayerLayouter>();
         sut = new DiagramLayoutTask(_layerLayouter);
      }
   }

   public class When_laying_out_a_ui_free_reaction_diagram : concern_for_DiagramLayoutTask
   {
      private DiagramModel _model;
      private MoleculeNode _educt;
      private MoleculeNode _product;
      private ReactionNode _reaction;
      private int _changedCount;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel();
         _educt = _model.CreateNode<MoleculeNode>("A", PointF.Empty, _model);
         _product = _model.CreateNode<MoleculeNode>("B", PointF.Empty, _model);
         _reaction = _model.CreateNode<ReactionNode>("R", PointF.Empty, _model);
         _reaction.DisplayEductsRight = true;
         new ReactionLink().Initialize(ReactionLinkType.Educt, _reaction, _educt);
         new ReactionLink().Initialize(ReactionLinkType.Product, _reaction, _product);
         _changedCount = 0;
         _model.Changed += () => _changedCount++;
      }

      protected override void Because()
      {
         sut.LayoutReactionDiagram(_model);
      }

      [Observation]
      public void should_lay_out_the_nodes_from_left_to_right()
      {
         (_educt.Location.X < _reaction.Location.X).ShouldBeTrue();
         (_reaction.Location.X < _product.Location.X).ShouldBeTrue();
      }

      [Observation]
      public void should_reset_the_educt_display_side_of_the_reactions()
      {
         _reaction.DisplayEductsRight.ShouldBeFalse();
      }

      [Observation]
      public void should_mark_the_model_as_layouted()
      {
         _model.IsLayouted.ShouldBeTrue();
      }

      [Observation]
      public void should_notify_the_change_once()
      {
         _changedCount.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_not_use_the_go_layer_layouter()
      {
         A.CallTo(() => _layerLayouter.PerformLayout(A<IContainerBase>._, A<IList<IHasLayoutInfo>>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_allow_undoing_the_layout()
      {
         _model.Undo();
         _educt.Location.ShouldBeEqualTo(PointF.Empty);
         _reaction.Location.ShouldBeEqualTo(PointF.Empty);
         _reaction.DisplayEductsRight.ShouldBeTrue();
      }
   }

   public class When_laying_out_a_container_of_a_ui_free_reaction_diagram : concern_for_DiagramLayoutTask
   {
      private DiagramModel _model;
      private ContainerNode _container;
      private ReactionNode _reaction;

      protected override void Context()
      {
         base.Context();
         _model = new DiagramModel();
         _container = _model.CreateNode<ContainerNode>("C", PointF.Empty, _model);
         _reaction = _model.CreateNode<ReactionNode>("R", PointF.Empty, _container);
         _reaction.DisplayEductsRight = true;
      }

      protected override void Because()
      {
         sut.LayoutReactionDiagram(_container);
      }

      [Observation]
      public void should_treat_the_container_as_part_of_the_ui_free_model()
      {
         _reaction.DisplayEductsRight.ShouldBeFalse();
         _model.IsLayouted.ShouldBeTrue();
         A.CallTo(() => _layerLayouter.PerformLayout(A<IContainerBase>._, A<IList<IHasLayoutInfo>>._)).MustNotHaveHappened();
      }
   }

   public class When_laying_out_a_go_diagram : concern_for_DiagramLayoutTask
   {
      private GoDiagramModel _model;
      private GoReactionNode _reaction;

      protected override void Context()
      {
         base.Context();
         _model = new GoDiagramModel();
         _reaction = _model.CreateNode<GoReactionNode>("R", PointF.Empty, _model);
         _reaction.DisplayEductsRight = true;
      }

      protected override void Because()
      {
         sut.LayoutReactionDiagram(_model);
      }

      [Observation]
      public void should_use_the_go_layer_layouter()
      {
         A.CallTo(() => _layerLayouter.PerformLayout(_model, null)).MustHaveHappened();
      }

      [Observation]
      public void should_reset_the_educt_display_side_and_mark_the_model_as_layouted()
      {
         _reaction.DisplayEductsRight.ShouldBeFalse();
         _model.IsLayouted.ShouldBeTrue();
      }
   }
}
