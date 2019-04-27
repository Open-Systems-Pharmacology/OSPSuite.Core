using System.Collections.Generic;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   /// <summary>
   ///    Core class for the export to the simmodel calculation kernel.
   ///    Contains all data objects from which a  simmodel xml could be generated
   /// </summary>
   public class SimulationExport
   {
      private int _currentId = 1;
      public int Version { get; }

      public SimulationExport()
      {
         ParameterList = new Cache<int, ParameterExport>(x => x.Id);
         VariableList = new Cache<int, VariableExport>(x => x.Id);
         ObserverList = new List<QuantityExport>();
         EventList = new List<EventExport>();
         FormulaList = new List<FormulaExport>();
         Version = Constants.SIM_MODEL_XML_VERSION;
      }

      /// <summary>
      ///    Gets or sets the delimiter to separate path elements
      /// </summary>
      /// <value>The object path element delimiter.</value>
      public string ObjectPathDelimiter { get; set; }

      /// <summary>
      ///    Gets or sets the random seed used in the simulation
      /// </summary>
      /// <value>The random seed.</value>
      public double RandomSeed { set; get; }

      /// <summary>
      ///    Gets or sets the list of parameters in the simulation.
      /// </summary>
      /// <value>The parameter list.</value>
      public ICache<int, ParameterExport> ParameterList { get; set; }

      public ICache<int, VariableExport> VariableList { get; set; } //Id, Export12e
      public IList<QuantityExport> ObserverList { get; set; }
      public IList<EventExport> EventList { get; set; }
      public IList<FormulaExport> FormulaList { get; set; }
      public SolverSettingsExport Solver { get; set; }
      public OutputSchemaExport OutputSchema { get; set; }

      public int NewId()
      {
         return _currentId++;
      }

      private ParameterExport addSolverParameter(string solverParameterName, double parameterValue)
      {
         return addSolverParameter(solverParameterName, parameterValue.ConvertedTo<string>());
      }

      private ParameterExport addSolverParameter(string solverParameterName, string parameterValue)
      {
         var explicitFormulaExport = new ExplicitFormulaExport {Id = NewId(), Equation = parameterValue};
         FormulaList.Add(explicitFormulaExport);
         var parameterExport = new ParameterExport(NewId(), solverParameterName, explicitFormulaExport.Id)
         {
            EntityId = solverParameterName,
            Path = solverParameterName
         };
         ParameterList.Add(parameterExport);
         return parameterExport;
      }

      public void AddSimulationConfiguration(ISimulationSettings simulationSettings)
      {
         addSolverSettings(simulationSettings.Solver);
         addOutputSchema(simulationSettings.OutputSchema);
         RandomSeed = simulationSettings.RandomSeed;
      }

      private void addSolverSettings(SolverSettings settings)
      {
         var solverSettingsExport = new SolverSettingsExport {Name = settings.Name};

         var solverPara = addSolverParameter("AbsTol", settings.AbsTol);
         solverSettingsExport.AbsTol = solverPara.Id;
         solverPara = addSolverParameter("RelTol", settings.RelTol);
         solverSettingsExport.RelTol = solverPara.Id;
         solverPara = addSolverParameter("H0", settings.H0);
         solverSettingsExport.H0 = solverPara.Id;
         solverPara = addSolverParameter("HMin", settings.HMin);
         solverSettingsExport.HMin = solverPara.Id;
         solverPara = addSolverParameter("HMax", settings.HMax);
         solverSettingsExport.HMax = solverPara.Id;
         solverPara = addSolverParameter("MxStep", settings.MxStep);
         solverSettingsExport.MxStep = solverPara.Id;
         solverPara = addSolverParameter("UseJacobian", settings.UseJacobian ? 1 : 0);
         solverSettingsExport.UseJacobian = solverPara.Id;

         var extraOptions = new List<SolverOptionExport>();
         solverSettingsExport.SolverOptions = extraOptions;
         Solver = solverSettingsExport;
      }

      private void addOutputSchema(OutputSchema schema)
      {
         var schemaExport = new OutputSchemaExport();
         schema.Intervals.Each(x => schemaExport.OutputIntervals.Add(outputIntervalExportFrom(x)));
         schema.TimePoints.Each(x => schemaExport.OutputTimes.Add(x));
         OutputSchema = schemaExport;
      }

      private static OutputIntervalExport outputIntervalExportFrom(OutputInterval outputInterval)
      {
         return new OutputIntervalExport
         {
            StartTime = outputInterval.StartTime.Value,
            EndTime = outputInterval.EndTime.Value,
            Resolution = outputInterval.Resolution.Value
         };
      }
   }
}