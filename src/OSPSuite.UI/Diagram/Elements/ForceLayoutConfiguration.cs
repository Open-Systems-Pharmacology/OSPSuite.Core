using OSPSuite.Presentation.Diagram.Elements;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ForceLayoutConfiguration : IForceLayoutConfiguration
   {
      public const int NUMBER_OF_GROUPS = 10;

      public float BaseGravitationalMass { get; set; }
      public float BaseElectricalCharge { get; set; }
      public float BaseSpringLength { get; set; }
      public float BaseSpringStiffness { get; set; }

      public float?[] RelativeGravitationalMassOf { get; set; }
      public float?[] RelativeElectricalChargeOf { get; set; }
      public float?[,] RelativeSpringLengthOf { get; set; }
      public float?[,] RelativeSpringStiffnessOf { get; set; }

      public int MaxIterations { get; set; }
      public float Epsilon { get; set; }
      public float InfinityDistance { get; set; }
      public float ArrangementSpacingWidth { get; set; }
      public float ArrangementSpacingHeight { get; set; }

      public bool LogPositions { get; set; }

      public float? RelativeElectricalChargeLinkless
      {
         get { return RelativeElectricalChargeOf[NodeLayoutType.LINKLESS_NODE]; }
         set { RelativeElectricalChargeOf[NodeLayoutType.LINKLESS_NODE] = value; }
      }
      public float? RelativeElectricalChargeRemote
      {
         get { return RelativeElectricalChargeOf[NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE]; }
         set { RelativeElectricalChargeOf[NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE] = value; }
      }
      public float? RelativeElectricalChargeContainer
      {
         get { return RelativeElectricalChargeOf[NodeLayoutType.CONTAINER_NODE]; }
         set { RelativeElectricalChargeOf[NodeLayoutType.CONTAINER_NODE] = value; }
      }
      public float? RelativeElectricalChargeNeighborhood
      {
         get { return RelativeElectricalChargeOf[NodeLayoutType.NEIGHBORHOOD_NODE]; }
         set { RelativeElectricalChargeOf[NodeLayoutType.NEIGHBORHOOD_NODE] = value; }
      }
      public float? RelativeElectricalChargeNeighborPort
      {
         get { return RelativeElectricalChargeOf[NodeLayoutType.NEIGHBORHOOD_PORT]; }
         set { RelativeElectricalChargeOf[NodeLayoutType.NEIGHBORHOOD_PORT] = value; }
      }
      public float? RelativeElectricalChargeMolecule
      {
         get { return RelativeElectricalChargeOf[NodeLayoutType.MOLECULE_NODE]; }
         set { RelativeElectricalChargeOf[NodeLayoutType.MOLECULE_NODE] = value; }
      }
      public float? RelativeElectricalChargeObserver
      {
         get { return RelativeElectricalChargeOf[NodeLayoutType.OBSERVER_NODE]; }
         set { RelativeElectricalChargeOf[NodeLayoutType.OBSERVER_NODE] = value; }
      }
      public float? RelativeElectricalChargeReaction
      {
         get { return RelativeElectricalChargeOf[NodeLayoutType.REACTION_NODE]; }
         set { RelativeElectricalChargeOf[NodeLayoutType.REACTION_NODE] = value; }
      }

      public float? RelativeGravitationalMassLinkless
      {
         get { return RelativeGravitationalMassOf[NodeLayoutType.LINKLESS_NODE]; }
         set { RelativeGravitationalMassOf[NodeLayoutType.LINKLESS_NODE] = value; }
      }
      public float? RelativeGravitationalMassRemote
      {
         get { return RelativeGravitationalMassOf[NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE]; }
         set { RelativeGravitationalMassOf[NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE] = value; }
      }
      public float? RelativeGravitationalMassContainer
      {
         get { return RelativeGravitationalMassOf[NodeLayoutType.CONTAINER_NODE]; }
         set { RelativeGravitationalMassOf[NodeLayoutType.CONTAINER_NODE] = value; }
      }
      public float? RelativeGravitationalMassNeighborhood
      {
         get { return RelativeGravitationalMassOf[NodeLayoutType.NEIGHBORHOOD_NODE]; }
         set { RelativeGravitationalMassOf[NodeLayoutType.NEIGHBORHOOD_NODE] = value; }
      }
      public float? RelativeGravitationalMassNeighborPort
      {
         get { return RelativeGravitationalMassOf[NodeLayoutType.NEIGHBORHOOD_PORT]; }
         set { RelativeGravitationalMassOf[NodeLayoutType.NEIGHBORHOOD_PORT] = value; }
      }
      public float? RelativeGravitationalMassMolecule
      {
         get { return RelativeGravitationalMassOf[NodeLayoutType.MOLECULE_NODE]; }
         set { RelativeGravitationalMassOf[NodeLayoutType.MOLECULE_NODE] = value; }
      }
      public float? RelativeGravitationalMassObserver
      {
         get { return RelativeGravitationalMassOf[NodeLayoutType.OBSERVER_NODE]; }
         set { RelativeGravitationalMassOf[NodeLayoutType.OBSERVER_NODE] = value; }
      }
      public float? RelativeGravitationalMassReaction
      {
         get { return RelativeGravitationalMassOf[NodeLayoutType.REACTION_NODE]; }
         set { RelativeGravitationalMassOf[NodeLayoutType.REACTION_NODE] = value; }
      }

      public float? RelativeSpringLengthContainerContainer
      {
         get { return RelativeSpringLengthOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.CONTAINER_NODE]; }
         set { RelativeSpringLengthOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.CONTAINER_NODE] = value; }
      }
      public float? RelativeSpringLengthContainerNeighborhood
      {
         get { return RelativeSpringLengthOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.NEIGHBORHOOD_NODE]; }
         set
         {
            RelativeSpringLengthOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.NEIGHBORHOOD_NODE] = value;
            RelativeSpringLengthOf[NodeLayoutType.NEIGHBORHOOD_NODE, NodeLayoutType.CONTAINER_NODE] = value;
         }
      }
      public float? RelativeSpringLengthNeighborPortNeighborhood
      {
         get { return RelativeSpringLengthOf[NodeLayoutType.NEIGHBORHOOD_PORT, NodeLayoutType.NEIGHBORHOOD_NODE]; }
         set
         {
            RelativeSpringLengthOf[NodeLayoutType.NEIGHBORHOOD_NODE, NodeLayoutType.NEIGHBORHOOD_PORT] = value;
            RelativeSpringLengthOf[NodeLayoutType.NEIGHBORHOOD_PORT, NodeLayoutType.NEIGHBORHOOD_NODE] = value;
         }
      }
      public float? RelativeSpringLengthMoleculeNeighborPort
      {
         get { return RelativeSpringLengthOf[NodeLayoutType.MOLECULE_NODE, NodeLayoutType.NEIGHBORHOOD_PORT]; }
         set
         {
            RelativeSpringLengthOf[NodeLayoutType.NEIGHBORHOOD_PORT, NodeLayoutType.MOLECULE_NODE] = value;
            RelativeSpringLengthOf[NodeLayoutType.MOLECULE_NODE, NodeLayoutType.NEIGHBORHOOD_PORT] = value;
         }
      }
      public float? RelativeSpringLengthObserverMolecule
      {
         get { return RelativeSpringLengthOf[NodeLayoutType.OBSERVER_NODE, NodeLayoutType.MOLECULE_NODE]; }
         set
         {
            RelativeSpringLengthOf[NodeLayoutType.MOLECULE_NODE, NodeLayoutType.OBSERVER_NODE] = value;
            RelativeSpringLengthOf[NodeLayoutType.OBSERVER_NODE, NodeLayoutType.MOLECULE_NODE] = value;
         }
      }
      public float? RelativeSpringLengthReactionMolecule
      {
         get { return RelativeSpringLengthOf[NodeLayoutType.REACTION_NODE, NodeLayoutType.MOLECULE_NODE]; }
         set
         {
            RelativeSpringLengthOf[NodeLayoutType.MOLECULE_NODE, NodeLayoutType.REACTION_NODE] = value;
            RelativeSpringLengthOf[NodeLayoutType.REACTION_NODE, NodeLayoutType.MOLECULE_NODE] = value;
         }
      }
      public float? RelativeSpringLengthContainerRemote
      {
         get { return RelativeSpringLengthOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE]; }
         set
         {
            RelativeSpringLengthOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE] = value;
            RelativeSpringLengthOf[NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE, NodeLayoutType.CONTAINER_NODE] = value;
         }
      }

      public float? RelativeSpringStiffnessContainerContainer
      {
         get { return RelativeSpringStiffnessOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.CONTAINER_NODE]; }
         set { RelativeSpringStiffnessOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.CONTAINER_NODE] = value; }
      }
      public float? RelativeSpringStiffnessContainerNeighborhood
      {
         get { return RelativeSpringStiffnessOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.NEIGHBORHOOD_NODE]; }
         set
         {
            RelativeSpringStiffnessOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.NEIGHBORHOOD_NODE] = value;
            RelativeSpringStiffnessOf[NodeLayoutType.NEIGHBORHOOD_NODE, NodeLayoutType.CONTAINER_NODE] = value;
         }
      }
      public float? RelativeSpringStiffnessNeighborPortNeighborhood
      {
         get { return RelativeSpringStiffnessOf[NodeLayoutType.NEIGHBORHOOD_PORT, NodeLayoutType.NEIGHBORHOOD_NODE]; }
         set
         {
            RelativeSpringStiffnessOf[NodeLayoutType.NEIGHBORHOOD_NODE, NodeLayoutType.NEIGHBORHOOD_PORT] = value;
            RelativeSpringStiffnessOf[NodeLayoutType.NEIGHBORHOOD_PORT, NodeLayoutType.NEIGHBORHOOD_NODE] = value;
         }
      }
      public float? RelativeSpringStiffnessMoleculeNeighborPort
      {
         get { return RelativeSpringStiffnessOf[NodeLayoutType.MOLECULE_NODE, NodeLayoutType.NEIGHBORHOOD_PORT]; }
         set
         {
            RelativeSpringStiffnessOf[NodeLayoutType.NEIGHBORHOOD_PORT, NodeLayoutType.MOLECULE_NODE] = value;
            RelativeSpringStiffnessOf[NodeLayoutType.MOLECULE_NODE, NodeLayoutType.NEIGHBORHOOD_PORT] = value;
         }
      }
      public float? RelativeSpringStiffnessObserverMolecule
      {
         get { return RelativeSpringStiffnessOf[NodeLayoutType.OBSERVER_NODE, NodeLayoutType.MOLECULE_NODE]; }
         set
         {
            RelativeSpringStiffnessOf[NodeLayoutType.MOLECULE_NODE, NodeLayoutType.OBSERVER_NODE] = value;
            RelativeSpringStiffnessOf[NodeLayoutType.OBSERVER_NODE, NodeLayoutType.MOLECULE_NODE] = value;
         }
      }
      public float? RelativeSpringStiffnessReactionMolecule
      {
         get { return RelativeSpringStiffnessOf[NodeLayoutType.REACTION_NODE, NodeLayoutType.MOLECULE_NODE]; }
         set
         {
            RelativeSpringStiffnessOf[NodeLayoutType.MOLECULE_NODE, NodeLayoutType.REACTION_NODE] = value;
            RelativeSpringStiffnessOf[NodeLayoutType.REACTION_NODE, NodeLayoutType.MOLECULE_NODE] = value;
         }
      }
      public float? RelativeSpringStiffnessContainerRemote
      {
         get { return RelativeSpringStiffnessOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE]; }
         set
         {
            RelativeSpringStiffnessOf[NodeLayoutType.CONTAINER_NODE, NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE] = value;
            RelativeSpringStiffnessOf[NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE, NodeLayoutType.CONTAINER_NODE] = value;
         }
      }

      public ForceLayoutConfiguration()
      {
         BaseGravitationalMass = 2F;
         BaseElectricalCharge = 50F;
         BaseSpringLength = 25F;
         BaseSpringStiffness = 0.1F;

         RelativeGravitationalMassOf = new float?[NUMBER_OF_GROUPS];
         RelativeElectricalChargeOf = new float?[NUMBER_OF_GROUPS];
         RelativeSpringLengthOf = new float?[NUMBER_OF_GROUPS, NUMBER_OF_GROUPS];
         RelativeSpringStiffnessOf = new float?[NUMBER_OF_GROUPS, NUMBER_OF_GROUPS];

         RelativeElectricalChargeOf[NodeLayoutType.LINKLESS_NODE] = 0.2F;
         RelativeElectricalChargeOf[NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE] = 0.2F;
         RelativeElectricalChargeOf[NodeLayoutType.CONTAINER_NODE] = 8F;
         RelativeElectricalChargeOf[NodeLayoutType.NEIGHBORHOOD_PORT] = 1F;
         RelativeElectricalChargeOf[NodeLayoutType.NEIGHBORHOOD_NODE] = 1F;
         RelativeElectricalChargeOf[NodeLayoutType.MOLECULE_NODE] = 2F;
         RelativeElectricalChargeOf[NodeLayoutType.OBSERVER_NODE] = 1F;
         RelativeElectricalChargeOf[NodeLayoutType.REACTION_NODE] = 1F;

         RelativeGravitationalMassOf[NodeLayoutType.LINKLESS_NODE] = 1F;
         RelativeGravitationalMassOf[NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE] = 0F;
         RelativeGravitationalMassOf[NodeLayoutType.CONTAINER_NODE] = 0.1F;
         RelativeGravitationalMassOf[NodeLayoutType.NEIGHBORHOOD_PORT] = 0F; //fixed in layout process
         RelativeGravitationalMassOf[NodeLayoutType.NEIGHBORHOOD_NODE] = 0F;
         RelativeGravitationalMassOf[NodeLayoutType.MOLECULE_NODE] = 0F;
         RelativeGravitationalMassOf[NodeLayoutType.OBSERVER_NODE] = 0F;
         RelativeGravitationalMassOf[NodeLayoutType.REACTION_NODE] = 0F;

         setRelativeSpringLengthAndStiffness(NodeLayoutType.CONTAINER_NODE, NodeLayoutType.CONTAINER_NODE, 2, 1);
         setRelativeSpringLengthAndStiffness(NodeLayoutType.CONTAINER_NODE, NodeLayoutType.NEIGHBORHOOD_NODE, 1, 1);
         setRelativeSpringLengthAndStiffness(NodeLayoutType.NEIGHBORHOOD_PORT, NodeLayoutType.NEIGHBORHOOD_NODE, 1, 1);
         setRelativeSpringLengthAndStiffness(NodeLayoutType.MOLECULE_NODE, NodeLayoutType.NEIGHBORHOOD_PORT, 1, 1);
         setRelativeSpringLengthAndStiffness(NodeLayoutType.MOLECULE_NODE, NodeLayoutType.OBSERVER_NODE, 1, 1);
         setRelativeSpringLengthAndStiffness(NodeLayoutType.MOLECULE_NODE, NodeLayoutType.REACTION_NODE, 1, 1);
         setRelativeSpringLengthAndStiffness(NodeLayoutType.CONTAINER_NODE, NodeLayoutType.REMOTE_CONTAINER_BOUNDARY_NODE, 2, 1);

         MaxIterations = 1000;
         Epsilon = 0.1F;
         InfinityDistance = 500F;
         ArrangementSpacingWidth = 100F;
         ArrangementSpacingHeight = 100F;

         LogPositions = false;
      }

      private void setRelativeSpringLengthAndStiffness(int i, int k, float length, float stiffness)
      {
         RelativeSpringLengthOf[i, k] = length;
         RelativeSpringStiffnessOf[i, k] = stiffness;
         RelativeSpringLengthOf[k, i] = length;
         RelativeSpringStiffnessOf[k, i] = stiffness;
      }
   }
}