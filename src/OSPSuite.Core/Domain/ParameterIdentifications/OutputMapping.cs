using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class OutputMapping
   {
      public virtual WeightedObservedData WeightedObservedData { get; set; }
      public virtual SimulationQuantitySelection OutputSelection { get; set; }
      public virtual Scalings Scaling { get; set; }
      public float Weight { get; set; }

      /// <summary>
      ///    Returns the consolidated path of the mapped output (e.g without the name of the simulation)
      /// </summary>
      public virtual string OutputPath => OutputSelection?.Path ?? string.Empty;

      /// <summary>
      ///    Returns the full path of the mapped output (e.g with the name of the simulation)
      /// </summary>
      public virtual string FullOutputPath => OutputSelection?.FullQuantityPath ?? string.Empty;


      /// <summary>
      ///    Returns the underying mapped output (Molecule Amount or Observer)
      /// </summary>
      public virtual IQuantity Output => OutputSelection?.Quantity;

      public OutputMapping()
      {
         Weight = Constants.DEFAULT_WEIGHT;
      }

      public virtual ISimulation Simulation => OutputSelection?.Simulation;
      public IDimension Dimension => OutputSelection?.Quantity?.Dimension;

      public virtual bool UsesObservedData(DataRepository observerData)
      {
         return Equals(WeightedObservedData.ObservedData, observerData);
      }

      public virtual bool UsesSimulation(ISimulation simulation)
      {
         return Equals(OutputSelection.Simulation, simulation);
      }

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

      public void UpdateSimulation(ISimulation newSimulation)
      {
         OutputSelection.UpdateSimulation(newSimulation);
      }

      public bool DimensionsAreConsistent(DataRepository observedData)
      {
         if (observedData == null || Output == null)
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
         if(Output.Dimension.BaseUnit.Name == observationColumn.Dimension.BaseUnit.Name)
            return true;

         return false;
      }

      public bool DimensionsAreConsistent()
      {
         return DimensionsAreConsistent(WeightedObservedData?.ObservedData);
      }
   }
}