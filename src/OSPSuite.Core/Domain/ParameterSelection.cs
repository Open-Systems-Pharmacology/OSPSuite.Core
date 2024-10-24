﻿using System;

namespace OSPSuite.Core.Domain
{
   public class ParameterSelection : SimulationQuantitySelection
   {
      [Obsolete("For serialization")]
      public ParameterSelection()
      {
      }

      public ParameterSelection(IModelCoreSimulation simulation, QuantitySelection quantitySelection) : base(simulation, quantitySelection)
      {
      }

      public ParameterSelection(IModelCoreSimulation simulation, string parameterPath) : this(simulation, new QuantitySelection(parameterPath, QuantityType.Parameter))
      {
      }

      public virtual IParameter Parameter => Quantity as IParameter;
      
      public new ParameterSelection Clone()
      {
         return new ParameterSelection(Simulation, QuantitySelection.Clone());
      }
   }
}