using System;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public class SimulationQuantitySelection
   {
      private IQuantity _quantity;
      private string _fullQuantityPath;
      public virtual ISimulation Simulation { get; private set; }
      
      public virtual string SimulationId { get; set; }
      public virtual QuantitySelection QuantitySelection { get; }
      public virtual IDimension Dimension => Quantity?.Dimension;

      [Obsolete("For serialization")]
      public SimulationQuantitySelection()
      {
      }

      public SimulationQuantitySelection(ISimulation simulation, QuantitySelection quantitySelection)
      {
         Simulation = simulation;
         QuantitySelection = quantitySelection;
      }

      public virtual bool IsValid => Quantity != null;

      /// <summary>
      ///    Returns the consolidated path of the underlying quantity (e.g. without the name of the simulation)
      /// </summary>
      public virtual string Path => QuantitySelection?.Path ?? string.Empty;

      public virtual string[] PathArray => Path.ToPathArray();

      public virtual IQuantity Quantity
      {
         get
         {
            if (Simulation == null)
               return null;

            if (_quantity == null)
               setQuantity();

            return _quantity;
         }
      }

      private void setQuantity()
      {
         _quantity = new ObjectPath(PathArray).TryResolve<IQuantity>(Simulation.Model.Root);
      }

      /// <summary>
      ///    Returns the full path of the underlying quantity (e.g. with the name of the simulation)
      /// </summary>
      public virtual string FullQuantityPath
      {
         get
         {
            if (_fullQuantityPath != null)
               return _fullQuantityPath;

            buildFullQuantityPath();
            return _fullQuantityPath ?? string.Empty;
         }
      }

      private void buildFullQuantityPath()
      {
         if (Simulation == null)
            return;

         var pathArray = PathArray.ToList();
         pathArray.Insert(0, Simulation.Name);

         _fullQuantityPath = pathArray.ToPathString();
      }

      public override bool Equals(object obj)
      {
         if (obj == null)
            return false;

         if (this == obj)
            return true;

         if (obj.GetType() != GetType())
            return false;

         return Equals((SimulationQuantitySelection) obj);
      }

      protected bool Equals(SimulationQuantitySelection other)
      {
         return Equals(Simulation, other.Simulation) && Equals(QuantitySelection, other.QuantitySelection);
      }

      public override int GetHashCode()
      {
         unchecked
         {
            return ((Simulation?.GetHashCode() ?? 0) * 397) ^ (QuantitySelection?.GetHashCode() ?? 0);
         }
      }

      public SimulationQuantitySelection Clone()
      {
         return new SimulationQuantitySelection(Simulation, QuantitySelection.Clone());
      }

      public void UpdateSimulation(ISimulation newSimulation)
      {
         Simulation = newSimulation;
         setQuantity();
         buildFullQuantityPath();
      }
   }
}