using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class DiagramOptions : IDiagramOptions
   {
      public DiagramOptions()
      {
         SnapGridVisible = false;
         MoleculePropertiesVisible = false;
         ObserverLinksVisible = false;
         UnusedMoleculesVisibleInModelDiagram = false;
         DefaultNodeSizeReaction =  NodeSize.Middle;
         DefaultNodeSizeMolecule = NodeSize.Middle;
         DefaultNodeSizeObserver = NodeSize.Middle;
         DiagramColors = new DiagramColors();
      }

      public bool SnapGridVisible { get; set; }
      public bool MoleculePropertiesVisible { get; set; }
      public bool ObserverLinksVisible { get; set; }
      public bool UnusedMoleculesVisibleInModelDiagram { get; set; }
      public NodeSize DefaultNodeSizeReaction { get; set; }
      public NodeSize DefaultNodeSizeMolecule { get; set; }
      public NodeSize DefaultNodeSizeObserver { get; set; }
      public IDiagramColors DiagramColors { get; set; }

      public IDiagramOptions Clone()
      {
         var clone = new DiagramOptions();
         clone.UpdatePropertiesFrom(this);
         return clone;
      }

      public void UpdatePropertiesFrom(IDiagramOptions source)
      {
         SnapGridVisible = source.SnapGridVisible;
         MoleculePropertiesVisible = source.MoleculePropertiesVisible;
         ObserverLinksVisible = source.ObserverLinksVisible;
         UnusedMoleculesVisibleInModelDiagram = source.UnusedMoleculesVisibleInModelDiagram;
         DefaultNodeSizeReaction = source.DefaultNodeSizeReaction;
         DefaultNodeSizeMolecule = source.DefaultNodeSizeMolecule;
         DefaultNodeSizeObserver = source.DefaultNodeSizeObserver;
         DiagramColors.UpdatePropertiesFrom(source.DiagramColors);
      }
   }
}