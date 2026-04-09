using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_DiagramOptions : ContextSpecification<DiagramOptions>
   {
      protected override void Context()
      {
         sut = new DiagramOptions();
      }
   }

   public class When_cloning_diagram_options : concern_for_DiagramOptions
   {
      private IDiagramOptions _clone;

      protected override void Context()
      {
         base.Context();
         sut.SnapGridVisible = true;
         sut.MoleculePropertiesVisible = true;
         sut.ObserverLinksVisible = true;
         sut.UnusedMoleculesVisibleInModelDiagram = true;
         sut.DefaultNodeSizeReaction = NodeSize.Large;
         sut.DefaultNodeSizeMolecule = NodeSize.Small;
         sut.DefaultNodeSizeObserver = NodeSize.Large;
         sut.DiagramColors.MoleculeNode = Color.Red;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_create_a_new_instance()
      {
         ReferenceEquals(_clone, sut).ShouldBeFalse();
      }

      [Observation]
      public void should_copy_all_properties()
      {
         _clone.SnapGridVisible.ShouldBeTrue();
         _clone.MoleculePropertiesVisible.ShouldBeTrue();
         _clone.ObserverLinksVisible.ShouldBeTrue();
         _clone.UnusedMoleculesVisibleInModelDiagram.ShouldBeTrue();
         _clone.DefaultNodeSizeReaction.ShouldBeEqualTo(NodeSize.Large);
         _clone.DefaultNodeSizeMolecule.ShouldBeEqualTo(NodeSize.Small);
         _clone.DefaultNodeSizeObserver.ShouldBeEqualTo(NodeSize.Large);
      }

      [Observation]
      public void should_deep_clone_diagram_colors()
      {
         ReferenceEquals(_clone.DiagramColors, sut.DiagramColors).ShouldBeFalse();
         _clone.DiagramColors.MoleculeNode.ShouldBeEqualTo(Color.Red);
      }
   }

   public class When_updating_diagram_options_from_another_instance : concern_for_DiagramOptions
   {
      private DiagramOptions _source;

      protected override void Context()
      {
         base.Context();
         _source = new DiagramOptions
         {
            SnapGridVisible = true,
            MoleculePropertiesVisible = true,
            DefaultNodeSizeReaction = NodeSize.Small,
            DefaultNodeSizeMolecule = NodeSize.Large
         };
         _source.DiagramColors.MoleculeNode = Color.Blue;
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_source);
      }

      [Observation]
      public void should_update_all_properties()
      {
         sut.SnapGridVisible.ShouldBeTrue();
         sut.MoleculePropertiesVisible.ShouldBeTrue();
         sut.DefaultNodeSizeReaction.ShouldBeEqualTo(NodeSize.Small);
         sut.DefaultNodeSizeMolecule.ShouldBeEqualTo(NodeSize.Large);
      }

      [Observation]
      public void should_update_diagram_colors()
      {
         sut.DiagramColors.MoleculeNode.ShouldBeEqualTo(Color.Blue);
      }
   }
}
