using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Starter.Domain
{
   public class TestSimulation : ModelCoreSimulation, ISimulation
   {

      public double? TotalDrugMassPerBodyWeightFor(string compoundName)
      {
         return null;
      }

      public IEnumerable<T> All<T>() where T : class, IEntity
      {
         var root = Model?.Root;
         return root == null ? Enumerable.Empty<T>() : root.GetAllChildren<T>();
      }

      /// <summary>
      ///    Returns the endtime of the simulation in kernel unit
      /// </summary>
      public virtual double? EndTime
      {
         get { return OutputSchema.Intervals.Select(x => x.EndTime.Value).Max(); }
      }

      public IEnumerable<CurveChart> Charts
      {
         get
         {
            yield break ;
         }
      }

      public OutputSelections OutputSelections => Settings.OutputSelections;
      public OutputSchema OutputSchema => Settings.OutputSchema;
      public ISimulationSettings Settings => BuildConfiguration.SimulationSettings;

      public ISimulationSettings SimulationSettings => BuildConfiguration.SimulationSettings;

      public IReadOnlyList<string> CompoundNames => BuildConfiguration.AllPresentMolecules().Select(x => x.Name).ToList();

      public IReactionBuildingBlock Reactions
      {
         get => BuildConfiguration.Reactions;
         set => BuildConfiguration.Reactions = value;
      }

      public void AddChartTemplate(CurveChartTemplate chartTemplate)
      {
         SimulationSettings.AddChartTemplate(chartTemplate);
      }

      public void RemoveChartTemplate(string chartTemplateName)
      {
         SimulationSettings.RemoveChartTemplate(chartTemplateName);
      }

      public IEnumerable<CurveChartTemplate> ChartTemplates => SimulationSettings.ChartTemplates;

      public CurveChartTemplate DefaultChartTemplate => SimulationSettings.DefaultChartTemplate;

      public bool IsLoaded { get; set; }
      public void RemoveAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         
      }

      public IEnumerable<ISimulationAnalysis> Analyses { get; } = new List<ISimulationAnalysis>();
      public void AddAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         
      }

      public bool HasUpToDateResults => true;
      public bool ComesFromPKSim => false;

      public bool UsesObservedData(DataRepository observedData)
      {
         return true;
      }
   }
}