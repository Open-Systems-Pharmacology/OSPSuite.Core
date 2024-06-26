﻿using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.SimModel;

namespace OSPSuite.Core
{
   internal class SimModelManagerForSpecs : SimModelManagerBase
   {
      public SimModelManagerForSpecs(ISimModelExporter simModelExporter, ISimModelSimulationFactory simModelSimulationFactory) : base(simModelExporter, simModelSimulationFactory)
      {
      }

      public Simulation CreateSimulation(IModelCoreSimulation modelCoreSimulation)
      {
         return CreateSimulation(_simModelExporter.ExportSimModelXml(modelCoreSimulation, SimModelExportMode.Optimized));
      }

      public IReadOnlyList<ParameterProperties> SetVariableParameters(Simulation simulation, IReadOnlyList<string> variablePaths) => base.SetVariableParameters(simulation, variablePaths, calculateSensitivities: false);

      public  IReadOnlyList<SpeciesProperties> SetVariableSpecies(Simulation simulation, IReadOnlyList<string> variablePaths)
      {
         return SetVariableMolecules(simulation, variablePaths);
      }
   }
}