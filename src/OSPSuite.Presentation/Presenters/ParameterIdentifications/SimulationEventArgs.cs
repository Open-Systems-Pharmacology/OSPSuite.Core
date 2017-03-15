using System;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class SimulationEventArgs : EventArgs
   {
      public ISimulation Simulation { get; }

      public SimulationEventArgs(ISimulation simulation)
      {
         Simulation = simulation;
      }
   }
}