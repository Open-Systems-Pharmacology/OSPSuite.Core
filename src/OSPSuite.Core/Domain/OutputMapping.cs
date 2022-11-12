using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public class OutputMapping
   {
      public virtual WeightedObservedData WeightedObservedData { get; set; }

      public virtual SimulationQuantitySelection OutputSelection { get; set; }

      public virtual Scalings Scaling { get; set; }

      public float Weight { get; set; } = Constants.DEFAULT_WEIGHT;

      /// <summary>
      ///    Returns the consolidated path of the mapped output (e.g without the name of the simulation)
      /// </summary>
      public virtual string OutputPath => OutputSelection?.Path ?? string.Empty;

      /// <summary>
      ///    Returns the full path of the mapped output (e.g with the name of the simulation)
      /// </summary>
      public virtual string FullOutputPath => OutputSelection?.FullQuantityPath ?? string.Empty;

      /// <summary>
      ///    Returns the underlying mapped output (Molecule Amount or Observer)
      /// </summary>
      public virtual IQuantity Output => OutputSelection?.Quantity;

      public virtual IModelCoreSimulation Simulation => OutputSelection?.Simulation;

      public IDimension Dimension => OutputSelection?.Quantity?.Dimension;

      public virtual bool UsesObservedData(DataRepository observerData) => Equals(WeightedObservedData.ObservedData, observerData);

      public virtual bool UsesSimulation(ISimulation simulation) => Equals(OutputSelection.Simulation, simulation);

      public virtual bool IsValid => Output != null && WeightedObservedData != null;

      public virtual OutputMapping Clone()
      {
         return new OutputMapping
         {
            OutputSelection = OutputSelection.Clone(),
            WeightedObservedData = WeightedObservedData.Clone(),
            Weight = Weight,
            Scaling = Scaling
         };
      }

      public void UpdateSimulation(ISimulation newSimulation) => OutputSelection.UpdateSimulation(newSimulation);

      public bool DimensionsAreConsistentForParameterIdentification(DataRepository observedData)
      {
         return Output != null && dimensionsAreConsistent(observedData);
      }

      public bool SimulationDimensionsAreConsistent(DataRepository observedData)
      {
         return Output == null || dimensionsAreConsistent(observedData);
      }

      public bool DimensionsAreConsistentForParameterIdentification()
      {
         return DimensionsAreConsistentForParameterIdentification(WeightedObservedData?.ObservedData);
      }

      private bool dimensionsAreConsistent(DataRepository observedData)
      {
         if (observedData == null)
            return false;

         var observationColumn = observedData.FirstDataColumn();

         if (Output.Dimension == null || observationColumn.Dimension == null)
            return false;

         if (Output.Dimension == observationColumn.Dimension)
            return true;

         if (Output.IsConcentration() && observationColumn.IsConcentration())
            return true;

         if (Output.IsAmount() && observationColumn.IsAmount())
            return true;

         //not the same dimension but sharing the same base unit (for example for fraction and dimensionsless)
         if (Output.Dimension.BaseUnit.Name == observationColumn.Dimension.BaseUnit.Name)
            return true;

         return false;
      }
   }
}