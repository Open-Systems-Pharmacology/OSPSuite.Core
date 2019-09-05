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

      public IEnumerable<CurveChart> Charts
      {
         get { yield break; }
      }

      public OutputSchema OutputSchema => Settings.OutputSchema;
      public ISimulationSettings Settings => BuildConfiguration.SimulationSettings;

      public IReadOnlyList<string> CompoundNames => BuildConfiguration.AllPresentMolecules().Select(x => x.Name).ToList();

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