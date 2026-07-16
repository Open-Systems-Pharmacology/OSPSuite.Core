namespace OSPSuite.Core.Diagram
{
   public interface IDiagramOptions
   {
      IDiagramOptions Clone();
      void UpdatePropertiesFrom(IDiagramOptions source);

      bool SnapGridVisible { get; set; }
      bool MoleculePropertiesVisible { get; set; }
      bool ObserverLinksVisible { get; set; }
      bool UnusedMoleculesVisibleInModelDiagram { get; set; }
      NodeSize DefaultNodeSizeReaction { get; set; }
      NodeSize DefaultNodeSizeMolecule { get; set; }
      NodeSize DefaultNodeSizeObserver { get; set; }
      IDiagramColors DiagramColors { get; set; }
   }
}