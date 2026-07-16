using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_DiagramColors : ContextSpecification<DiagramColors>
   {
      protected override void Context()
      {
         sut = new DiagramColors();
      }
   }

   public class When_cloning_diagram_colors : concern_for_DiagramColors
   {
      private IDiagramColors _clone;

      protected override void Context()
      {
         base.Context();
         sut.DiagramBackground = Color.Red;
         sut.BorderFixed = Color.Blue;
         sut.ContainerOpacity = 0.5F;
         sut.MoleculeNode = Color.Green;
         sut.ReactionNode = Color.Yellow;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_create_a_new_instance()
      {
         _clone.ShouldNotBeNull();
         ReferenceEquals(_clone, sut).ShouldBeFalse();
      }

      [Observation]
      public void should_copy_all_color_properties()
      {
         _clone.DiagramBackground.ShouldBeEqualTo(Color.Red);
         _clone.BorderFixed.ShouldBeEqualTo(Color.Blue);
         _clone.ContainerOpacity.ShouldBeEqualTo(0.5F);
         _clone.MoleculeNode.ShouldBeEqualTo(Color.Green);
         _clone.ReactionNode.ShouldBeEqualTo(Color.Yellow);
      }
   }

   public class When_updating_diagram_colors_from_another_instance : concern_for_DiagramColors
   {
      private DiagramColors _source;

      protected override void Context()
      {
         base.Context();
         _source = new DiagramColors
         {
            DiagramBackground = Color.Black,
            BorderFixed = Color.Cyan,
            NodeSizeOpacity = 0.8F,
            ObserverNode = Color.Magenta,
            JournalPageNode = Color.Orange
         };
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_source);
      }

      [Observation]
      public void should_update_all_properties_from_source()
      {
         sut.DiagramBackground.ShouldBeEqualTo(Color.Black);
         sut.BorderFixed.ShouldBeEqualTo(Color.Cyan);
         sut.NodeSizeOpacity.ShouldBeEqualTo(0.8F);
         sut.ObserverNode.ShouldBeEqualTo(Color.Magenta);
         sut.JournalPageNode.ShouldBeEqualTo(Color.Orange);
      }
   }
}
