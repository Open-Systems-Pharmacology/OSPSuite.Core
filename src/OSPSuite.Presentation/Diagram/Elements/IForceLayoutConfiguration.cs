namespace OSPSuite.Presentation.Diagram.Elements
{
   public interface IForceLayoutConfiguration
   {
      float BaseGravitationalMass { get; set; }
      float BaseElectricalCharge { get; set; }
      float BaseSpringLength { get; set; }
      float BaseSpringStiffness { get; set; }
      float?[] RelativeGravitationalMassOf { get; set; }
      float?[] RelativeElectricalChargeOf { get; set; }
      float?[,] RelativeSpringLengthOf { get; set; }
      float?[,] RelativeSpringStiffnessOf { get; set; }
      int MaxIterations { get; set; }
      float Epsilon { get; set; }
      float InfinityDistance { get; set; }
      float ArrangementSpacingWidth { get; set; }
      float ArrangementSpacingHeight { get; set; }
      bool LogPositions { get; set; }
      float? RelativeElectricalChargeLinkless { get; set; }
      float? RelativeElectricalChargeRemote { get; set; }
      float? RelativeElectricalChargeContainer { get; set; }
      float? RelativeElectricalChargeNeighborhood { get; set; }
      float? RelativeElectricalChargeNeighborPort { get; set; }
      float? RelativeElectricalChargeMolecule { get; set; }
      float? RelativeElectricalChargeObserver { get; set; }
      float? RelativeElectricalChargeReaction { get; set; }
      float? RelativeGravitationalMassLinkless { get; set; }
      float? RelativeGravitationalMassRemote { get; set; }
      float? RelativeGravitationalMassContainer { get; set; }
      float? RelativeGravitationalMassNeighborhood { get; set; }
      float? RelativeGravitationalMassNeighborPort { get; set; }
      float? RelativeGravitationalMassMolecule { get; set; }
      float? RelativeGravitationalMassObserver { get; set; }
      float? RelativeGravitationalMassReaction { get; set; }
      float? RelativeSpringLengthContainerContainer { get; set; }
      float? RelativeSpringLengthContainerNeighborhood { get; set; }
      float? RelativeSpringLengthNeighborPortNeighborhood { get; set; }
      float? RelativeSpringLengthMoleculeNeighborPort { get; set; }
      float? RelativeSpringLengthObserverMolecule { get; set; }
      float? RelativeSpringLengthReactionMolecule { get; set; }
      float? RelativeSpringLengthContainerRemote { get; set; }
      float? RelativeSpringStiffnessContainerContainer { get; set; }
      float? RelativeSpringStiffnessContainerNeighborhood { get; set; }
      float? RelativeSpringStiffnessNeighborPortNeighborhood { get; set; }
      float? RelativeSpringStiffnessMoleculeNeighborPort { get; set; }
      float? RelativeSpringStiffnessObserverMolecule { get; set; }
      float? RelativeSpringStiffnessReactionMolecule { get; set; }
      float? RelativeSpringStiffnessContainerRemote { get; set; }
   }
}